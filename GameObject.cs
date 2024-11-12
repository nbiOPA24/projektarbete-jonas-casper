using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

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

    public abstract void LoadContent();
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
    public SoundEffect ShootSound;
    public float ShootCooldown = 0.3f;
    public float ShootTimer = 0;

    public Player(int textureSize, Texture2D texture, Vector2 position,  int baseHealth, int baseDamage, int baseShield, float speed, SoundEffect shootSound, float shootCooldown, float shootTimer)
    : base(textureSize, texture, position)

    {
        BaseHealth = baseHealth;
        BaseDamage = baseDamage;
        BaseShield = baseShield;
        Speed = speed;
        ShootSound = shootSound;
    }
}