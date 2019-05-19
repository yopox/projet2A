using System;
using Microsoft.Xna.Framework.Media;

namespace mono.core
{
    public static class SoundManager
    {
        private static Song song;
        public static Microsoft.Xna.Framework.Content.ContentManager Content;

        public static void SetContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Content = content;
        }

        public static void PlayBGM(string name)
        {
            song = Content.Load<Song>("Music/" + name);
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 1f;
            MediaPlayer.IsRepeating = true;
        }

    }
}
