//konstruktor för klassen enemy
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

    protected Random random = new Random();

    

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
    public virtual void MovePattern(GameTime gameTime)
    {
        // Standard rörelse, kan överskridas av subklasser
       Position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Update(GameTime gameTime)
    {
        MovePattern(gameTime); 
        
        if (Position.Y > 480)
        {
            IsActive = false;

        }
    }


    
    
    
}
    class SmallEnemy : Enemy
{
    public SmallEnemy(Vector2 startPosition,Texture2D texture)
        : base(startPosition, texture, "SmallEnemy", 70, 10, 0, 35)
    {
        
    }

    public override void MovePattern(GameTime gametime)
    {
        base.MovePattern(gametime);
        Position.X += (float)(random.NextDouble() * 2 - 1);

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



