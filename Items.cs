using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Dynamic;
using System.Data;



public abstract class Item
{
    private Random ItemRandom = new Random();
    public int ItemSize = 20;
    public Vector2 Position {get;set;}
    public Texture2D Texture {get; set;}
    public bool IsActive {get; set;} = true;

    public Rectangle HeartHitbox
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, ItemSize, ItemSize);
        }
    }

    public Item(Vector2 position, Texture2D texture)
    {
        Position = position;
        Texture = texture;
    }
    public class HeartItem : Item
    {
        public int HealthBoost {get; set;}

        public HeartItem(Vector2 position, Texture2D texture, int healthboost)
            : base(position, texture)
        {
            HealthBoost = healthboost;        
        }
        
        public void DrawHeart(SpriteBatch SpriteBatch)
        {
            Rectangle heartRectangle = new Rectangle((int)Position.X, (int)Position.Y , ItemSize, ItemSize);
            SpriteBatch.Draw(Texture, heartRectangle, Color.White);
        }
    }
}
        
                
    
    
        