using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// Logik för hitbox
public class Hitbox
{
    //Skapar formen för hitboxen
    public Rectangle Bounds{get; private set;}
    //Skapar formen på hitboxen utifrån position och texturens storlek
    public Hitbox(Vector2 position, Texture2D texture)
    {
        Bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);        
    }
    //konstruktor för skapandet av hitbox
    public Hitbox(Vector2 position, int width, int higth)
    {
        Bounds = new Rectangle((int)position.X, (int)position.Y, width, higth) ;
    }
    //updaterar hitboxens position
    public void Update(Vector2 position)
    {
        Bounds = new Rectangle((int)position.X, (int)position.Y, Bounds.Width, Bounds.Height);
    }
}