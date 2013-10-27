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

        public Actor(Vector2 location, int Width, int Height,float drawDepth)
        {
            this.Location = location;
            this.Width = Width;
            this.Height = Height;
            this.DrawDepth = drawDepth;
            Flipped = false;
            Inactive = false;
            Velocity = Vector2.Zero;
            Alpha = 1.0f;
        }

        #endregion 

        #region Properties

        public bool Inactive
        {
            get;
            set;
        }

        #region Movement

        public virtual Vector2 Velocity
        {
            get;
            set;
        }

        #endregion

        #region Information About Screen Rendering

        public float Alpha
        {
            get;
            set;
        }

        protected bool Flipped
        {
            get;
            set;
        }

        public virtual Vector2 Location
        {
            get;
            set;
        }

        protected float DrawDepth
        {
            get;
            set;
        }

        protected int Width
        {
            get;
            set;
        }

        protected int Height
        {
            get;
            set;
        }

        protected Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)Location.X, (int)Location.Y, Width, Height);
            }
        }

        #endregion

        #endregion

        #region Helper Methods

        protected void PlayAnimation(string name)
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

        protected virtual void CollisionTest() { }

        public virtual bool CollidesWith(Actor actor)
        {
            return this.CollisionRectangle.Intersects(actor.CollisionRectangle);
        }

        #endregion

        #region Update 

        public virtual void Update(GameTime gameTime)
        {
            CollisionTest();
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
                    animations[currentAnimation].FrameRectangle, Color.White * Alpha, 0.0f, Vector2.Zero, effect, DrawDepth);
            }
        }

        #endregion
    }
}
