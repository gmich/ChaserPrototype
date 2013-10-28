using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HungerPrototype.Animations
{
    public class HungerBar
    {
        #region Declarations

        Vector2 Location;
        float fillTime;
        float hungerTimeStatus;
        Vector2 frameSize;
        Texture2D frameTexture;
        Texture2D fillingTexture;
        SpriteFont font;
        Color color;
        string text;

        #endregion

        #region Constructor

        public HungerBar(SpriteFont font, Texture2D frameTexture, Texture2D fillingTexture, Vector2 location, Vector2 frameSize, float fillTime,Color color, string text)
        {
            this.frameTexture = frameTexture;
            this.fillingTexture = fillingTexture;
            this.Location = location;
            this.frameSize = frameSize;
            this.fillTime = fillTime;
            this.hungerTimeStatus = fillTime;
            this.font = font;
            this.color = color;
            this.text = text;
        }

        #endregion

        #region Properties

        public Boolean IsHungry
        {
            get
            {
                return hungerTimeStatus == 0.0f;
            }
        }

        public void FillHunger()
        {
            hungerTimeStatus = fillTime;
        }

        public void ManipulateHunger(float amount)
        {
            hungerTimeStatus = MathHelper.Clamp(hungerTimeStatus + amount, 0.0f, fillTime);
        }

        Rectangle FrameRectangle
        {
            get
            {
                int offSet=2;
                return new Rectangle((int)Location.X+offSet, (int)Location.Y+offSet,(int)frameSize.X-offSet, (int)frameSize.Y-offSet); 
            }
        }

        float Progress
        {
            get
            {
                return (hungerTimeStatus / fillTime);
            }
        }

        public Rectangle HungerBarRectangle
        {
            get
            {
                int offSet = 2;

                return new Rectangle((int)Location.X + offSet, (int)Location.Y + offSet, (int)(frameSize.X * Progress), (int)frameSize.Y - offSet);
            }
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ManipulateHunger(-elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(frameTexture, FrameRectangle, null,Color.White,0.0f,Vector2.Zero,SpriteEffects.None,0.8f);

            spriteBatch.Draw(fillingTexture, HungerBarRectangle, null, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0.9f);

            spriteBatch.DrawString(font, text, Location + new Vector2(frameSize.X + 10, 0), color);
        }

    }
}


