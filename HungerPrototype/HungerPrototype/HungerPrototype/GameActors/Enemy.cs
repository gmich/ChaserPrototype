using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HungerPrototype.GameActors
{
    using Input;
    using Animations;
    using Managers;

    public class Enemy : Actor
    {

        #region Declarations

        Vector2 velocity;
        Vector2 location;
        Actor target;
        HungerBar hungerBar;

        #endregion

        #region Constructor

        public Enemy(ContentManager Content, Texture2D animationStrip, Vector2 Location, int Width, int Height, float drawDepth)
            : base(Location, Width, Height, drawDepth)
        {

            animations.Add("run", new AnimationStrip(animationStrip, 50, "run"));
            animations["run"].LoopAnimation = true;

            currentAnimation = "run";
            MaxVelocity = 300.0f;
            hungerBar = new HungerBar(Content.Load<SpriteFont>(@"Fonts\nameFont"),Content.Load<Texture2D>(@"Textures\HungerBar\frameTexture"), Content.Load<Texture2D>(@"Textures\HungerBar\fillingTexture"), new Vector2(10, 150), new Vector2(280, 30), 25.0f,Color.Blue,"Opponent");
        }

        #endregion

        #region Override Directional Information

        Vector2 LocationToChase
        {
            get
            {
                if (target == null)
                    return new Vector2(500, 0);
                else
                    return (target.Location - new Vector2(target.Width / 2, target.Height / 2));
            }
        }

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
                location.X = value.X;
                location.Y = MathHelper.Clamp(value.Y, 0, WindowBoundaries.Y - Height - 20);
            }
        }

        public override Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity.X = MathHelper.Clamp(value.X, -MaxVelocity, MaxVelocity);
                velocity.Y = value.Y;
            }
        }

        Vector2 Acceleration
        {
            get
            {
                return new Vector2(40, 0);
            }
        }

        #endregion

        #region HungerBar

        public void ManipulateHungerBar(float value)
        {
            hungerBar.ManipulateHunger(value);
        }

        public bool IsHungry()
        {
            return hungerBar.IsHungry;
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

        Vector2 Friction
        {
            get
            {
                return new Vector2(15, 0);
            }
        }

        Vector2 Gravity
        {
            get
            {
                return new Vector2(0, 15);
            }
        }

        void ApplyPhyics()
        {
            if (Velocity.X != 0)
            {
                if (Flipped)
                    velocity.X = MathHelper.Max(0, velocity.X - Friction.X);
                else
                    velocity.X = MathHelper.Min(0, velocity.X + Friction.X);
            }

            Velocity += Gravity;

            if (Location.Y == WindowBoundaries.Y - Height - 20)
                velocity.Y = 0;

            if (Location.X == 0 && !Flipped)
                velocity.X = 0;
        }

        #endregion

        #region Chasing

        public void SetTarget(Actor actor)
        {
            this.target = actor;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!(LocationToChase.X + 5 > location.X && LocationToChase.X - 5 < location.X))
            {
                if (LocationToChase.X < location.X)
                {
                    Flipped = false;
                    Velocity -= Acceleration;
                }
                else if (LocationToChase.X > location.X)
                {
                    Flipped = true;
                    Velocity += Acceleration;
                }
            }

            //Jumping option
            if ((LocationToChase.Y + 50) < location.Y &&
               (LocationToChase.X - 100 < location.X && LocationToChase.X + 100 > location.X)
                && velocity.Y == 0)
            {
                Velocity += new Vector2(0, -200);
            }
            
            Location += Velocity * elapsed;

            ApplyPhyics();

            base.Update(gameTime);

            hungerBar.Update(gameTime);
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            hungerBar.Draw(spriteBatch);
        }
    }
}
