using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using JcGame;
using System.Diagnostics.Contracts;

#region GameObject Class
//Basklassen som alla våra subklasser ärver ifrån
public abstract class GameObject
{
    //Fält med egenskaper för alla subklasser
    public int TextureSize{get;set;}
    public Texture2D Texture{get;set;}
    public Vector2 Position{get;set;}
    public bool IsActive {get;set;} = true;
    public int BaseHealth {get;set;} = 0;
    public Hitbox Hitbox {get;set;}
    public float Speed {get;set;}
    
    //Konstruktor för Basklassen
    protected GameObject(int textureSize,Texture2D texture, Vector2 position, int baseHealth, float speed)
    {
        TextureSize = textureSize;
        Texture = texture;
        Position = position;
        BaseHealth = baseHealth;
        Speed = speed;

        int width = texture?.Width ?? textureSize;
        int height = texture?.Height ?? textureSize;

        Hitbox = new Hitbox(position, width, height);
    }
    //Logik för att skapa hitboxes
   
    //Vilka metoder som våra basklasser skas ärva
    public abstract void LoadContent(ContentManager contentManager);
    public virtual void Update(GameTime gameTime)
    {
        Position += new Vector2(0, Speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        Hitbox.Update(Position);            
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive && Texture != null)
        {
            Rectangle rectangle = new Rectangle((int) Position.X, (int) Position.Y, TextureSize, TextureSize);
            spriteBatch.Draw(Texture, rectangle, Color.White);
        }
    }
}
#endregion

#region Player Class
public class Player : GameObject
{
    //Egenskaper för player
    public int BaseDamage {get; set;}
    public int BaseShield {get; set;}
    public SoundEffect LaserSound {get; set;}
    public float ShootCooldown{get; set;} = 0.3f;
    public float ShootTimer{get; set;} = 0;
    private List<Projectile> projectiles = new List<Projectile>();
    private Texture2D projectileTexture;
    private Game1 game;

    //Konstruktor för player
    public Player(int textureSize, Texture2D texture, Vector2 position, int baseHealth, float speed, int baseDamage, int baseShield, SoundEffect laserSound, Game1 game)
    : base(textureSize, texture, position, baseHealth, speed)

    {
        BaseDamage = baseDamage;
        BaseShield = baseShield;
        LaserSound = laserSound;
        this.game = game;
    }
    
    //PLayer LoadContent håller playerinformation som ska laddas in i game1 LoadContent
    public override void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("player");
        projectileTexture = content.Load<Texture2D>("laserGreen");
        LaserSound = content.Load<SoundEffect>("laserSound");
    }
    // Player Update håller playerlogik som ska laddas in i Game1 LoadContent
    public override void Update(GameTime gameTime)
    {
        if (BaseHealth <= 0)
        {
            if (BaseHealth <= 0)
            {
                game.Exit();
            }
        }   
        //Logik för hur spelaren rör på sig, "pil upp" för uppåt tex
        float movementSpeed = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        //Sparar var spelaren är 
        var playerPosition = Position;
        //Läser av vilken tangentsom trycks ned
        var keyboardState = Keyboard.GetState();
    
        //Hur man styr spelaren
        if (keyboardState.IsKeyDown(Keys.Left))
        playerPosition.X -= movementSpeed;

        if (keyboardState.IsKeyDown(Keys.Right))
        playerPosition.X += movementSpeed;

        if (keyboardState.IsKeyDown(Keys.Down))
        playerPosition.Y += movementSpeed;

        if (keyboardState.IsKeyDown(Keys.Up))
        playerPosition.Y -= movementSpeed;
        
        //Escape stänger ned spelet
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            game.Exit();

        //Logik för hur ofta projektiler kan spawna
        ShootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        //Kod för hur man skjuter samt skapandet av projektiler och hur de beter sig
        if (keyboardState.IsKeyDown(Keys.Space) && ShootTimer >= ShootCooldown)
        {
            ShootTimer = 0F;
            //Ser till så att projektilen skjuts ifrån mitten av spelaren
            float xOffset = Texture.Width / 2;
            float yOffset = Texture.Height / 2;
            Vector2 projectileStartPosition = new Vector2(Position.X + xOffset, Position.Y + yOffset);
            
            //Projektilens riktning samt hastighet
            Vector2 direction = new Vector2(0, -20);
            float projectileSpeed = 50f;
            int textureSize = 10;
            int baseHealth = 0;
            
            //Lägger till projektilen i listan projectiles
            projectiles.Add(new Projectile(textureSize, projectileStartPosition, projectileTexture, baseHealth, direction, projectileSpeed, BaseDamage));
            //Spelar skjutljudet
            LaserSound.Play();
        }
        foreach (var projectile in projectiles)
            projectile.Update(gameTime);

        projectiles.RemoveAll(p => !p.IsActive);

        Position = playerPosition;

        foreach (var obj in game.nonPlayerObjects)
        {
            if (IsActive && obj.IsActive && Hitbox.Intersects(obj.Hitbox))
            {
                if (obj is HeartItem heartItem)
                {
                    BaseHealth = BaseHealth;
                }
                else if(obj is SmallEnemy smallEnemy)
                {
                    BaseHealth = BaseHealth - smallEnemy.BaseDamage;
                    obj.IsActive = false;
                }
                else if(obj is MediumEnemy mediumEnemy)
                {
                    BaseHealth = BaseHealth - mediumEnemy.BaseDamage;
                    obj.IsActive = false;
                }
                else if (obj is BigEnemy bigEnemy)
                {
                    BaseHealth = BaseHealth - bigEnemy.BaseDamage;
                    obj.IsActive = false;
                }
            }
        }
    }
    public void DrawProjectile(SpriteBatch spriteBatch) //Ritar ut projectilerna.
    // OBS:för både Player och Enemys projectiler, Behöver byta namn för mer klarhet just nu kan man tolka det som att den bara ritar ut Playerns Attck medn används även för Enemys.
    {
        if (IsActive) 
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
#endregion
#region SmallEnemy Class
public class SmallEnemy : GameObject
{
    public int BaseDamage {get; set;}
    
    public int ScreenWidth {get; set;}
    public float ElapsedTime {get; set;}
            
    public SmallEnemy(int textureSize, Texture2D texture, Vector2 position, int baseHealth, int baseDamage, int speed, int screenWidth, float elapsedTime)
    : base(textureSize, texture, position, baseHealth, speed)
    {
        BaseDamage = baseDamage;
        Speed = speed;
        ScreenWidth = screenWidth;
        ElapsedTime = elapsedTime;
    }
    public override void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("eyelander");   
    }
    public override void Update(GameTime gameTime)
    {
        ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        //SmalEnemys rörelseemönster, Math.Sin skapar en mjukvågrörelse. 
        float xMovement = (float)Math.Sin(ElapsedTime * 2) * 1.5f;
        float yMovement = Speed * 0.1f;

        Position = new Vector2 //här skapas även en ny Vector, och anävder en MathHelper som ser till att Enemyn inte kan lämna skärmen på Y-axeln
        (
        MathHelper.Clamp(Position.X + xMovement, 0, ScreenWidth - Texture.Width),
        Position.Y + yMovement
        );
    }
}
#endregion
#region MediumEnemy Class
public class MediumEnemy : GameObject
{
    public int BaseDamage {get; set;}
    public int ScreenWidth {get; set;}
    public float ElapsedTime {get; set;}
    public SoundEffect LaserSound {get; set;}
    public float shootCooldown = 2f;
    public float timeSinceLastShot = 0f;
    //public List<MediumEnemyProjectile> mediumEnemyProjectiles;
    public Texture2D projectileTexture;
    public MediumEnemy(int textureSize, Texture2D texture, Vector2 position, int baseHealth, int baseDamage, int speed, int screenWidth, float elapsedTime, SoundEffect laserSound)
    : base (textureSize, texture, position, baseHealth, speed)
    {
        BaseDamage = baseDamage;
        Speed = speed;
        ScreenWidth = screenWidth;
        ElapsedTime = elapsedTime;
        LaserSound = laserSound;
        //mediumEnemyProjectiles = new List<MediumEnemyProjectile>();
    }
    
    public override void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("antmaker");
        LaserSound = content.Load<SoundEffect>("laserSound");
        projectileTexture = content.Load<Texture2D>("laserRed");
    }
    public override void Update(GameTime gameTime)
    {
        ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Rörelsemönster för M-E samma som för S-E bara att den rör sig i saktare tempo vertikalt.
        float xMovement = (float)Math.Sin(ElapsedTime * 2) * 4.5f; 
        float yMovement = Speed * 0.1f;
        Position = new Vector2
        (
            MathHelper.Clamp(Position.X + xMovement, 0, ScreenWidth - Texture.Width),
            Position.Y + yMovement
        );
    }
    public void DrawProjectile(SpriteBatch spriteBatch) //Ritar ut projectilerna.
    // OBS:för både Player och Enemys projectiler, Behöver byta namn för mer klarhet just nu kan man tolka det som att den bara ritar ut Playerns Attck medn används även för Enemys.
    {
        if (IsActive) 
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
#endregion
#region BigEnemy Class

public class BigEnemy : GameObject
{
    public int BaseDamage { get; set;} 
    public int ScreenWidth { get; set; }
    public float ElapsedTime { get; set; }
    private bool movingRight = true;
    public BigEnemy(int textureSize, Texture2D texture, Vector2 position, int baseHealth, int baseDamage, int speed, int screenWidth, float elapsedTime)
    : base(textureSize, texture, position, baseHealth, speed)
    {
        BaseHealth = baseHealth;
        BaseDamage = baseDamage;
        Speed = speed;
        ScreenWidth = screenWidth;
        ElapsedTime = elapsedTime;
    }
   
    public override void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("enemyUFO");
    }
    public override void Update(GameTime gameTime)
    {
        ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (movingRight)
        {
            // Flytta till höger
            Position = new Vector2(Position.X + Speed, Position.Y); // Skapa en ny Vector2
            // Kontrollera om den når högra kanten
            if (Position.X + Texture.Width >= ScreenWidth)
            {
                Position = new Vector2(ScreenWidth - Texture.Width, Position.Y); // går till högra kanten
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
    }
     public void DrawProjectile(SpriteBatch spriteBatch) //Ritar ut projectilerna.
    // OBS:för både Player och Enemys projectiler, Behöver byta namn för mer klarhet just nu kan man tolka det som att den bara ritar ut Playerns Attck medn används även för Enemys.
    {
        if (IsActive) 
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
#endregion
#region HeartItem Class
public class HeartItem : GameObject
{
    public int HealthBoost = 10;

    public HeartItem (int textureSize, Vector2 position, Texture2D texture, int healthBoost,int baseHealth) 
    : base (textureSize, texture, position, baseHealth, 0)
    {
        HealthBoost = healthBoost;
    }
    //Laddar in rätt texture
    public override void LoadContent(ContentManager content)
    {
        TextureSize = 10;
        Texture = content.Load<Texture2D>("heartTexture");
    }
    public override void Update(GameTime gameTime)
    {
        //Lämnas tom då HeartItem inte behöver updateras
    }
}
#endregion
#region Projectiels Class
public class Projectile : GameObject
{
    public Vector2 Direction { get; set; }
    public float Speed { get; set; }
    public int Damage { get; set; }
    
    public Projectile(int textureSize, Vector2 position, Texture2D texture, int baseHealth, Vector2 direction, float speed, int damage)
    : base(textureSize, texture, position, 0, speed)
    {
        Direction = direction;
        Speed = speed;
        Damage = damage;
    }
     public override void Update(GameTime gametime) 
    {// Updatera position baserat på riktning och hastighet, om projektilen åker utanför sätts IsActive till false
         Position += Direction * Speed * (float)gametime.ElapsedGameTime.TotalSeconds;

        if (Position.X < 0 || Position.X > 1920 || Position.Y < 0 || Position.Y > 1080)
        {
             IsActive = false;
        }
    }

    public override void LoadContent(ContentManager content)
    {
        
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
    
}
#endregion
#region PlayerProjectiles Class
public class PlayerProjectile : Projectile
{   
    public Texture2D ProjectileTexture {get; set;}
    public PlayerProjectile(int textureSize, Vector2 position, Texture2D texture, Vector2 direction, float speed, int damage)
        : base(textureSize, position, texture, 0, direction, speed, damage)
    {
        
    }   
    public override void LoadContent(ContentManager content)
    {
        ProjectileTexture = content.Load<Texture2D>("laserGreen");
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
    


}

#endregion

#region  EnemyPrjectile Class
public class EnemyProjectile : Projectile
{
    public EnemyProjectile (int textureSize, Vector2 position, Texture2D texture, int baseHealth, Vector2 direction, float speed, int damage)
        : base (textureSize, position, texture, 0, direction, speed, damage)
    {
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    public override void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("laserRed");
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}
        
#endregion
