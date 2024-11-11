using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Dynamic;
using System.Data;



public abstract class Item
{
    public int ItemSize = 50;
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

        public HeartItem(Vector2 position, Texture2D texture, int healthboost) : base(position, texture)
           
        {
            HealthBoost = healthboost;        
        }
        
        public void DrawHeartItem(SpriteBatch SpriteBatch)
        {
            Rectangle heartRectangle = new Rectangle((int)Position.X, (int)Position.Y , ItemSize, ItemSize);
            SpriteBatch.Draw(Texture, heartRectangle, Color.White);
        }
    }

    public class AttackSpeedItem : Item
    {
        public int AttackSpeedBoost {get; set;}
    
    public AttackSpeedItem(Vector2 position, Texture2D texture, int attackSpeedBoost) : base(position, texture)
    {
        AttackSpeedBoost = attackSpeedBoost;
    }
    public void DrawAttackSpeedItem(SpriteBatch SpriteBatch)
        {
            Rectangle AttackSpeedRectangle = new Rectangle((int)Position.X, (int)Position.Y , ItemSize, ItemSize);
            SpriteBatch.Draw(Texture, AttackSpeedRectangle, Color.White);
        }
    }
}
        
                
    
    
        