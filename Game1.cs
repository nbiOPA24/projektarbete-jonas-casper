using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;

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

        Random rnd = new Random();
        float smallStart = rnd.Next(20, 780);
        float mediumStart = rnd.Next(20, 780);
        float bigStart = rnd.Next(20,780);
    
        player = new Player(this, new Vector2(350, 400), playerTexture, 100, 10, 20, 5);
        smallEnemy = new SmallEnemy(new Vector2(smallStart, 20), eyelanderTexture,  _graphics.PreferredBackBufferWidth); // TODO Sätt "20" till -100 för spawna utanför skärm"
        mediumEnemy = new MediumEnemy(new Vector2(mediumStart,20), antmakerTexture); // TODOSätt "20" till -100 för spawna utanför skärm"
        bigEnemy = new BigEnemy(new Vector2(bigStart,20), enemyUFOTexture, _graphics.PreferredBackBufferWidth); // TODO Sätt "20" till -100 för spawna utanför skärm"
    }
    protected override void Update(GameTime gameTime)
    {
        player.PlayerMovement(projectiles, laserGreenTexture, gameTime);
        
        var keyboardState = Keyboard.GetState();
        
        foreach (var projectile in projectiles)
        {
          projectile.Update(gameTime);
        }
        projectiles.RemoveAll(p => !p.IsActive);
        
        smallEnemy.MoveDownSmoothly(gameTime); //läser in metoden MoveDownSmoothly med (gametime) som inparameter
        bigEnemy.MoveSideToSide(gameTime); //läser in metoden MoveSidetoSide för BigEnemy
        UtilityMethods utility = new UtilityMethods();
        player.Position = utility.InsideBorder(player.Position, playerTexture, _graphics);
        
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
