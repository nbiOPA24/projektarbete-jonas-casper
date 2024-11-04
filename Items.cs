using System.Dynamic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;


public struct Heart
   {
        public Vector2 Position {get; set;} 
        public Texture2D Texture {get; set;}
        public int Size{get; set;}

        public Heart(Vector2 positioin, Texture2D texture, int size)
        {
            Position = Position;
            Texture = texture;
            Size = size;
        }
    }
    