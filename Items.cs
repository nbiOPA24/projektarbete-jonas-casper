using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Dynamic;

public class Item
{
    public struct Heart
    {
        public Vector2 Position {get;set;}
        public Texture2D Texture {get; set;}
        public int Size{get; set;}
        private Random ItemRandom = new Random();

        public Heart(Vector2 position, Texture2D texture, int size)
        {
            Position = new Vector2(ItemRandom.Next(20, 1880), ItemRandom.Next(20, 500));
            Texture = texture;
            Size = size;
        }
        public void DrawHeart(SpriteBatch SpriteBatch, Color color)
        {
            Rectangle heartRectangle = new Rectangle((int)Position.X, (int)Position.Y , Size, Size);
            SpriteBatch.Draw(Texture, heartRectangle, Color.Red);
        }
    }
}