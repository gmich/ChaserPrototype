using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HungerPrototype.Managers
{
    using GameActors;

    public class LevelManager
    {

        #region Declarations

        List<Actor> food;
        List<Enemy> enemies;
        Player player;
        Texture2D burgerTexture;
        Texture2D vodkaTexture;
        Texture2D enemyTexture;
        SpriteFont scoreFont;

        int catScore;
        int enemyScore;
        int currEnemies;

        float timeSinceLastFood;
        float timeSinceLastEnemy;

        Random rand;
        ContentManager Content;

        #endregion

        public LevelManager(ContentManager Content)
        {
            this.Content = Content;
            burgerTexture = Content.Load<Texture2D>(@"Textures\burger");
            vodkaTexture = Content.Load<Texture2D>(@"Textures\vodka");
            enemyTexture = Content.Load<Texture2D>(@"Textures\Enemy\run");
            scoreFont = Content.Load<SpriteFont>(@"Fonts\scoreFont");

            rand = new Random();
            catScore = enemyScore = 0;
            NewGame();
        }

        #region New Game

        void NewGame()
        {
            food = new List<Actor>();
            enemies = new List<Enemy>();
            player = new Player(Content, new Vector2(400, 500), 40, 40, 0.9f);
            timeSinceLastFood = 0.0f;
            timeSinceLastEnemy = 10.0f;
            currEnemies = 0;
        }

        #endregion

        #region Properties

        int MaxEnemies
        {
            get
            {
                return 1;
            }
        }

        #endregion

        #region Helper Methods

        Vector2 RandomLocation()
        {
            return new Vector2(rand.Next(20, 910), -60);
        }

        void AddRandomItem()
        {
            if (rand.Next(3) == 0)
                food.Add(new Food(burgerTexture, RandomLocation(), 24, 16, 0.8f));
            else
                food.Add(new Food(vodkaTexture, RandomLocation(), 24, 80, 0.8f));

            foreach (Enemy enemy in enemies)
                enemy.SetTarget(food[food.Count - 1]);
        }

        void AddEnemy()
        {
            if (MaxEnemies > currEnemies)
            {
                currEnemies++;
                Vector2 spawnLocation;
                if (rand.Next(2) == 0)
                    spawnLocation = new Vector2(-200, 600);
                else
                    spawnLocation = new Vector2(1200, 600);

                enemies.Add(new Enemy(Content, enemyTexture,spawnLocation, 50, 66, 0.7f));
            }
        }

        void CheckVictory()
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.IsHungry())
                {
                    NewGame();
                    catScore++;
                }
            }

            if (player.IsHungry())
            {
                NewGame();
                enemyScore++;
            }
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            timeSinceLastFood += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeSinceLastEnemy += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFood > 1.5f)
            {
                timeSinceLastFood = 0.0f;
                AddRandomItem();
            }

            if (timeSinceLastEnemy > 8.5f)
            {
                timeSinceLastEnemy = 0.0f;
                AddEnemy();
            }

            player.Update(gameTime);

            for (int i = 0; i < enemies.Count; i++)
                enemies[i].Update(gameTime);

            for (int i = 0; i < food.Count; i++)
            {
                food[i].Update(gameTime);

                foreach (Enemy enemy in enemies)
                {
                    if ((food[i].CollidesWith(enemy)))
                    {
                        food.RemoveAt(i);
                        enemy.ManipulateHungerBar(2.0f);
                        SoundManager.PlayDing(-1.0f);
                        return;
                    }
                }

                if ((food[i].CollidesWith(player)))
                {
                    food.RemoveAt(i);
                    player.ManipulateHungerBar(3.0f);
                    SoundManager.PlayDing(1.0f);
                }
                else if (food[i].Inactive)
                    food.RemoveAt(i);
            }

            CheckVictory();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);

            foreach (Actor actor in food)
                actor.Draw(spriteBatch);

            foreach(Enemy enemy in enemies)
                enemy.Draw(spriteBatch);

            spriteBatch.DrawString(scoreFont,"Cat: " +catScore.ToString() + "    Opponent: " + enemyScore.ToString(), new Vector2(10,10), Color.Black);
        }
    }
}
