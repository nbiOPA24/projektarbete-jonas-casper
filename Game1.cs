using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using Microsoft.VisualBasic;
using System;
using Microsoft.Xna.Framework.Audio;

namespace JcGame;

public class Game1 : Game
{
    /*public enum GameState
    {
        MainMenu,
        Playing,
        GameOver,
        Exit
    }*/
    private SpriteFont font;
    Player player;
    private GraphicsDeviceManager _graphics;
    private int _nativeWidth = 1920;
    private int _nativeHeight = 1080;
    //private bool isGameOver = false;
    private Texture2D heartTexture;
    private Texture2D attackSpeedTexture;
    private Item.AttackSpeedItem attackSpeed;
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private Texture2D laserGreenTexture;
    private Texture2D laserRedTexture;
    private Texture2D backgroundTexture;
    //private Texture2D gameOverTexture;
    public SoundEffect shootSound;
    private List<Projectile> projectiles;
    private EnemySpawnManager enemySpawnManager;
    private BackGroundManager backGroundManager;
    private Texture2D hitboxTexture; // TODO TA BORT SENARE MÅLAR HITBOX
    private Item.HeartItem heart;
    private double spawnTimer = 0;
    private double randomHeartTimer;
    private Random heartRandom = new Random();
    UtilityMethods utility = new UtilityMethods();
    
            
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
    //I LoadContent så laddas allt vi lägger in, tex player skin, item skins, bakgrund osv
    protected override void LoadContent()
    {
        //bakgrund
        backgroundTexture = Content.Load<Texture2D>("SpaceBackground");
        backGroundManager = new BackGroundManager(backgroundTexture, 1f);

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("playerHealth");
        // Skapa en enkel röd textur för att visualisera hitboxar
        hitboxTexture = new Texture2D(GraphicsDevice, 1, 1); //TABPRT SENARE MÅLAR HITBOX
        hitboxTexture.SetData(new[] { Color.Red * 0.5f }); // Halvgenomskinlig röd färg TA BORT SENARE MÅLAR HITBOX

        //Hjärtat som ger liv       
        heartTexture = Content.Load<Texture2D>("heartTexture");
        heart = new Item.HeartItem(Vector2.Zero, heartTexture, 10);
        heart.IsActive = false;
        attackSpeedTexture = Content.Load<Texture2D>("attackSpeedTexture");
        attackSpeed = new Item.AttackSpeedItem(Vector2.Zero, attackSpeedTexture, 2);
        attackSpeed.IsActive = false;
        Random random = new Random();
        randomHeartTimer = random.Next(5000, 15000);        
               
        //gameOverTexture = Content.Load<Texture2D>("Gameover");
        //gameOverTexture = Content.Load<Texture2D>("Gameover");
        //Projektil med ljud
        projectiles = new List<Projectile>();
        laserGreenTexture = Content.Load<Texture2D>("laserGreen");
        laserRedTexture = Content.Load<Texture2D>("laserRed");
        shootSound = Content.Load<SoundEffect>("laserSound"); 

        //spelaren       
        playerTexture = Content.Load<Texture2D>("player");
        player = new Player(this, new Vector2(940, 1000), playerTexture, 100, 35, 20, 15, shootSound);//baseHealth, baseDamage, baseShield, speed 
        
        enemySpawnManager = new EnemySpawnManager(2f, _graphics.PreferredBackBufferWidth, Content.Load<Texture2D>("eyelander"), Content.Load<Texture2D>("antmaker"), Content.Load<Texture2D>("enemyUfo"), shootSound);
    }
        //I update har vi allt som uppdateras i spelet, allt här updaterars 60ggr per sekund
        protected override void Update(GameTime gameTime)
    {
        backGroundManager.Update();
        //logik för när hjärtat ska spawna
        spawnTimer += gameTime.ElapsedGameTime.TotalMilliseconds; 
        if (spawnTimer >= randomHeartTimer)
        //Skapar ett nytt hjärta på en random plats inom vissa kordinater
        {
            heart.Position = new Vector2(heartRandom.Next(20, 1880), heartRandom.Next(20, 550));
            heart.IsActive = true; 
            spawnTimer = 0;
            randomHeartTimer = heartRandom.Next(5000, 15000);
        }
        // Vid kollision mellan hjärtat och spelare så ökar spelarens health med heart.healthboost som är 10hp
        if (heart.IsActive && player.Hitbox.Bounds.Intersects(heart.HeartHitbox))
        {
            player.BaseHealth += heart.HealthBoost;
            heart.IsActive = false; 
            spawnTimer = 0;
            randomHeartTimer = heartRandom.Next(5000, 15000);
        }
        // Spelaren dör om health är lika med eller mindre än 0. Nu avslutas spelet, men tanken är att man ska hamna i en meny!
        if (player.BaseHealth <= 0)
        {
            if (player.BaseHealth <= 0)
            {
                Exit();
            }
        }
        
        //Om spelaren och en enemy kolliderar så förlorar spelaren health som är lika enemy.Attack. och enemien dör
        foreach (var enemy in enemySpawnManager.enemies)
        {
            if(utility.CheckCollisionPlayer(enemy, player))
                {
                    player.BaseHealth = player.BaseHealth - enemy.Attack;
                    enemy.IsActive = false;
                }
            //Hanterar spelarens projektiler, om kollision mellan projektil och enemy så förlorar enemy health baserat på player.BaseDamage                
            foreach(var projectile in projectiles)
            {
                if (utility.CheckCollisionProjectile(enemy, projectile))
                {
                    projectile.IsActive = false;
                    enemy.Health = enemy.Health - player.BaseDamage;
                    if (enemy.Health <= 0) 
                    {
                        enemy.IsActive = false;
                    }
                }
                //Om en enemy hamnar utanför skärmen vertikalt så 'dör' enemien samt dess projektil
                if (enemy.Position.Y > GraphicsDevice.Viewport.Height || enemy.Position.Y < 0)
                {
                    enemy.IsActive = false; // Fienden har lämnat skärmen vertikalt                   
                }
                if(projectile.Position.Y > GraphicsDevice.Viewport.Height  || projectile.Position.Y < 0)
                {
                    projectile.IsActive = false;
                }
            }     
        }
        //Listan på alla aktiva enemies, tar bort enemies som inte länge är aktiva, alltså som har 'dött'                
        enemySpawnManager.enemies.RemoveAll(e => !e.IsActive);
        player.PlayerMovement(projectiles, laserGreenTexture, gameTime);
        enemySpawnManager.Update(gameTime);
        
        //Logik för hur alla enemies ska röra sig
        foreach (var enemy in enemySpawnManager.enemies)
        {
            if (enemy is SmallEnemy smallEnemy)
                smallEnemy.MoveDownSmoothly(gameTime); //läser in metoden MoveDownSmoothly med (gametime) som inparameter        
            
            else if (enemy is BigEnemy bigEnemy)
                bigEnemy.MoveSideToSide(gameTime); //läser in metoden MoveSidetoSide för BigEnemy
            
            else if (enemy is MediumEnemy mediumEnemy)
            {
                mediumEnemy.MoveDownSmoothlyFaster(gameTime);
                mediumEnemy.Update(gameTime, player, player.Position, laserRedTexture);
            }
            //Uppdaterar enemies hitbox så den följer bilden korrekt    
            enemy.UpdateHitbox();
            
        }
        //Går igenom listan med projektiler och tar bort de som är inte är aktiva
        foreach (var projectile in projectiles)
            projectile.Update(gameTime);
        
        projectiles.RemoveAll(p => !p.IsActive);
        //Kontrollerar så att spelaren inte kan åka utenför fönstrets kanter.    
        player.Position = utility.InsideBorder(player.Position, playerTexture, _graphics);

        base.Update(gameTime);
    }
    //Draw målar in alla textures i spelet
    protected override void Draw(GameTime gameTime)
    
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
       
        backGroundManager.Draw(_spriteBatch);
        if (attackSpeed.IsActive)
        {
            attackSpeed.DrawAttackSpeedItem(_spriteBatch);
        }
        //Om heart är aktivt så målas ett hjärta ut någonstans på skärmern
        if (heart.IsActive)
        {
            heart.DrawHeartItem(_spriteBatch);
        }
        //Målar ut en healthtect längst upp till vänster i spelfönstret
        string healthText = $"Health: {player.BaseHealth}";
        _spriteBatch.DrawString(font, healthText, new Vector2(100,100), Color.White);
                
        {
            enemySpawnManager.DrawEnemys(_spriteBatch);
            foreach (var enemy in enemySpawnManager.enemies)
        {
            if (enemy is MediumEnemy mediumEnemy)
            {
                // Rita projektilerna som MediumEnemy har skjutit
                mediumEnemy.DrawMediumEnemyAttack(_spriteBatch);
            }
        }
            enemySpawnManager.DrawHitboxes(_spriteBatch, hitboxTexture); //TODO TA BORT SENARE MÅLAR HITBOX
            player.DrawPlayer(_spriteBatch);
            //går igenom listan med projektiler och målar ut dom                    
            foreach (var projectile in projectiles)
                projectile.DrawPlayerAttack(_spriteBatch);
        }
                       
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
