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

        List<Actor> actors;
        Actor player;
        Texture2D burgerTexture;
        Texture2D vodkaTexture;
        SpriteFont scoreFont;
        int score;
        float timeSinceLastFood;
        Random rand;

        #endregion

        public LevelManager(ContentManager Content)
        {
            actors = new List<Actor>();
            player = new Player(Content, new Vector2(400, 200), 40, 40, 0.9f);
            burgerTexture = Content.Load<Texture2D>(@"Textures\burger");
            vodkaTexture = Content.Load<Texture2D>(@"Textures\vodka");
            scoreFont = Content.Load<SpriteFont>(@"Fonts\scoreFont");
            timeSinceLastFood = 0.0f;
            rand = new Random();
            score = 0;
        }

        #region Helper Methods

        Vector2 RandomLocation()
        {
            return new Vector2(rand.Next(20, 910), -60);
        }

        void AddRandomItem()
        {
            if (rand.Next(3) == 0)
                actors.Add(new Food(burgerTexture, RandomLocation(), 24, 16, 0.8f));
            else
                actors.Add(new Food(vodkaTexture, RandomLocation(), 24, 80, 0.8f));
        }

        #endregion
        public void Update(GameTime gameTime)
        {
            timeSinceLastFood += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFood > 1.5f)
            {
                timeSinceLastFood = 0.0f;
                AddRandomItem();
            }

            player.Update(gameTime);

            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].Update(gameTime);

                if ((actors[i].CollidesWith(player)))
                {
                    actors.RemoveAt(i);
                    SoundManager.PlayDing();
                    score++;
                }
                else if (actors[i].Inactive)
                    actors.RemoveAt(i);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);

            foreach (Actor actor in actors)
                actor.Draw(spriteBatch);

            spriteBatch.DrawString(scoreFont,score.ToString(), new Vector2(10,10), Color.Black);
        }
    }
}
