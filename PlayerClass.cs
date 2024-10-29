using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
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
    public Hitbox Hitbox{get; set;}
    private float shootCooldown = 0.3f;
    private float shootTimer = 0;

    public Player(Game1 game, Vector2 startPosition,Texture2D texture, int baseHealth, int baseAttack, int baseShield, float speed)
    {
        this.game = game;
        Texture = texture;
        Position = startPosition;   
        BaseHealth = baseHealth;
        BaseAttack = baseAttack;
        BaseShield = baseShield;
        Speed = speed;
        
        Hitbox = new Hitbox(Position, Texture);
    }

    public void DrawPlayer(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
    public void PlayerMovement(List<Projectile> projectiles, Texture2D laserGreenTexture, GameTime gameTime)
    {
        var playerPosition = Position;
        var keyboardState = Keyboard.GetState();
        shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        

        if (keyboardState.IsKeyDown(Keys.Left))
         playerPosition.X -= Speed;

        if (keyboardState.IsKeyDown(Keys.Right))
        playerPosition.X += Speed;

        if (keyboardState.IsKeyDown(Keys.Down))
        playerPosition.Y += Speed;

        if (keyboardState.IsKeyDown(Keys.Up))
        playerPosition.Y -= Speed;

        if (keyboardState.IsKeyDown(Keys.Space) && shootTimer >= shootCooldown)
        {
            shootTimer = 0F;
            float xOffset = Texture.Width / 2;
            float yOffset = Texture.Height / 2;
            Vector2 projectileStartPosition = new Vector2(Position.X + xOffset, Position.Y + yOffset);
        
            Vector2 direction = new Vector2(0, -20);
            float projectileSpeed = Speed + 2;
            projectiles.Add(new Projectile(laserGreenTexture, projectileStartPosition, direction, projectileSpeed, 10));
        }

         // Uppdaterar positionen direkt
        Position = playerPosition;

        Hitbox.Update(Position);

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            game.Exit();
    }
}



    
    
