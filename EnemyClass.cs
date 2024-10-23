//konstruktor för klassen enemy
using System.Buffers.Text;

class Enemy
{
    public string Name {get; set;}
    public int Health {get; set;}
    public int Attack {get; set;} 
    public int Shield {get; set;}
    public float Speed {get; set;}

    public Enemy(string name, int health, int attack, int shield, float speed)
    {
        Name = name; 
        Health = health;
        Attack = attack;
        Shield = shield;
        Speed = speed;
    }

}

    class SmallEnemy : Enemy
{
    public SmallEnemy(): base ("SmallEnemy", 70, 10, 0, 35)
    {
        //Logik för smallenemy
    }
}

class MediumEnemy : Enemy
{
    
    public MediumEnemy(): base ("MediumEnemy", 100, 15, 5, 20)
    {
        //logik för medium enemy
    }


}

class BigEnemy : Enemy
{

 public BigEnemy(): base ("BigEnemy", 150, 20, 15, 5)
    {
        //logik för bigenemy
    }

}



