using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HungerPrototype.GameActors
{
    using Animations;
    using Input;

    public class Food : Actor
    {

        #region Declarations

        Vector2 velocity;
        Vector2 location;
        bool hitTheGround;

        #endregion 

        #region Constructor

        public Food(Texture2D texture, Vector2 Location, int Width, int Height,float drawDepth)
            : base(Location, Width, Height,drawDepth)
        {
            hitTheGround = false;
            animations.Add("idle", new AnimationStrip(texture, 64, "idle"));
            animations["idle"].LoopAnimation = true;

            PlayAnimation("idle");
        }

        #endregion

        #region Override Directional Information

        Vector2 WindowBoundaries
        {
            get
            {
                return new Vector2(1000, 600);
            }
        }

        public override Vector2 Location
        {
            get
            {
                return location;
            }
            set
            {
                location.X = MathHelper.Clamp(value.X, 0, WindowBoundaries.X - Width);
                location.Y = MathHelper.Clamp(value.Y, 0, WindowBoundaries.Y - Height - 20);
            }
        }

        float AlphaChangeRate
        {
            get
            {
                return 1.5f;
            }
        }

        Vector2 Acceleration
        {
            get
            {
                return new Vector2(30, 0);
            }
        }

        #endregion

        #region Collision Detection

        protected override void CollisionTest()
        {
            if (Location.Y == WindowBoundaries.Y - 20)
                velocity.Y = 0;
        }

        #endregion

        #region Physics Helper Methods

        Vector2 Gravity
        {
            get
            {
                return new Vector2(0,15);
            }
        }

        void ApplyPhyics()
        {

            Velocity += Gravity;

            if (Location.Y == WindowBoundaries.Y - Height - 20)
            {
                Velocity = Vector2.Reflect(Velocity, new Vector2(0, 1)) * 0.1f;
                hitTheGround = true;
            }

        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            if (hitTheGround)
                Alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds * AlphaChangeRate;

            if (Alpha < 0.0f)
                Inactive = true;

            Location += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ApplyPhyics();

            base.Update(gameTime);

        }

        #endregion

    }
}


