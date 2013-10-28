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
        private static SoundEffect attack;

        public static void Initialize(ContentManager content)
        {
            try
            {
                mew = content.Load<SoundEffect>(@"Sounds\mew");
                ding = content.Load<SoundEffect>(@"Sounds\ding");
                attack = content.Load<SoundEffect>(@"Sounds\attack");
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
                mew.Play(0.4f,0.0f,0.0f);
              
            }
            catch
            {
                Debug.Write("PlayMew Failed");
            }
        }

        public static void PlayDing(float pitch)
        {
            try
            {
                ding.Play(0.2f, pitch, 0.0f);

            }
            catch
            {
                Debug.Write("PlayDing Failed");
            }
        }

        public static void PlayAttack()
        {
            try
            {
                attack.Play(0.5f,1.0f, 0.0f);

            }
            catch
            {
                Debug.Write("PlayAttack Failed");
            }
        }

    }
}