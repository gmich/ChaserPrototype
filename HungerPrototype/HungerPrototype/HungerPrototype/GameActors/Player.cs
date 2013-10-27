using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HungerPrototype.GameActors
{
    using Input;
    using Animations;
    public class Player : Actor
    {

        #region Declarations

        Vector2 velocity;
        Vector2 location;
        float timeSincePurr;

        #endregion 

        #region Constructor

        public Player(ContentManager Content, Vector2 Location, int Width, int Height,float drawDepth)
            : base(Location, Width, Height,drawDepth)
        {
            animations.Add("idle", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Idle"), 50, "idle"));
            animations["idle"].LoopAnimation = true;

            animations.Add("run", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Run"), 50, "run"));
            animations["run"].LoopAnimation = true;

            animations.Add("purr", new AnimationStrip(Content.Load<Texture2D>(@"Textures\purr"), 50, "purr"));
            animations["purr"].LoopAnimation = false;
            animations["purr"].FrameLength = 0.12f;

            animations.Add("jump", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Jump"), 50, "jump"));
            animations["jump"].LoopAnimation = false;

            animations.Add("falling", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Falling"), 50, "falling"));
            animations["falling"].LoopAnimation = true;

            PlayAnimation("idle");
            timeSincePurr = 0.0f;
        }

        #endregion

        #region Override Directional Information

        float MaxVelocity
        {
            get
            {
                return 400.0f;
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
                location.X = MathHelper.Clamp(value.X, 0, WindowBoundaries.X - Width);
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
                return new Vector2(0,15);
            }
        }

        void ApplyPhyics()
        {
            if (Velocity.X != 0)
            {
                if (Flipped)
                    velocity.X = MathHelper.Max(0, velocity.X- Friction.X);
                else
                    velocity.X = MathHelper.Min(0, velocity.X + Friction.X);
            }

            Velocity += Gravity;

            if (Location.Y == WindowBoundaries.Y - Height - 20)
                velocity.Y = 0;

            if (Location.X == WindowBoundaries.X - Width && Flipped)
                velocity.X = 0;

            if (Location.X == 0 && !Flipped)
                velocity.X = 0;

        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            string newAnimation = "idle";
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                Flipped = false;
                Velocity -= Acceleration;
            }
            if (InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                Flipped = true;
                Velocity += Acceleration;
            }
            if (InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Space) && Velocity.Y==0)
            {
                Velocity += new Vector2(0,-400);
            }

            if (Velocity.X != 0)
            {
                newAnimation = "run";
                timeSincePurr = 0.0f;
            }
            else
            {
                newAnimation = "idle";
                timeSincePurr += elapsed;
            }

            if (Velocity.Y != 0)
                newAnimation = "jump";

            if (timeSincePurr > 4.0f)
                newAnimation = "purr";

            if (newAnimation != currentAnimation)
                currentAnimation = newAnimation;

            Location += Velocity * elapsed;

            ApplyPhyics();

            base.Update(gameTime);

        }

        #endregion

    }
}
