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

    public class Player : Actor
    {

        #region Declarations

        Vector2 velocity;
        Vector2 location;
        string newAnimation;
        float purrTime;
        float mewTime;
        float attackTime;
        Texture2D mewFace;
        #endregion 

        #region Constructor

        public Player(ContentManager Content, Vector2 Location, int Width, int Height,float drawDepth)
            : base(Location, Width, Height,drawDepth)
        {
            animations.Add("idle", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Idle"), 50, "idle"));
            animations["idle"].LoopAnimation = true;

            animations.Add("run", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Run"), 50, "run"));
            animations["run"].LoopAnimation = true;

            animations.Add("attack", new AnimationStrip(Content.Load<Texture2D>(@"Textures\attack"), 50, "attack"));
            animations["attack"].LoopAnimation = true;

            animations.Add("crouch", new AnimationStrip(Content.Load<Texture2D>(@"Textures\crouch"), 50, "crouch"));
            animations["crouch"].LoopAnimation = true;

            animations.Add("mew", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Mew"), 50, "mew"));
            animations["mew"].LoopAnimation = true;

            animations.Add("purr", new AnimationStrip(Content.Load<Texture2D>(@"Textures\purr"), 50, "purr"));
            animations["purr"].LoopAnimation = false;
            animations["purr"].FrameLength = 0.12f;

            animations.Add("jump", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Jump"), 50, "jump"));
            animations["jump"].LoopAnimation = false;

            animations.Add("falling", new AnimationStrip(Content.Load<Texture2D>(@"Textures\Falling"), 50, "falling"));
            animations["falling"].LoopAnimation = true;

            mewFace = Content.Load<Texture2D>(@"Textures\mewFace");

            PlayAnimation("jump");
            timeSincePurr = 0.0f;
            timeSinceMew = 5.0f;
            attackTime = 5.0f;
            newAnimation = "jump";
            MaxVelocity = 400.0f;
            IsCrouching = false;
        }

        #endregion

        bool IsCrouching
        {
            get;
            set;
        }

        bool IsAttacking
        {
            get
            {
                return timeSinceAttack < attackTime;
            }
        }
        float timeSincePurr
        {
            get
            {
                return purrTime;
            }
            set
            {
                purrTime = MathHelper.Min(10.0f, value);
            }
        }

        float timeSinceMew
        {
            get
            {
                return mewTime;
            }
            set
            {
                mewTime = MathHelper.Min(10.0f, value);
            }
        }

        float timeSinceAttack
        {
            get
            {
                return attackTime;
            }
            set
            {
                attackTime = MathHelper.Min(10.0f, value);
            }
        }

        float AttackDuration
        {
            get
            {
                return 0.1f;
            }
        }

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

        public override Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                if (timeSinceAttack > AttackDuration)
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

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) && velocity.Y==0)
            {
                IsCrouching = true;
                timeSincePurr = 0.0f;
            }
            else
            {
                IsCrouching = false;
            }

            if (!IsCrouching)
            {
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

                if (InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Space) && Velocity.Y == 0)
                {
                    Velocity += new Vector2(0, -400);
                    timeSincePurr = 0.0f;
                }
            }
            else
            {
                if (InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
                {
                    IsCrouching = false;
                    //SoundManager.PlayAttack();
                    if (Flipped)
                        velocity.X = 700;
                    else
                        velocity.X = -700;

                    if (velocity.Y == 0)
                        velocity.Y -= 90;
                    timeSinceAttack = 0.0f;
                    timeSincePurr = 0.0f;
                    currentAnimation = "attack";
                }
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

            if (InputManager.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                SoundManager.PlayMew();
                timeSinceMew = 0.0f;
                timeSincePurr = 0.0f;
            }
                     

            timeSinceMew += elapsed;
            timeSinceAttack += elapsed;

            
            Location += Velocity * elapsed;

            if (IsCrouching && !IsAttacking)
                newAnimation = "crouch";

            if (timeSinceAttack > AttackDuration)
            {
                if (newAnimation != currentAnimation)
                    currentAnimation = newAnimation;
                ApplyPhyics();
            }
            base.Update(gameTime);

        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (timeSinceMew < 0.4f)
            {
                SpriteEffects effect = SpriteEffects.None;
                Rectangle rect;
                if (IsCrouching)
                    rect = new Rectangle((int)location.X, (int)location.Y + 5, Width, Height);
                else
                    rect = CollisionRectangle;

                if (Flipped)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }
                spriteBatch.Draw(mewFace, rect,
                    new Rectangle(0,0,50,40), Color.White * Alpha, 0.0f, Vector2.Zero, effect, 0.0f);
            }

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
