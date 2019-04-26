using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
            var sound = SoundManager.Content.Load<SoundEffect>(id);
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
            var dist = Vector2.Distance(position, pos);
            if (dist > radius)
            {
                sfx.Volume = 0f;
            }
            else
            {
                sfx.Volume = volume / 100f * (radius - dist) / radius;
            }
        }
    }
}
