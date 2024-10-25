//konstruktor för klassen enemy
using System;
using System.Buffers.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy
{
    public Vector2 Position {get; set;}
    public Texture2D Texture { get; set; }
    public string Name {get; set;}
    public int Health {get; set;}
    public int Attack {get; set;} 
    public int Shield {get; set;}
    public float Speed {get; set;}
    public bool IsActive {get; set;} = true;

    public Enemy(Vector2 startPosition,Texture2D texture, string name, int health, int attack, int shield, float speed)
    {
        Texture = texture;
        Position = startPosition;
        Name = name; 
        Health = health;
        Attack = attack;
        Shield = shield;
        Speed = speed;
    }
}
class SmallEnemy : Enemy
{
    public SmallEnemy(Vector2 startPosition,Texture2D texture, int screenWidth)
        : base(startPosition, texture, "SmallEnemy", 70, 10, 0, 10)
        
    {
        this.screenWidth = screenWidth; //Tar in våran screenWidth
    }
    private Random random = new Random();
    private int screenWidth; // 
    private float elapsedTime; //E
    public void MoveDownSmoothly(GameTime gameTime)
    {
        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Skapar en mjuk rörelse i x-riktning
        float xMovement = (float)Math.Sin(elapsedTime * 2) * 1.5f; // "2" styr hastigheten och "1.5" amplituden
        float yMovement = Speed * 0.1f;

        Position = new Vector2
        (
            MathHelper.Clamp(Position.X + xMovement, 0, screenWidth - Texture.Width),
            Position.Y + yMovement
        );
    }
    public void DrawSmallEnemy(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}

class MediumEnemy : Enemy
{
    public MediumEnemy(Vector2 startPosition,Texture2D texture)
         : base (startPosition, texture,"MediumEnemy", 100, 15, 5, 20)
    {
        //logik för medium enemy
    }
    public void DrawMediumEnemy(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}

class BigEnemy : Enemy
{

 public BigEnemy(Vector2 startPosition,Texture2D texture)
      : base (startPosition, texture,"BigEnemy", 150, 20, 15, 5)
    {
        
    }
    public void DrawBigEnemy(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}



