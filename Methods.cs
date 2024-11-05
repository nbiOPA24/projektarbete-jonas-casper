using System.Diagnostics;
using JcGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class UtilityMethods
{
    //Metod föra att hålla objekt inom "fönstret"
    public Vector2 InsideBorder(Vector2 position, Texture2D texture, GraphicsDeviceManager graphics)
    {
        position.X = MathHelper.Clamp(position.X, 0, graphics.PreferredBackBufferWidth - texture.Width);
        position.Y = MathHelper.Clamp(position.Y, 0, graphics.PreferredBackBufferHeight -texture.Height);
        return position;
    }
    public bool CheckCollisionPlayer(Enemy enemy, Player player)
    {
        return enemy.Hitbox.Bounds.Intersects(player.Hitbox.Bounds);
    }

    public bool CheckCollisionProjectile(Enemy enemy, Projectile projectile)
    {
        return enemy.Hitbox.Bounds.Intersects(projectile.Hitbox.Bounds);
    }

    /*public bool CheckCollisionHeart(Item heart, Player player)
    {
        
    }*/

}