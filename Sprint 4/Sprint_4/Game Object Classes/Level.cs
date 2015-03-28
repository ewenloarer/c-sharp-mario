﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Sprint4
{
    public class Level
    {
        Game1 game;
        public Mario mario;
        public Fireball fireball;
        public List<Fireball> levelFireballs = new List<Fireball>();
        public List<Fireball> deadFireballs = new List<Fireball>();
        public LevelBuilder builder;
        public List<Enemy> levelEnemies = new List<Enemy>();
        public List<Block> levelBlocks = new List<Block>();
        public List<Item> levelItems = new List<Item>();       
        public List<KeyValuePair<IAnimatedSprite, Vector2>> levelBackgrounds = new List<KeyValuePair<IAnimatedSprite, Vector2>>();
        public CollisionDetector collision;
        
        public Texture2D background;
        
        public Level(Game1 game, string fileName)
        {
            this.game = game;
            mario = new Mario(this.game, new Vector2(250, (float)440));

            
            builder = new LevelBuilder(game);
            builder.Build(fileName, levelEnemies, levelBlocks, levelItems, levelBackgrounds);
            collision = new CollisionDetector(game);
            game.soundManager.PlaySong(SoundManager.songs.athletic);

            background = game.Content.Load<Texture2D>("background2");

        }

        public void Update(GameTime gameTime)
        {
            foreach (Enemy enemy in levelEnemies)
            {
                if (game.gameCamera.InCameraView(enemy.GetBoundingBox()))
                {
                    enemy.Update(gameTime);
                }
            }
            foreach (Item item in levelItems)
            {
                if (game.gameCamera.InCameraView(item.GetBoundingBox()))
                {
                    item.Update(gameTime);
                }
            }
            foreach (Block blockUpdater in levelBlocks)
            {
                if (game.gameCamera.InCameraView(blockUpdater.GetBoundingBox()))
                {
                    blockUpdater.Update(gameTime);
                }
            }
            
            foreach (Fireball fireball in levelFireballs)
             {
                fireball.Update(gameTime);
                if (fireball.fireballLifespan == 0)
                {
                    deadFireballs.Add(fireball);
                    if (mario.fireballCount > 0)
                    {
                        mario.fireballCount--;
                    } 
                }                             
             }

            foreach (Fireball fireball in deadFireballs)
            {
                levelFireballs.Remove(fireball);
                               
            }
             
            collision.Detect(mario, levelFireballs, levelEnemies, levelBlocks, levelItems); 

            mario.Update(gameTime);
            if (mario.position.X < 0)
            {
                mario.position.X = 0;
            }
            game.gameCamera.LookAt(mario.position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, new Rectangle(0, 55, 4096, 432), Color.White);
            foreach (KeyValuePair<IAnimatedSprite, Vector2> backgroundObject in levelBackgrounds)
            {
                backgroundObject.Key.Draw(spriteBatch, backgroundObject.Value);
            }
            foreach (Enemy enemy in levelEnemies)
            {
                if (game.gameCamera.InCameraView(enemy.GetBoundingBox()))
                {
                    enemy.Draw(spriteBatch);
                }
            }
            foreach (Item item in levelItems)
            {
                if (game.gameCamera.InCameraView(item.GetBoundingBox()))
                {
                    item.Draw(spriteBatch);
                }
            }
            foreach (Block blockDrawer in levelBlocks)
            {
                if (game.gameCamera.InCameraView(blockDrawer.GetBoundingBox()))
                {
                    blockDrawer.Draw(spriteBatch, blockDrawer.position);
                }
            }          
            foreach (Fireball fireball in levelFireballs)
            {
                if (game.gameCamera.InCameraView(fireball.GetBoundingBox()))
                {                   
                    fireball.Draw(spriteBatch);                     
                }
            }
            mario.Draw(spriteBatch);
        }
    }
}
