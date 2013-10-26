using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HungerPrototype.GameActors
{
    public class Player : Actor
    {

        #region Declarations

        Vector2 velocity;

        #endregion 

        #region Constructor

        public Player(ContentManager Content, Vector2 Location, int Width, int Height)
            : base(Content, Location, Width, Height)
        {
        }

        #endregion

        #region Implement Abstract Directional Information

        float MaxVelocity
        {
            get
            {
                return 100.0f;
            }    

        }

        Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity.X = MathHelper.Clamp(value.X, -MaxVelocity, MaxVelocity);
            }
        }

        Vector2 Acceleration
        {
            get
            {
                return new Vector2(5, 0);
            }
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {

            //Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

        }
        #endregion

    }
}
