using Microsoft.Xna.Framework.Graphics;
using JcGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System;
namespace JcGame;
public class ProjectileManager
{
    public List<Projectile> projectiles = new List<Projectile>();

    public void AddProjectile(Projectile projectile)
    {
        projectiles.Add(projectile); 
    }
    public void Update(GameTime gameTime)
    {
        foreach (var projectile in projectiles)
        {
            if (projectile.IsActive)
            {
                projectile.Update(gameTime);
            }
        }
        
        projectiles.RemoveAll(p => !p.IsActive);
        
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var projectile in projectiles)
        {
            if (projectile.IsActive)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }


}