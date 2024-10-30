using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class EnemySpawnManager
{
    private List<Enemy> enemies = new List<Enemy>();
    private float spawnInterval;
    private float elapsedSpawnTime;
    private Texture2D smallEnemyTexture, mediumEnemyTexture, bigEnemyTexture;
    private int screenWidth;
    
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
            //SpawnEnemy();
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
        }

    }

}