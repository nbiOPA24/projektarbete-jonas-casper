 using System.Diagnostics;
 using JcGame;
 using Microsoft.Xna.Framework;
 using Microsoft.Xna.Framework.Graphics;
 public class UtilityMethods
 {
     //Metod föra att hålla objekt inom "fönstret"
     public Vector2 InsideBorder(Vector2 position, Texture2D texture, GraphicsDeviceManager graphics)
     {
         position.X = MathHelper.Clamp(position.X, 0, graphics.PreferredBackBufferWidth - texture.Width);
         position.Y = MathHelper.Clamp(position.Y, 0, graphics.PreferredBackBufferHeight - texture.Height);
         return position;
     }
    //  public bool CheckCollisionPlayer(Enemys enemy, Player player) 
    //  //kollar om en fiendes och spelarenssamt projektilens hitboxar överlappar varandra. Om de gör det returneras true, vilket imnebär en kollision.
    //  {
    //      return enemy.Hitbox.Bounds.Intersects(player.Hitbox.Bounds);
    //  }

    //  public bool CheckCollisionProjectile(Enemys enemy, Projectile projectile)
    //  {
    //      return enemy.Hitbox.Bounds.Intersects(projectile.Hitbox.Bounds);
    //  }

    public void PlayerUpdate(Player player, GameTime gameTime, Game game)
    {
        if (BaseHealth <= 0)
        {
            if (BaseHealth <= 0)
            {
                game.Exit();
            }
        }
        //Logik för hur spelaren rör på sig, "pil upp" för uppåt tex
        float movementSpeed = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        //Sparar var spelaren är 
        var playerPosition = Position;
        //Läser av vilken tangentsom trycks ned
        var keyboardState = Keyboard.GetState();
    
        //Hur man styr spelaren
        if (keyboardState.IsKeyDown(Keys.Left))
        playerPosition.X -= movementSpeed;

        if (keyboardState.IsKeyDown(Keys.Right))
        playerPosition.X += movementSpeed;

        if (keyboardState.IsKeyDown(Keys.Down))
        playerPosition.Y += movementSpeed;

        if (keyboardState.IsKeyDown(Keys.Up))
        playerPosition.Y -= movementSpeed;
        
        //Escape stänger ned spelet
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            game.Exit();

        //Logik för hur ofta projektiler kan spawna
        ShootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        //Kod för hur man skjuter samt skapandet av projektiler och hur de beter sig
        if (keyboardState.IsKeyDown(Keys.Space) && ShootTimer >= ShootCooldown)
        {
            ShootTimer = 0F;
            //Ser till så att projektilen skjuts ifrån mitten av spelaren
            float xOffset = Texture.Width / 2;
            float yOffset = Texture.Height / 2;
            Vector2 projectileStartPosition = new Vector2(Position.X + xOffset, Position.Y + yOffset);
            
            //Projektilens riktning samt hastighet
            Vector2 direction = new Vector2(0, -20);
            float projectileSpeed = 50f;
            int textureSize = 10;
            int baseHealth = 0;
            
            //Lägger till projektilen i listan projectiles
            projectiles.Add(new Projectile(textureSize, projectileStartPosition, projectileTexture, baseHealth, direction, projectileSpeed, BaseDamage));
            //Spelar skjutljudet
            LaserSound.Play();
        }
        foreach (var projectile in projectiles)
            projectile.Update(gameTime);

        projectiles.RemoveAll(p => !p.IsActive);

        Position = playerPosition;

        foreach (var obj in game.nonPlayerObjects)
        {
            if (IsActive && obj.IsActive && hitbox.Intersects(obj.hitbox))
            {
                if (obj is HeartItem heartItem)
                {
                    BaseHealth = BaseHealth;
                }
                else if(obj is SmallEnemy smallEnemy)
                {
                    BaseHealth = BaseHealth - smallEnemy.BaseDamage;
                    obj.IsActive = false;
                }
                else if(obj is MediumEnemy mediumEnemy)
                {
                    BaseHealth = BaseHealth - mediumEnemy.BaseDamage;
                    obj.IsActive = false;
                }
                else if (obj is BigEnemy bigEnemy)
                {
                    BaseHealth = BaseHealth - bigEnemy.BaseDamage;
                    obj.IsActive = false;
                }
            }
        }

    }
    
}

//using System.ComponentModel.Design;

