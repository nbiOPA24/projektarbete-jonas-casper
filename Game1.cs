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
    private SpriteBatch _spriteBatch;
    private Texture2D playerTexture;
    private Texture2D eyelanderTexture;
    private Texture2D antmakerTexture;
    private Texture2D enemyUFOTexture;
    private Texture2D laserGreenTexture;
    private List<Projectile> projectiles;
    private List<Enemy> enemies; 
    private Texture2D hitboxTexture; //TODO TAG BORT SENARE, MÅLAR UT HITBOX ********
    public void DrawRectangle(Rectangle rectangle, Color color) //TODO TAG BORT SENARE, MÅLAR UT HITBOX ********
{
    _spriteBatch.Draw(hitboxTexture, rectangle, color); //TODO TAG BORT SENARE, MÅLAR UT HITBOX ********
}
    
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

        
        hitboxTexture = new Texture2D(GraphicsDevice, 1, 1);//TODO TAG BORT SENARE, MÅLAR UT HITBOX *****
        hitboxTexture.SetData(new[] { Color.White });//TODO TAG BORT SENARE, MÅLAR UT HITBOX *****
        
        //Här laddas alla .pngfiler in för player, projectile samlt alla enemies  
        playerTexture = Content.Load<Texture2D>("player");
        laserGreenTexture = Content.Load<Texture2D>("laserGreen");
        eyelanderTexture = Content.Load<Texture2D>("eyelander");
        antmakerTexture = Content.Load<Texture2D>("antmaker");
        enemyUFOTexture = Content.Load<Texture2D>("enemyUFO");
        projectiles = new List<Projectile>();
        enemies = new List<Enemy>();

        //Ger ett randomnummer som representerar en plats inanför spelfönstrets skärm som bestämmer var i Y(X?)ledd enemies ska spawna
        Random rnd = new Random();
        float smallStart = rnd.Next(20, 780);
        float mediumStart = rnd.Next(20, 780);
        float bigStart = rnd.Next(20,780);
    
        //Skapar  player samt alla enemies och änven vart dom ska spawna. Även alla agenskaper, om speed, health, shield 
        player = new Player(this, new Vector2(350, 400), playerTexture, 100, 10, 20, 5);
        smallEnemy = new SmallEnemy(new Vector2(smallStart, 20), eyelanderTexture,  _graphics.PreferredBackBufferWidth); // TODO Sätt "20" till -100 för spawna utanför skärm"
        mediumEnemy = new MediumEnemy(new Vector2(mediumStart,20), antmakerTexture); // TODOSätt "20" till -100 för spawna utanför skärm"
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
            if(utility.CheckCollision(enemy, player))
            {
                enemy.IsActive = false;
            }
        }
        enemies.RemoveAll(e => !e.IsActive);
        player.PlayerMovement(projectiles, laserGreenTexture, gameTime);

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
       
        player.DrawPlayer(_spriteBatch);
        DrawRectangle(player.Hitbox.Bounds, Color.Red);
                       
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
