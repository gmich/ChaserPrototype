using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace HungerPrototype.Managers
{
    public static class SoundManager
    {
        private static SoundEffect mew;
        private static SoundEffect ding;

        public static void Initialize(ContentManager content)
        {
            try
            {
                mew = content.Load<SoundEffect>(@"Sounds\mew");
                ding = content.Load<SoundEffect>(@"Sounds\ding");
            }
            catch
            {
                Debug.Write("SoundManager Initialization Failed");
            }
        }

    
        public static void PlayMew()
        {
            try
            {
                mew.Play(0.1f,0.0f,0.0f);
              
            }
            catch
            {
                Debug.Write("PlayMew Failed");
            }
        }

        public static void PlayDing()
        {
            try
            {
                ding.Play(0.2f, 1.0f, 0.0f);

            }
            catch
            {
                Debug.Write("PlayDing Failed");
            }
        }

    }
}