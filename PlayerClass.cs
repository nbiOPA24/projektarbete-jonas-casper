using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using JcGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public class Player
{
    private Game1 game;
    public Vector2 Position {get; set;}
    private Texture2D Texture { get; set;}
    public int BaseHealth {get; set;} 
    public int BaseAttack {get; set;}
    public int BaseShield {get; set;}
    public float Speed {get; set;} 
    

    
    
    public Player(Game1 game, Vector2 startPosition,Texture2D texture, int baseHealth, int baseAttack, int baseShield, float speed)
    {
        this.game = game;
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
    public void PlayerMovement()
    {
        var playerPosition = Position;
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Left))
            playerPosition.X -= Speed;

        if (keyboardState.IsKeyDown(Keys.Right))
            playerPosition.X += Speed;

        if (keyboardState.IsKeyDown(Keys.Down))
            playerPosition.Y += Speed;

        if (keyboardState.IsKeyDown(Keys.Up))
            playerPosition.Y -= Speed;

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            game.Exit();

        if (keyboardState.IsKeyDown(Keys.Space))
        Position = playerPosition;         
    }
}



    
    
