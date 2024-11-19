using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
public class EnemySpawnManager 
{
     // TODO DENNA METODEN SKA TAS BORT, DEN MÅLAR UT HITBOXRNA FÖR ATT SE ATT DOM FUNGERAR!!!!!!!!!!!!!!!!!!!!!!!
    public void DrawHitboxes(SpriteBatch spriteBatch, Texture2D hitboxTexture)
{
    foreach (var enemy in Enemies)
    {
        if (enemy.IsActive)
        {
            spriteBatch.Draw(hitboxTexture, enemy.hitbox, Color.Red);
        }
    }
}
    public List<GameObject> Enemies = new List<GameObject>();
    public float SpawnInterval;
    public float ElapsedSpawnTime;
    public Texture2D SmallEnemyTexture, MediumEnemyTexture, BigEnemyTexture;
    public int ScreenWidth;
    public SoundEffect ShootSound;
    Random rnd = new Random();
    
    
    
    public EnemySpawnManager (float spawnInterval, int screenWidth, Texture2D smallEnemyTexture, Texture2D mediumEnemyTexture, Texture2D bigEnemyTexture, SoundEffect shootSound)
    {
        SpawnInterval = spawnInterval;
        ScreenWidth = screenWidth;
        SmallEnemyTexture = smallEnemyTexture;
        MediumEnemyTexture = mediumEnemyTexture;
        BigEnemyTexture = bigEnemyTexture;
        ShootSound = shootSound;
        
    }
    public void Update (GameTime gameTime)
    {
        ElapsedSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (ElapsedSpawnTime >= SpawnInterval)
        {
            //Kollar hur lång tid senast en Enemy har spawnat, har spaw time större eller likamed SpawnInter val spawnar en Enemy
            SpawnEnemy();
            ElapsedSpawnTime = 0f;
        }
        foreach (GameObject enemy in Enemies) //Kollar vilken enemy det är som spawnar, om enemys liv är noll raderas den specefika enemyn 
        {
            if(enemy is SmallEnemy smallEnemy)
            smallEnemy.Update(gameTime);

            else if(enemy is MediumEnemy mediumEnemy)
            mediumEnemy.Update(gameTime);
            
            else if (enemy is BigEnemy bigEnemy)
            bigEnemy.Update(gameTime);
            
            //GameObject.UpdateHitbox(); 
            if (enemy.BaseHealth <= 0)
            enemy.IsActive = false;

        }
        Enemies.RemoveAll(e => !e.IsActive);
        
    }
    private void SpawnEnemy() // Spawn-metod med en Random som ger olika sannolikheter baserat på Enemyns storlek(Svagast störst chans att spawna)
    {
        
        float spawnX = rnd.Next(0, ScreenWidth - 100);
        GameObject newEnemy;
        
        // Generera ett slumpmässigt tal mellan 0 och 999
        int spawnChance = rnd.Next(0, 1000);

        if (spawnChance < 600) // 60% sannolikhet för SmallEnemy
            newEnemy = new SmallEnemy(32, SmallEnemyTexture, new Vector2(spawnX, 100), 40, 10, 30, ScreenWidth, 0);
        
        else if (spawnChance < 850) // 25% sannolikhet för MediumEnemy
            newEnemy = new MediumEnemy(32, MediumEnemyTexture, new Vector2(spawnX, 100), 100, 25, 20, ScreenWidth, 0, ShootSound);
        
        else // 15% sannolikhet för BigEnemy
            newEnemy = new BigEnemy(32, BigEnemyTexture, new Vector2(spawnX, 100), 150, 40, 15, ScreenWidth, 0);
        
        //newEnemy.UpdateHitbox();
        Enemies.Add(newEnemy);

    }
    public void DrawEnemys(SpriteBatch spriteBatch) // Metod för att rita ut enemys.
    {
        foreach (var Enemy in Enemies)
        spriteBatch.Draw(Enemy.Texture, Enemy.Position, Color.White);
    }     
}