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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Player
{
    private Game1 game;
    public Vector2 Position {get; set;}
    public SoundEffect shootSound;
    private Texture2D Texture { get; set;}
    public int BaseHealth {get; set;} 
    public int BaseDamage {get; set;}
    public int BaseShield {get; set;}
    public float Speed {get; set;} 
    public Hitbox Hitbox{get; set;}
    //Cooldown för projektilerna
    private float shootCooldown = 0.3f;
    private float shootTimer = 0;
    public bool IsActive {get; set;} =  true; 
    //Konstruktor för PLayer
    public Player(Game1 game, Vector2 startPosition,Texture2D texture, int baseHealth, int baseDamage, int baseShield, float speed, SoundEffect shootSound)
    {
        //Spelaren egenskaper
        this.game = game;
        this.shootSound = shootSound;
        Texture = texture;
        Position = startPosition;   
        BaseHealth = baseHealth;
        BaseDamage = baseDamage;
        BaseShield = baseShield;
        Speed = speed;
        //Hitbox
        Hitbox = new Hitbox(Position, Texture);
    }
    //målar ut spelaren vid den aktuella positionen
    public void DrawPlayer(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
    //hanterar hur man förflyttar spelaren. 
    public void PlayerMovement(List<Projectile> projectiles, Texture2D laserGreenTexture, GameTime gameTime)
    {
        //Sparar va spelaren är 
        var playerPosition = Position;
        //Läser av vilken tangentsom trycks ned
        var keyboardState = Keyboard.GetState();
        shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        //Hur man styr spelaren
        if (keyboardState.IsKeyDown(Keys.Left))
        playerPosition.X -= Speed;

        if (keyboardState.IsKeyDown(Keys.Right))
        playerPosition.X += Speed;

        if (keyboardState.IsKeyDown(Keys.Down))
        playerPosition.Y += Speed;

        if (keyboardState.IsKeyDown(Keys.Up))
        playerPosition.Y -= Speed;
        //Kod för hur man skjuter samt skapandet av projektiler och hur de beter sig
        if (keyboardState.IsKeyDown(Keys.Space) && shootTimer >= shootCooldown)
        {
            shootTimer = 0F;
            //Ser till så att projektilen skjuts ifrån mitten av spelaren
            float xOffset = Texture.Width / 2;
            float yOffset = Texture.Height / 2;
            Vector2 projectileStartPosition = new Vector2(Position.X + xOffset, Position.Y + yOffset);
            
            //Projektilens riktning samt hastighet
            Vector2 direction = new Vector2(0, -20);
            float projectileSpeed = Speed + 2;
            //Lägger till projektilen i listan projectiles
            projectiles.Add(new Projectile(laserGreenTexture, projectileStartPosition, direction, projectileSpeed, 10, Hitbox));
            //Spelar skjutljudet
            shootSound.Play();
        }
        
        // Uppdaterar positionen direkt
        Position = playerPosition;
        //Uppdaterar Hitboxen utefter spelarens position
        Hitbox.Update(Position);
        
        //Gör att man kan avsluta spelet med "ESC" eller "Back" knappen på en kontroller 
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            game.Exit();
    }
}



    
    
