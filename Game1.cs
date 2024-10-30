using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace JcGame;

public class Game1 : Game
{
    Player player;
    SmallEnemy smallEnemy;
    MediumEnemy mediumEnemy;
    BigEnemy bigEnemy;
    private GraphicsDeviceManager _graphics;
    private int _nativeWidth = 1920;
    private int _nativeHeight = 1080;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private Texture2D eyelanderTexture;
    private Texture2D antmakerTexture;
    private Texture2D enemyUFOTexture;
    private Texture2D laserGreenTexture;
    private List<Projectile> projectiles;
    private List<Enemy> enemies; 
    private EnemySpawnManager enenemySpawnManager;
    

        
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        _graphics.PreferredBackBufferWidth = _nativeWidth;
        _graphics.PreferredBackBufferHeight = _nativeHeight;
        _graphics.ApplyChanges();
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
       base.Initialize();
    }
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

       
        //Här laddas alla .pngfiler in för player, projectile samlt alla enemies  
        playerTexture = Content.Load<Texture2D>("player");
        enenemySpawnManager = new EnemySpawnManager(5f, _graphics.PreferredBackBufferWidth, Content.Load<Texture2D>("eyelander"), Content.Load<Texture2D>("antmaker"), Content.Load<Texture2D>("enemyUfo"));
        projectiles = new List<Projectile>();
        laserGreenTexture = Content.Load<Texture2D>("laserGreen");
        eyelanderTexture = Content.Load<Texture2D>("eyelander");
        antmakerTexture = Content.Load<Texture2D>("antmaker");
        enemyUFOTexture = Content.Load<Texture2D>("enemyUFO");
        enemies = new List<Enemy>();
        
        //Ger ett randomnummer som representerar en plats inanför spelfönstrets skärm som bestämmer var i Y(X?)ledd enemies ska spawna
        Random rnd = new Random();
        float smallStart = rnd.Next(20, 780);
        float mediumStart = rnd.Next(20, 780);
        float bigStart = rnd.Next(20,780);
    
        //Skapar  player samt alla enemies och änven vart dom ska spawna. Även alla agenskaper, om speed, health, shield 
        player = new Player(this, new Vector2(350, 400), playerTexture, 100, 10, 20, 5);
        smallEnemy = new SmallEnemy(new Vector2(smallStart, 20), eyelanderTexture,  _graphics.PreferredBackBufferWidth); // TODO Sätt "20" till -100 för spawna utanför skärm"
        mediumEnemy = new MediumEnemy(new Vector2(mediumStart,20), antmakerTexture, _graphics.PreferredBackBufferWidth); // TODOSätt "20" till -100 för spawna utanför skärm"
        bigEnemy = new BigEnemy(new Vector2(bigStart,20), enemyUFOTexture, _graphics.PreferredBackBufferWidth); // TODO Sätt "20" till -100 för spawna utanför skärm"

        enemies.Add(smallEnemy);
        enemies.Add(smallEnemy);
        enemies.Add(mediumEnemy);
        enemies.Add(bigEnemy);
    }
    protected override void Update(GameTime gameTime)
    {
        UtilityMethods utility = new UtilityMethods();
        foreach (var enemy in enemies)
        {
            if(utility.CheckCollisionPlayer(enemy, player))
            {
                enemy.IsActive = false;
                
            }
            foreach(var projectile in projectiles)
            if (utility.CheckCollisionProjectile(enemy, projectile))
            {
                enemy.IsActive = false;
                projectile.IsActive = false;
                break;
            }
        }
        enemies.RemoveAll(e => !e.IsActive);
        player.PlayerMovement(projectiles, laserGreenTexture, gameTime);
        enenemySpawnManager.Update(gameTime);
        foreach (var enemy in enemies)
        {
            if (enemy is SmallEnemy smallEnemy)
            {
                smallEnemy.MoveDownSmoothly(gameTime); //läser in metoden MoveDownSmoothly med (gametime) som inparameter        
            }
            else if (enemy is BigEnemy bigEnemy)
            {
                bigEnemy.MoveSideToSide(gameTime); //läser in metoden MoveSidetoSide för BigEnemy
            }
            else if (enemy is MediumEnemy)
            {
                mediumEnemy.MoveDownSmoothlyFaster(gameTime);
            }
            enemy.UpdateHitbox();
        }

        foreach (var projectile in projectiles)
        {
            projectile.Update(gameTime);
        }
        projectiles.RemoveAll(p => !p.IsActive);
        
        
       
        player.Position = utility.InsideBorder(player.Position, playerTexture, _graphics);
        
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        enenemySpawnManager.DrawEnemys(_spriteBatch);
        player.DrawPlayer(_spriteBatch);
                               
        foreach (var projectile in projectiles)
            projectile.DrawPlayerAttack(_spriteBatch);
        
        if (smallEnemy.IsActive)
            smallEnemy.DrawSmallEnemy(_spriteBatch);
        
        if (mediumEnemy.IsActive)
            mediumEnemy.DrawMediumEnemy(_spriteBatch);
        
        if(bigEnemy.IsActive)
            bigEnemy.DrawBigEnemy(_spriteBatch);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
