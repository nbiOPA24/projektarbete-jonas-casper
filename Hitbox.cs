using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Hitbox
{
    public Rectangle Bounds{get; private set;}
    
    public Hitbox(Vector2 position, Texture2D texture)
    {
        Bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width -15, texture.Height -10);        
    }
    public void Update(Vector2 position)
    {
        Bounds = new Rectangle((int)position.X, (int)position.Y, Bounds.Width, Bounds.Height);
    }
}