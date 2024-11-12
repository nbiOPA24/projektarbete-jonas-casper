using Microsoft.Xna.Framework.Graphics;
using JcGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
namespace JcGame;
public class BackGroundManager
{
    private BackgroundLayer backgroundLayer;
    //KOnstruktor som tar en textur och en hastighet för att skapa en bakgrund som rör sig
    public BackGroundManager(Texture2D texture, float speed)
    {
        backgroundLayer = new BackgroundLayer(texture, speed);
    }

    //Uppdaterar background 60ggr per sekund
    public void Update()
    {
        backgroundLayer.Update();
    }

    //Målar ut bakgrunden
    public void Draw(SpriteBatch spriteBatch)
    {
        backgroundLayer.Draw(spriteBatch);
    }
}
public class BackgroundLayer
{
    //egenskaper för Bakgrund
    private Texture2D texture;
    private Vector2 position1;
    private Vector2 position2;
    private float speed;
    
    //Konstruktor för bakgrund 
    public BackgroundLayer(Texture2D texture, float speed)
    {
        
        this.texture = texture;
        position1 = new Vector2(0, 0);//Första bakgrundsbilden börjar vid pixel 0.0
        position2 = new Vector2(0, -texture.Height);//Andra bakgrundsbilden börjar direkt efter den första
        this.speed = speed;
        
    }

    public void Update()
    {
        //Hanterar hur fort bakgrunden ska röra sig
        position1.Y += speed;
        position2.Y += speed;

        // Uppdatera bara position1 om den lämnar skärmen
        if (position1.Y >= texture.Height)
        {
            position1.Y = position2.Y - texture.Height; // Flytta position1 ovanför position2
        }
        // Uppdatera bara position2 om den lämnar skärmen
        if (position2.Y >= texture.Height)
        {
            position2.Y = position1.Y - texture.Height; // Flytta position2 ovanför position1
        }
    }
    //Målar ut båda bilderna av bakgrunden i följd
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position1, Color.White);
        spriteBatch.Draw(texture, position2, Color.White);
    }
}


