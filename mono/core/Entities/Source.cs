using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace mono.core.Entities
{
    public class Source
    {

        private readonly string id;
        private readonly int radius;
        private readonly int volume;
        private readonly Vector2 position;

        private SoundEffectInstance sfx;

        public Source(string id, int radius, int volume, int x, int y)
        {
            this.id = id;
            this.radius = radius;
            this.volume = volume;
            position = new Vector2(x, y);
            var sound = SoundManager.Content.Load<SoundEffect>("Music/SoundEffects/" + id);
            sfx = sound.CreateInstance();
            sfx.IsLooped = true;
        }

        public void Activate(Vector2 pos)
        {
            SetVolume(pos);
            sfx.Play();
        }

        public void SetVolume(Vector2 pos)
        {
            var dist = Vector2.Distance(position, pos + new Vector2(32f, 32f));
            if (dist > radius)
            {
                sfx.Volume = 0f;
            }
            else
            {
                var vol = volume / 100f * (radius - dist) / radius;
                sfx.Volume = vol;
                Console.WriteLine(dist);
            }
        }
    }
}
