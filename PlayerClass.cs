using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class Player
    {
        public Vector2 Position {get; set;}
        private Texture2D Texture { get; set; }
        public int BaseHealth { get; set;} 
        public int BaseAttack {get; set;}
        public int BaseShield {get; set;}
        public float Speed {get; set;} 
        
     public Player(Vector2 startPosition,Texture2D texture, int baseHealth, int baseAttack, int baseShield, float speed)
        {
            Texture = texture;
            Position = startPosition;   
            BaseHealth = baseHealth;
            BaseAttack = baseAttack;
            BaseShield = baseShield;
            Speed = speed;
        }

        public void DrawPlayer(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }


    
    
