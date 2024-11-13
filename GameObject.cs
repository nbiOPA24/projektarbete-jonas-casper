using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
#region GameObject Class

public abstract class GameObject
{
    public int TextureSize{get; set;}
    public Texture2D Texture{get;set;}
    public Vector2 Position{get;set;}
    public bool IsActive {get;set;} = true;

    protected GameObject(int textureSize,Texture2D texture, Vector2 position)
    {
        TextureSize = textureSize;
        Texture = texture;
        Position = position;
    }

    public Rectangle hitbox
    {
        get
        {
            return new Rectangle((int)Position.X, (int) Position.Y, TextureSize, TextureSize);
        }
    }

    public abstract void LoadContent(ContentManager contentManager);
    public abstract void Update(GameTime gameTime);
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive && Texture != null)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
#endregion
#region Player Class
public class Player : GameObject
{
    public int BaseHealth{get; set;}
    public int BaseDamage {get; set;}
    public int BaseShield {get; set;}
    public float Speed {get; set;}
    public SoundEffect LaserSound {get; set;}
    public float ShootCooldown{get; set;} = 0.3f;
    public float ShootTimer{get; set;} = 0;
    private List<Projectile> projectiles = new List<Projectile>();
    private Texture2D projectileTexture;

    public Player(int textureSize, Texture2D texture, Vector2 position,  int baseHealth, int baseDamage, int baseShield, float speed, SoundEffect laserSound)
    : base(textureSize, texture, position)

    {
        BaseHealth = baseHealth;
        BaseDamage = baseDamage;
        BaseShield = baseShield;
        Speed = speed;
        LaserSound = laserSound;
    }
    public override void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("player");
        LaserSound = content.Load<SoundEffect>("laserSound");
        projectileTexture = content.Load<Texture2D>("laserGreen");
    }

    public override void Update(GameTime gameTime)
    {
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
        playerPosition.Y += movementSpeed;

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
            float projectileSpeed = 50;
            //Lägger till projektilen i listan projectiles
            projectiles.Add(new Projectile(projectileTexture, projectileStartPosition, direction, projectileSpeed, 10));
            //Spelar skjutljudet
            LaserSound.Play();
        }
        foreach (var projectile in projectiles)
            projectile.Update(gameTime);

        projectiles.RemoveAll(p => !p.IsActive);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        foreach (var projectile in projectiles)
        {
            projectile.DrawProjectile(spriteBatch);
        }
    }
    
}
#endregion
#region SmallEnemy Class
public class SmallEnemy : GameObject
{
    public int BaseHealth {get; set;}
    public int BaseDamage {get; set;}
    public int Speed {get; set;}
    public int ScreenWidth {get; set;}
    public float ElapsedTime {get; set;}
    
        
    public SmallEnemy(int textureSize, Texture2D texture, Vector2 position, int baseHealth, int baseDamage, int speed, int screenWidth, float elapsedTime)
    : base(textureSize, texture, position)
    {
        BaseHealth = baseHealth;
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
         //UpdateHitbox();  Kolla på detta 
        }
    }
#endregion
#region MediumEnemy Class
public class MediumEnemy : GameObject
{
    public int BaseHealth {get; set;}
    public int BaseDamage {get; set;}
    public int Speed {get; set;}
    public int ScreenWidth {get; set;}
    public float ElapsedTime {get; set;}
    public SoundEffect LaserSound {get; set;}
    public float shootCooldown = 2f;
    public float timeSinceLastShot = 0f;
    public List<MediumEnemyProjectile> mediumEnemyProjectiles;
    public Texture2D projectileTexture;
    public MediumEnemy(int textureSize, Texture2D texture, Vector2 position, int baseHealth, int baseDamage, int speed, int screenWidth, float elapsedTime, SoundEffect laserSound)
    : base (textureSize, texture, position)
    {
        BaseHealth = baseHealth;
        BaseDamage = baseDamage;
        Speed = speed;
        ScreenWidth = screenWidth;
        ElapsedTime = elapsedTime;
        LaserSound = laserSound;
        mediumEnemyProjectiles = new List<MediumEnemyProjectile>();
    }
    public override void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("antmaker");
        LaserSound = content.Load<SoundEffect>("laserSound");
        projectileTexture = content.Load<Texture2D>("laserRed");
    }
    public override void Update(GameTime gameTime)
    {
        
    }
}
#endregion