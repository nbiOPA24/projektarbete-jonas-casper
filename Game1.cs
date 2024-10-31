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
    private GraphicsDeviceManager _graphics;
    private int _nativeWidth = 1920;
    private int _nativeHeight = 1080;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private Texture2D laserGreenTexture;
    private List<Projectile> projectiles;
    private List<Enemy> enemies; 
    private EnemySpawnManager enemySpawnManager;
    private Texture2D hitboxTexture; // TODO TA BORT SENARE MÅLAR HITBOX
        
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

        // Skapa en enkel röd textur för att visualisera hitboxar
        hitboxTexture = new Texture2D(GraphicsDevice, 1, 1); //TABPRT SENARE MÅLAR HITBOX
        hitboxTexture.SetData(new[] { Color.Red * 0.5f }); // Halvgenomskinlig röd färg TA BORT SENARE MÅLAR HITBOX
       
        //Här laddas alla .pngfiler in för player, projectile samlt alla enemies  
        playerTexture = Content.Load<Texture2D>("player");
        
        projectiles = new List<Projectile>();
        laserGreenTexture = Content.Load<Texture2D>("laserGreen");
        enemies = new List<Enemy>();
                    
        //Skapar  player samt alla enemies och änven vart dom ska spawna. Även alla agenskaper, om speed, health, shield 
        player = new Player(this, new Vector2(350, 400), playerTexture, 100, 10, 20, 5);
        
        enemySpawnManager = new EnemySpawnManager(2f, _graphics.PreferredBackBufferWidth, Content.Load<Texture2D>("eyelander"), Content.Load<Texture2D>("antmaker"), Content.Load<Texture2D>("enemyUfo"));
    }
    protected override void Update(GameTime gameTime)
    {
        UtilityMethods utility = new UtilityMethods();
        foreach (var enemy in enemySpawnManager.enemies)
        {
            if(utility.CheckCollisionPlayer(enemy, player))
                enemy.IsActive = false;
            
            foreach(var projectile in projectiles)
            {
                if (utility.CheckCollisionProjectile(enemy, projectile))
                {
                    enemy.IsActive = false;
                    projectile.IsActive = false;
                }
            }     
        }
           
        enemies.RemoveAll(e => !e.IsActive);
        player.PlayerMovement(projectiles, laserGreenTexture, gameTime);
        enemySpawnManager.Update(gameTime);
        foreach (var enemy in enemySpawnManager.enemies)
        {
            if (enemy is SmallEnemy smallEnemy)
                smallEnemy.MoveDownSmoothly(gameTime); //läser in metoden MoveDownSmoothly med (gametime) som inparameter        
            
            else if (enemy is BigEnemy bigEnemy)
                bigEnemy.MoveSideToSide(gameTime); //läser in metoden MoveSidetoSide för BigEnemy
            
            else if (enemy is MediumEnemy mediumEnemy)
                mediumEnemy.MoveDownSmoothlyFaster(gameTime);
            
            enemy.UpdateHitbox();
        }

        foreach (var projectile in projectiles)
            projectile.Update(gameTime);
        
        projectiles.RemoveAll(p => !p.IsActive);
              
        player.Position = utility.InsideBorder(player.Position, playerTexture, _graphics);
        
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        enemySpawnManager.DrawEnemys(_spriteBatch);
        enemySpawnManager.DrawHitboxes(_spriteBatch, hitboxTexture); //TODO TA BORT SENARE MÅLAR HITBOX
        player.DrawPlayer(_spriteBatch);
                               
        foreach (var projectile in projectiles)
            projectile.DrawPlayerAttack(_spriteBatch);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
