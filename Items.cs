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
        public Random ItemRandom;
        

        public Heart(Vector2 position, Texture2D texture, int size, Random itemRandom)
        {
            Position = new Vector2(itemRandom.Next(20, 1880), itemRandom.Next(20, 1040));
            Texture = texture;
            Size = size;
            ItemRandom = itemRandom;
        }
        public void SpawnHeart(SpriteBatch SpriteBatch)
        {
            int ItemRandomX = ItemRandom.Next(20, 1880);
            int ItemRandomY = ItemRandom.Next(20, 1040);
            Position = new Vector2(ItemRandomX, ItemRandomY);
            Rectangle heartRectangle = new Rectangle(ItemRandomX, ItemRandomY , Size, Size);
            SpriteBatch.Draw(Texture, heartRectangle, Color.White);
        }
    }
}
