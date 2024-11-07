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
    public BackGroundManager(Texture2D texture, float speed)
    {
        backgroundLayer = new BackgroundLayer(texture, speed);
    }

    public void Update()
    {
        backgroundLayer.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        backgroundLayer.Draw(spriteBatch);
    }
    
}
public class BackgroundLayer
{
    private Texture2D texture;
    private Vector2 position1;
    private Vector2 position2;
    private float speed;
    

    public BackgroundLayer(Texture2D texture, float speed)
    {
        this.texture = texture;
        this.position1 = new Vector2(0, 0);
        // +1 för att få en överlapp, tabort skarven mellan bilderna
        this.position2 = new Vector2(0, -texture.Height);
        this.speed = speed;
        
    }

    public void Update()
    {
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

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position1, Color.White);
        spriteBatch.Draw(texture, position2, Color.White);
    }
}


