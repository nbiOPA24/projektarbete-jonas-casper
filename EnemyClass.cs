//konstruktor för klassen enemy
using System;
using System.Buffers.Text;
using System.Data;
using System.Runtime.Serialization;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Enemy
{
    public Vector2 Position {get; set;}
    public Texture2D Texture { get; set; }
    public string Name {get; set;}
    public int Health {get; set;}
    public int Attack {get; set;} 
    public int Shield {get; set;}
    public float Speed {get; set;}
    public bool IsActive {get; set;} = true;
    public Hitbox Hitbox {get; set;}
    

    public Enemy(Vector2 startPosition,Texture2D texture, string name, int health, int attack, int shield, float speed)
    {
        Texture = texture;
        Position = startPosition;
        Name = name; 
        Health = health;
        Attack = attack;
        Shield = shield;
        Speed = speed;
        Hitbox = new Hitbox(Position, Texture);
    }

    public void UpdateHitbox()
    {
        Hitbox.Update(Position);
    }

}
class SmallEnemy : Enemy
{
    private int screenWidth; // 
    private float elapsedTime; //E
    public SmallEnemy(Vector2 startPosition,Texture2D texture, int screenWidth)
        : base(startPosition, texture, "SmallEnemy", 70, 10, 0, 10)
        
    {
        this.screenWidth = screenWidth; //Tar in våran screenWidth
    }
 
    public void MoveDownSmoothly(GameTime gameTime)
    {
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Skapar en mjuk rörelse i x-riktning
        float xMovement = (float)Math.Sin(elapsedTime * 2) * 1.5f; 
        float yMovement = Speed * 0.1f;

        Position = new Vector2
        (
            MathHelper.Clamp(Position.X + xMovement, 0, screenWidth - Texture.Width),
            Position.Y + yMovement
        );
        UpdateHitbox(); 
    }
    
     public void DrawSmallEnemy(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}

class MediumEnemy : Enemy
{
    private int screenWidth;
    private float elapsedTime;
    public MediumEnemy(Vector2 startPosition,Texture2D texture)
         : base (startPosition, texture,"MediumEnemy", 100, 15, 5, 20)
    {
        //tar in våran screenwidth
    }
    public void MoveDownSmoothlyFaster(GameTime gameTime)
    {
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Skapar en mjuk rörelse i x-riktning
        float xMovement = (float)Math.Sin(elapsedTime * 2) * 2.5f; 
        float yMovement = Speed * 0.1f;

        Position = new Vector2
        (
            MathHelper.Clamp(Position.X + xMovement, 0, screenWidth - Texture.Width),
            Position.Y + yMovement
        );
        UpdateHitbox(); 
    }

    public void DrawMediumEnemy(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}

class BigEnemy : Enemy
{
    private float elapsedTime;
    private int screenWidth;
    private bool movingRight = true;
    
    public BigEnemy(Vector2 startPosition,Texture2D texture, int screenWidth)
      : base (startPosition, texture,"BigEnemy", 150, 20, 15, 2)
         {
             this.screenWidth = screenWidth;
         }
    
    public void MoveSideToSide(GameTime gameTime)
    {
        
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
         
        if (movingRight)
        {
        // Flytta till höger
        Position = new Vector2(Position.X + Speed, Position.Y); // Skapa en ny Vector2
        // Kontrollera om den når högra kanten
        if (Position.X + Texture.Width >= screenWidth)
        {
            Position = new Vector2(screenWidth - Texture.Width, Position.Y); // går till högra kanten
            movingRight = false; // Byt riktning
        }
        }
        else 
        {
        // Flytta till vänster
        Position = new Vector2(Position.X - Speed, Position.Y); // Skapa en ny Vector2
        // Kontrollera om den når vänstra kanten
        if (Position.X <= 0)
        {
            Position = new Vector2(0, Position.Y); // går till vänstra kanten
            movingRight = true; // Byt riktning
        }
        }
        UpdateHitbox();
    }
    
    public void DrawBigEnemy(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}



