using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using Microsoft.VisualBasic;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace JcGame;

public class Game1 : Game
{
    
    private SpriteFont font;
    private Player player;
    private EnemySpawnManager enemySpawnManager;
    private HeartItem heart;
    private SmallEnemy smallEnemy;
    private MediumEnemy mediumEnemy;
    private BigEnemy bigEnemy;
    private GraphicsDeviceManager _graphics;
    public List<GameObject> nonPlayerObjects = new List<GameObject>();
    
    // // //private bool isGameOver = false;
       // // private Texture2D attackSpeedTexture;
    // // private Item.AttackSpeedItem attackSpeed;
    private SpriteBatch _spriteBatch;
    
    // // //private Texture2D gameOverTexture;
    
    List<Projectile> projectiles = new List<Projectile>();
    
    public BackGroundManager backGroundManager;
    private Texture2D hitboxTexture; // TODO TA BORT SENARE MÅLAR HITBOX
    // private Item.HeartItem heart;
    // private double spawnTimer = 0;
    // private double randomHeartTimer;
    // private Random heartRandom = new Random();
        
    //UtilityMethods utility = new UtilityMethods();
    //private Texture2D projectileTexture;
            
    public Game1()
    {
        //övergripande inställningar för spelet
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        //Här berättar vi för spelet vad det är som ska laddas in när spelet startas. 
        int textureSize = 64; 
        float playerSpeed = 400f;

        heart = new HeartItem(textureSize, new Vector2(0,0), null, 10, 0);
        player = new Player(textureSize, null, new Vector2(940, 1000), 100, 35, 20, playerSpeed, null, this);//baseHealth, baseDamage, baseShield, speed
        smallEnemy = new SmallEnemy(64, null, new Vector2(400, 100), 40, 10, 15, screenWidth:1920, 0);
        mediumEnemy = new MediumEnemy(64, null, new Vector2(400, 100), 100, 25, 20, screenWidth:1920, 0, laserSound: null);
        bigEnemy = new BigEnemy (64, null, new Vector2(400,200), 150, 50, 5, screenWidth:1920, 0);
        
        // //Lägger till alla nonplayer objects i en lista
        
        nonPlayerObjects.Add(smallEnemy);   
        nonPlayerObjects.Add(mediumEnemy);
        nonPlayerObjects.Add(bigEnemy);
        nonPlayerObjects.Add(heart);
        
        base.Initialize();
    }
    //I LoadContent så laddas allt vi lägger in, tex player skin, item skins, bakgrund osv
    protected override void LoadContent()
    {
        
        hitboxTexture = new Texture2D(GraphicsDevice, 1, 1); //TA BORT SENARE, MÅLAR HITBOX
        hitboxTexture.SetData(new[] { Color.Red * 0.5f }); //TA BORT SENARE, MÅLAR HITBOX
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("playerHealth");
        //Alla sprites, bilder, ljud osv för respektive klass laddas in från GameObject                
        player.LoadContent(Content);
        smallEnemy.LoadContent(Content);
        mediumEnemy.LoadContent(Content);
        bigEnemy.LoadContent(Content);  
        heart.LoadContent(Content);
        backGroundManager = new BackGroundManager(1f);
        backGroundManager.LoadContent(Content);
        

        //Skapar Ett nytt objekt av typen EnemySpawnManager
        enemySpawnManager = new EnemySpawnManager(
            spawnInterval: 2f,
            screenWidth: GraphicsDevice.Viewport.Width, 
            smallEnemyTexture: smallEnemy.Texture, 
            mediumEnemyTexture: mediumEnemy.Texture, 
            bigEnemyTexture: bigEnemy.Texture, 
            shootSound: mediumEnemy.LaserSound);
                    
        base.LoadContent();
    ///////////////////////////////////////////////////////////////////////////////
        //bakgrund
        //backgroundTexture = Content.Load<Texture2D>("SpaceBackground");
        
        // //Hjärtat som ger liv       
        // heartTexture = Content.Load<Texture2D>("heartTexture");
        // heart = new Item.HeartItem(Vector2.Zero, heartTexture, 10);
        // heart.IsActive = false;
        // attackSpeedTexture = Content.Load<Texture2D>("attackSpeedTexture");
        // attackSpeed = new Item.AttackSpeedItem(Vector2.Zero, attackSpeedTexture, 2);
        // attackSpeed.IsActive = false;
        // Random random = new Random();
        // randomHeartTimer = random.Next(5000, 15000);        
              
                       
        // laserGreenTexture = Content.Load<Texture2D>("laserGreen");
        // laserRedTexture = Content.Load<Texture2D>("laserRed");
            
        
        // enemySpawnManager = new EnemySpawnManager(2f, _graphics.PreferredBackBufferWidth, Content.Load<Texture2D>("eyelander"), Content.Load<Texture2D>("antmaker"), Content.Load<Texture2D>("enemyUfo"), shootSound);
    }
        //I update har vi allt som uppdateras i spelet, allt här updaterars 60ggr per sekund
        protected override void Update(GameTime gameTime)
    {
        //Spellogik för respektive klass
        player.Update(gameTime);
        smallEnemy.Update(gameTime);
        mediumEnemy.Update(gameTime);
        bigEnemy.Update(gameTime);
        enemySpawnManager.Update(gameTime);
        heart.Update(gameTime);
        backGroundManager.Update();
        
        base.Update(gameTime);          
    }
    //Draw målar in alla textures i spelet
    protected override void Draw(GameTime gameTime)
    
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        backGroundManager.Draw(_spriteBatch);

        // Här ritas alla testures ut i spelet. 
        player.Draw(_spriteBatch);
        
        enemySpawnManager.DrawEnemys(_spriteBatch);
       
        string healthText = $"Health: {player.BaseHealth}";
        _spriteBatch.DrawString(font, healthText, new Vector2(100,100), Color.White);
              
        enemySpawnManager.DrawHitboxes(_spriteBatch, hitboxTexture);
        _spriteBatch.End();

        base.Draw(gameTime);
                             
        
    }
}
