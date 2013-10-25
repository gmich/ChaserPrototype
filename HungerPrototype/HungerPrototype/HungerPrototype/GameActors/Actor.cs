using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HungerPrototype.GameActors
{
    using Animations;

    public abstract class Actor
    {
        #region Declarations

        protected Dictionary<string, AnimationStrip> animations = new Dictionary<string, AnimationStrip>();
        protected string currentAnimation;

        #endregion

        #region Properties

        public bool Dead
        {
            get;
            set;
        }

        #region Movement

        abstract float Friction
        {
            get;
        }

        float Mass
        {
            get;
            set;
        }

        abstract Vector2 Velocity
        {
            get;
            set;
        }

        abstract Vector2 Acceleration
        {
            get;
            set;
        }

        #endregion

        #region Information About Screen Rendering

        bool Flipped
        {
            get;
            set;
        }

        abstract Vector2 Location
        {
            get;
            set;
        }

        float Width
        {
            get;
            set;
        }

        float Height
        {
            get;
            set;
        }

        abstract Rectangle CollisionRectangle
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Helper Methods

        void PlayAnimation(string name)
        {
            if (!(name == null) && animations.ContainsKey(name))
            {
                currentAnimation = name;
                animations[name].Play();
            }
        }

        void UpdateAnimation(GameTime gameTime)
        {
            if (animations.ContainsKey(currentAnimation))
            {
                if (animations[currentAnimation].FinishedPlaying)
                {
                    PlayAnimation(animations[currentAnimation].NextAnimation);
                }
                else
                {
                    animations[currentAnimation].Update(gameTime);
                }
            }
        }

        #endregion

        #region Collision Detection Methods

        abstract void CollisionTest();

        #endregion

        #region Update 

        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateAnimation(gameTime);

        }

        #endregion

        abstract  void Draw(SpriteBatch spriteBatch)
        {
            if (animations.ContainsKey(currentAnimation))
            {

                SpriteEffects effect = SpriteEffects.None;

                if (Flipped)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }
                spriteBatch.Draw(animations[currentAnimation].Texture, Camera.WorldToScreen(WorldRectangle),
                    animations[currentAnimation].FrameRectangle, Color.White, 0.0f, Vector2.Zero, effect, drawDepth);
            }
        }

        #endregion
    }
}
