using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class EnemySpawnManager
{
    public List<Enemy> enemies = new List<Enemy>();
    public float spawnInterval;
    public float elapsedSpawnTime;
    public Texture2D smallEnemyTexture, mediumEnemyTexture, bigEnemyTexture;
    public int screenWidth;
    
    public EnemySpawnManager(float spawnInterval, int screenWidth, Texture2D smallEnemyTexture, Texture2D mediumEnemyTexture, Texture2D bigEnemyTexture)
    {
        this.spawnInterval = spawnInterval;
        this.screenWidth = screenWidth;
        this.smallEnemyTexture = smallEnemyTexture;
        this.mediumEnemyTexture =mediumEnemyTexture;
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
        int spawnChance = rnd.Next(0, 100);

        
        if (spawnChance < 60) // 60% sannolikhet för SmallEnemy
        {
        newEnemy = new SmallEnemy(new Vector2(spawnX, -100), smallEnemyTexture, screenWidth);
        }
        else if (spawnChance < 85) // 25% sannolikhet för MediumEnemy
        {
        newEnemy = new MediumEnemy(new Vector2(spawnX, -100), mediumEnemyTexture, screenWidth);
        }
        else // 15% sannolikhet för BigEnemy
        {
        newEnemy = new BigEnemy(new Vector2(spawnX, -100), bigEnemyTexture, screenWidth);
        }

    enemies.Add(newEnemy);
    }
    public void DrawEnemys(SpriteBatch spriteBatch)
    {
        foreach (var enemy in enemies)
        spriteBatch.Draw(enemy.Texture, enemy.Position, Color.White);
    }

}