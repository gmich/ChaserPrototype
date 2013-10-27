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
        float timeSinceLastFood;
        Random rand;

        #endregion

        public LevelManager(ContentManager Content)
        {
            actors = new List<Actor>();
            player = new Player(Content, new Vector2(400, 200), 40, 40, 1.0f);
            burgerTexture = Content.Load<Texture2D>(@"Textures\burger");
            timeSinceLastFood = 0.0f;
            rand = new Random();
        }

        #region Helper Methods

        Vector2 RandomLocation()
        {
            return new Vector2(rand.Next(20, 910), -60);
        }

        #endregion
        public void Update(GameTime gameTime)
        {
            timeSinceLastFood += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFood > 1.5f)
            {
                timeSinceLastFood = 0.0f;
                actors.Add(new Food(burgerTexture, RandomLocation(), 64, 39, 0.9f));
            }

            player.Update(gameTime);

            for(int i=0;i<actors.Count;i++)
            {
                actors[i].Update(gameTime);

                if((player.CollidesWith(actors[i])) || (actors[i].Inactive))
                    actors.RemoveAt(i);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);

            foreach (Actor actor in actors)
                actor.Draw(spriteBatch);
        }
    }
}
