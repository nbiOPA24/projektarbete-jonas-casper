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
    private Vector2 position;
    private float speed;

    public BackgroundLayer(Texture2D texture, float speed)
    {
        this.texture = texture;
        this.position = Vector2.Zero;
        this.speed = speed;
    }

    public void Update()
    {
        position.Y += speed;
        if (position.Y >= texture.Height)
        {
            position.Y = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
        spriteBatch.Draw(texture, position + new Vector2(texture.Height, 0), Color.White);
    }
}


