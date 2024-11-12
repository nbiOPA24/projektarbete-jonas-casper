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
    {// Updatera position baserat på riktning och hastighet, om projektilen åker utanför sätts IsActive till false
        Position += Direction * Speed * (float)gametime.ElapsedGameTime.TotalSeconds;

        if (Position.X < 0 || Position.X > 1920 || Position.Y < 0 || Position.Y > 1080)
        {
            IsActive = false;
        }
    }
    public void DrawPlayerAttack(SpriteBatch spriteBatch) //Ritar ut projectilerna.
    // OBS:för både Player och Enemys projectiler, Behöver byta namn för mer klarhet just nu kan man tolka det som att den bara ritar ut Playerns Attck medn används även för Enemys.
    {
        if (IsActive) 
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
        Hitbox.Update(Position);
    }
   public class MediumEnemyProjectile : Projectile //M-Es projektil som ärver basegenskaper ifrån Projectile
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
