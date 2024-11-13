using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

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

public class Player : GameObject
{
    public int BaseHealth{get; set;}
    public int BaseDamage {get; set;}
    public int BaseShield {get; set;}
    public float Speed {get; set;}
    public SoundEffect ShootSound {get; set;}
    public float ShootCooldown{get; set;}
    public float ShootTimer{get; set;}
    private List<Projectile> projectiles = new List<Projectile>();
    private Texture2D projectileTexture;

    public Player(int textureSize, Texture2D texture, Vector2 position,  int baseHealth, int baseDamage, int baseShield, float speed, SoundEffect shootSound, float shootCooldown, float shootTimer)
    : base(textureSize, texture, position)

    {
        BaseHealth = baseHealth;
        BaseDamage = baseDamage;
        BaseShield = baseShield;
        Speed = speed;
        ShootSound = shootSound;
        ShootCooldown = shootCooldown;
        ShootTimer = shootTimer;
    }
    public override void LoadContent(ContentManager content)
    {
        Texture = content.Load<Texture2D>("player");
        ShootSound = content.Load<SoundEffect>("ShootSound");
        projectileTexture = content.Load<Texture2D>("laserGreen");
    }

    public override void Update(GameTime gameTime)
    {
         //Sparar va spelaren är 
        var playerPosition = Position;
        //Läser av vilken tangentsom trycks ned
        var keyboardState = Keyboard.GetState();
       
        //Hur man styr spelaren
        if (keyboardState.IsKeyDown(Keys.Left))
        playerPosition.X -= Speed;

        if (keyboardState.IsKeyDown(Keys.Right))
        playerPosition.X += Speed;

        if (keyboardState.IsKeyDown(Keys.Down))
        playerPosition.Y += Speed;

        ShootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (keyboardState.IsKeyDown(Keys.Up))
        
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
            float projectileSpeed = Speed + 2;
            //Lägger till projektilen i listan projectiles
            projectiles.Add(new Projectile(projectileTexture, projectileStartPosition, direction, projectileSpeed, 10));
            //Spelar skjutljudet
            ShootSound.Play();
        }
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
