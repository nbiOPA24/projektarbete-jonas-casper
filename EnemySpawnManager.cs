using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class EnemySpawnManager 
{
    public List<Enemy> enemies = new List<Enemy>();
    public float spawnInterval;
    public float elapsedSpawnTime;
    public Texture2D smallEnemyTexture, mediumEnemyTexture, bigEnemyTexture;
    public int screenWidth;
    
    
    public EnemySpawnManager (float spawnInterval, int screenWidth, Texture2D smallEnemyTexture, Texture2D mediumEnemyTexture, Texture2D bigEnemyTexture)
    {
        this.spawnInterval = spawnInterval;
        this.screenWidth = screenWidth;
        this.smallEnemyTexture = smallEnemyTexture;
        this.mediumEnemyTexture = mediumEnemyTexture;
        this.bigEnemyTexture = bigEnemyTexture;
    }
    public void Update (GameTime gameTime)
    {
        elapsedSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (elapsedSpawnTime >= spawnInterval)
        {
            
            SpawnEnemy();
            elapsedSpawnTime = 0f;
        }
        foreach (Enemy enemy in enemies)
        {
           
            if(enemy.Health <= 0)
            enemy.IsActive = false;

            if(enemy is SmallEnemy smallEnemy)
            smallEnemy.MoveDownSmoothly(gameTime);
            else if (enemy is MediumEnemy mediumEnemy)
            mediumEnemy.MoveDownSmoothlyFaster(gameTime);
            
            else if (enemy is BigEnemy bigEnemy)
            bigEnemy.MoveSideToSide(gameTime);
            
            enemy.UpdateHitbox();        
        }
        enemies.RemoveAll(e => !e.IsActive);
        
    }
    private void SpawnEnemy()
    {
        Random rnd = new Random();
        float spawnX = rnd.Next(0, screenWidth - 100);
        Enemy newEnemy;
        
        // Generera ett slumpmässigt tal mellan 0 och 99
        int spawnChance = rnd.Next(0, 1000);

        if (spawnChance < 600) // 60% sannolikhet för SmallEnemy
            newEnemy = new SmallEnemy(new Vector2(spawnX, 100), smallEnemyTexture, screenWidth);
        
        else if (spawnChance < 850) // 25% sannolikhet för MediumEnemy
            newEnemy = new MediumEnemy(new Vector2(spawnX, 100), mediumEnemyTexture, screenWidth);
        
        else // 15% sannolikhet för BigEnemy
            newEnemy = new BigEnemy(new Vector2(spawnX, 100), bigEnemyTexture, screenWidth);
        
        newEnemy.UpdateHitbox();
        enemies.Add(newEnemy);

    }
    public void DrawEnemys(SpriteBatch spriteBatch)
    {
        foreach (var Enemy in enemies)
        spriteBatch.Draw(Enemy.Texture, Enemy.Position, Color.White);
    }
     public void DrawHitboxes(SpriteBatch spriteBatch, Texture2D hitboxTexture) //TODO TA BORT SENARE MÅLAR HITBOX
{                                                                               //TODO TA BORT SENARE MÅLAR HITBOX
    foreach (var enemy in enemies)                                              //TODO TA BORT SENARE MÅLAR HITBOX
    {                                                                           //TODO TA BORT SENARE MÅLAR HITBOX
        // Ritar ut fiendens hitbox som en halvgenomskinlig rektangel           //TODO TA BORT SENARE MÅLAR HITBOX
        spriteBatch.Draw(hitboxTexture, enemy.Hitbox.Bounds, Color.Red * 0.5f); //TODO TA BORT SENARE MÅLAR HITBOX
    }
}
     
}