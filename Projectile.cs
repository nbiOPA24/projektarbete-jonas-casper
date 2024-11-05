using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Projectile
{
    public Vector2 Position { get; set; }
    public Vector2 Direction { get; set; }
    public Texture2D Texture { get; set; }
    public float Speed { get; set; }
    public int Damage { get; set; }
    public bool IsActive { get; set; }
    public Hitbox Hitbox {get; set;}

        public Projectile(Texture2D texture, Vector2 position, Vector2 direction, float speed, int damage, Hitbox hitbox)
    {
        Texture = texture;
        Position = position;
        Direction = direction;
        Speed = speed;
        Damage = damage;
        IsActive = true;
        Hitbox = new Hitbox(Position, Texture);
    }
    public virtual void Update(GameTime gametime)
    {
        Position += Direction * Speed * (float)gametime.ElapsedGameTime.TotalSeconds;

        if (Position.X < 0 || Position.X > 1920 || Position.Y < 0 || Position.Y > 1080)
        {
            IsActive = false;
        }
    }
    public void DrawPlayerAttack(SpriteBatch spriteBatch)
    {
        if (IsActive)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
        Hitbox.Update(Position);
    }
   public class MediumEnemyProjectile : Projectile
   {
        public MediumEnemyProjectile(Texture2D texture, Vector2 position, Vector2 direction, float speed, int damage, Hitbox hitbox)
            : base (texture, position, direction, speed, damage, null)
        {
            Direction = direction;
            
            Hitbox = new Hitbox(position, Texture);
            
        }
        public override void Update(GameTime gameTime)
        {
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds; // Uppdaterar position varje frame baserat på riktning och hastighet
            Hitbox.Update(Position); // Uppdaterar hitbox-positionen
        }

   }   
}
