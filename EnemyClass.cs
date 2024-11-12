//konstruktor för klassen enemy
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using static Projectile;


public abstract class Enemy
{
    
    public Vector2 Position {get; set;}
    
    public Texture2D Texture { get; set;}
    public string Name {get; set;}
    public int Health {get; set;}
    public int Attack {get; set;} 
    public int Shield {get; set;}
    public float Speed {get; set;}
    public bool IsActive {get; set;} = true;
    public Hitbox Hitbox {get; set;}
    

    public Enemy(Vector2 startPosition,Texture2D texture, string name, int health, int attack, int shield, float speed)
    {
        Texture = texture;
        Position = startPosition;
        Name = name; 
        Health = health;
        Attack = attack;
        Shield = shield;
        Speed = speed;
        Hitbox = new Hitbox(Position, Texture);
    }

    public void UpdateHitbox()
    {
        Hitbox.Update(Position);
    }
    
}
class SmallEnemy : Enemy //SmallEnemy som ärver basegenskapaer ifrån Enemy
{
    private int screenWidth; 
    private float elapsedTime; 
    public SmallEnemy(Vector2 startPosition,Texture2D texture, int screenWidth)
        : base(startPosition, texture, "SmallEnemy", 40, 10, 0, 10)
    {
        this.screenWidth = screenWidth; //Tar in våran screenWidth
    }
 
    public void MoveDownSmoothly(GameTime gameTime)
    {
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        //SmalEnemys rörelseemönster, Math.Sin skapar en mjukvågrörelse. 
        float xMovement = (float)Math.Sin(elapsedTime * 2) * 1.5f;
        float yMovement = Speed * 0.1f;

        Position = new Vector2 //här skapas även en ny Vector, och anävder en MathHelper som ser till att Enemyn inte kan lämna skärmen på Y-axeln
        (
            MathHelper.Clamp(Position.X + xMovement, 0, screenWidth - Texture.Width),
            Position.Y + yMovement
        );
        UpdateHitbox(); 
    }
    
     
}

class MediumEnemy : Enemy //Mediumenemy som ärver basegenskaper ifrån Enemy
{
    private int screenWidth;
    private float elapsedTime;
    public List<MediumEnemyProjectile> mediumEnemyProjectiles;
    private float shootCooldown = 2f; //En CoolDown för att sätta en timer på hur många sekunder mellan varje skott M-E skjuter.
    private float timeSinceLastShot = 0f;
    private SoundEffect shootSound;
    public Vector2 Vector2 { get; }
    public Texture2D MediumEnemyTexture { get; }

    public MediumEnemy(Vector2 startPosition,Texture2D texture, int screenWidth, SoundEffect shootsound)
         : base (startPosition, texture,"MediumEnemy", 100, 15, 5, 5)
    {
       this.screenWidth = screenWidth;
       this.shootSound = shootsound;                              
       mediumEnemyProjectiles = new List<MediumEnemyProjectile>();
    }
    public void Update(GameTime gametime, Player player, Vector2 playerPosition, Texture2D laserRedTexture)
    {
        timeSinceLastShot += (float)gametime.ElapsedGameTime.TotalSeconds;
        //Cooldown fuktion som håller koll när senaste skottet avlossades är det mer eller lika med två sekunder skjuter M-E.
        if(timeSinceLastShot >= shootCooldown)
        {
            MediumEnemyShoot(playerPosition, laserRedTexture);
            timeSinceLastShot = 0f;
        }
        foreach (var projectile in mediumEnemyProjectiles) //Lista som iterera projektilerna som avlossas. 
        {
            projectile.Update(gametime);
            
            if(projectile.Hitbox.Bounds.Intersects(player.Hitbox.Bounds)) //Träffar en projektil PLayernsHitbox skadar den på Playern
            {
                player.BaseHealth -= projectile.Damage;
                projectile.IsActive = false;
            }

        }
        mediumEnemyProjectiles.RemoveAll(p => !p.IsActive); // Om M-Es projektil antigen träffar spelaren eller åker utanför skärmen så raderas den ut.
        
        UpdateHitbox();
    }
    public void MoveDownSmoothlyFaster(GameTime gameTime)
    {
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Rörelsemönster för M-E samma som för S-E bara att den rör sig i saktare tempo vertikalt.
        float xMovement = (float)Math.Sin(elapsedTime * 2) * 4.5f; 
        float yMovement = Speed * 0.1f;

        Position = new Vector2
        (
            MathHelper.Clamp(Position.X + xMovement, 0, screenWidth - Texture.Width),
            Position.Y + yMovement
        );
        UpdateHitbox(); 
    }
    public void MediumEnemyShoot(Vector2 playerPosition, Texture2D laserRedTexture) //M-E projektil egenskaper. 
    {
        Vector2 projectilePosition = new Vector2(Position.X + Texture.Width / 2, Position.Y + Texture.Height);
       
        Vector2 direction = playerPosition - projectilePosition; //Sätter en vector som rör sig emot spelaren position när skottet avlossas ifrån M-E
        direction.Normalize();
        float speed = 300f;    //Här sätts projektilen Speed och Damage.
        int damage = 10;

        var newProjectile = new MediumEnemyProjectile(laserRedTexture, projectilePosition, direction, speed, damage, Hitbox);
        mediumEnemyProjectiles.Add(newProjectile);
        shootSound.Play(); 
    }
    public void DrawMediumEnemyAttack(SpriteBatch spriteBatch) 
    {
        foreach(var projectile in mediumEnemyProjectiles)
        {
            projectile.DrawPlayerAttack(spriteBatch);
        }
        
        Hitbox.Update(Position);
    }
}
class BigEnemy : Enemy // BigEnemy som ärver basegenskaper ifrån Enemy
{
    private float elapsedTime;
    private int screenWidth;
    private bool movingRight = true;
    
    public BigEnemy(Vector2 startPosition,Texture2D texture, int screenWidth)
      : base (startPosition, texture,"BigEnemy", 150, 20, 15, 2)
         {
             this.screenWidth = screenWidth;
         }
    public void MoveSideToSide(GameTime gameTime) //B-Es Rörelse-Metod
    {
        
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
         
        if (movingRight)
        {
        // Flytta till höger
        Position = new Vector2(Position.X + Speed, Position.Y); // Skapa en ny Vector2
        // Kontrollera om den når högra kanten
        if (Position.X + Texture.Width >= screenWidth)
        {
            Position = new Vector2(screenWidth - Texture.Width, Position.Y); // går till högra kanten
            movingRight = false; // Byt riktning
        }
        }
        else 
        {
        // Flytta till vänster
        Position = new Vector2(Position.X - Speed, Position.Y); // Skapa en ny Vector2
        // Kontrollera om den når vänstra kanten
        if (Position.X <= 0)
        {
            Position = new Vector2(0, Position.Y); // går till vänstra kanten
            movingRight = true; // Byt riktning
        }
        }
        UpdateHitbox();
    }
}    



