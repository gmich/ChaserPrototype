using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HungerPrototype.GameActors
{
    using Animations;

    public class Actor
    {
        #region Declarations

        protected Dictionary<string, AnimationStrip> animations = new Dictionary<string, AnimationStrip>();
        protected string currentAnimation;

        #endregion

        #region Constructor

        public Actor(ContentManager Content, Vector2 location, int Width, int Height)
        {
            this.Location = location;
            this.Width = Width;
            this.Height = Height;
        }

        #endregion 

        #region Properties

        public bool Inactive
        {
            get;
            set;
        }

        #region Movement

        virtual Vector2 Velocity
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

        virtual Vector2 Location
        {
            get;
            set;
        }

        float DrawDepth
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

        protected Rectangle CollisionRectangle
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

        protected void CollisionTest();

        #endregion

        #region Update 

        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateAnimation(gameTime);

        }

        #endregion

        #region Draw

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (animations.ContainsKey(currentAnimation))
            {

                SpriteEffects effect = SpriteEffects.None;

                if (Flipped)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }
                spriteBatch.Draw(animations[currentAnimation].Texture,CollisionRectangle,
                    animations[currentAnimation].FrameRectangle, Color.White, 0.0f, Vector2.Zero, effect, drawDepth);
            }
        }

        #endregion
    }
}
