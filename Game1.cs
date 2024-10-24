using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JcGame;

public class Game1 : Game
{
    Player player;
    SmallEnemy smallEnemy;
    MediumEnemy mediumEnemy;
    BigEnemy bigEnemy;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private Texture2D eyelanderTexture;
    private Texture2D antmakerTexture;
    private Texture2D enemyUFOTexture;
    private Texture2D laserGreenTexture;
    private List<Projectile> projectiles;
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("player");
        laserGreenTexture = Content.Load<Texture2D>("laserGreen");
        eyelanderTexture = Content.Load<Texture2D>("eyelander");
        antmakerTexture = Content.Load<Texture2D>("antmaker");
        enemyUFOTexture = Content.Load<Texture2D>("enemyUFO");
        projectiles = new List<Projectile>();
 
        player = new Player(new Vector2(350, 400), playerTexture, 100, 10, 20, 10);
        smallEnemy = new SmallEnemy(new Vector2(380, 20), eyelanderTexture);
        mediumEnemy = new MediumEnemy(new Vector2(200,20), antmakerTexture);
        bigEnemy = new BigEnemy(new Vector2(500,20), enemyUFOTexture);
        
        

        

    }
    protected override void Update(GameTime gameTime)
    {
        var playerPosition = player.Position;
        var keyboardState = Keyboard.GetState();
        

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (keyboardState.IsKeyDown(Keys.Left))
        {
            playerPosition.X -= player.Speed;
        }

        if (keyboardState.IsKeyDown(Keys.Right))
        {
            playerPosition.X += player.Speed;
        }

        if (keyboardState.IsKeyDown(Keys.Down))
        {
            playerPosition.Y += player.Speed;
        }

        if (keyboardState.IsKeyDown(Keys.Up))
        {
            playerPosition.Y -= player.Speed;
        }
        player.Position = playerPosition;    
        
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            float xOffset = playerTexture.Width / 2;
            float yOffset = playerTexture.Height / 2;
            Vector2 projectileStartPosition = new Vector2(player.Position.X + xOffset, player.Position.Y + yOffset);
            
            Vector2 direction = new Vector2(0,-1);
            projectiles.Add(new Projectile(laserGreenTexture, projectileStartPosition, direction, 300f, 10));
            
        }
        foreach (var projectile in projectiles)
        {
            projectile.Update(gameTime);
        }
        projectiles.RemoveAll(p => !p.IsActive);

        UtilityMethods utility = new UtilityMethods();
        player.Position = utility.InsideBorder(playerPosition, playerTexture, _graphics);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
       
        player.DrawPlayer(_spriteBatch);
        
        foreach (var projectile in projectiles)
        {
            projectile.DrawPlayerAttack(_spriteBatch);
        }
       
        smallEnemy.DrawSmallEnemy(_spriteBatch);
        mediumEnemy.DrawMediumEnemy(_spriteBatch);
        bigEnemy.DrawBigEnemy(_spriteBatch);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
