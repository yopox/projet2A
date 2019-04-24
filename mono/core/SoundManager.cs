using System;
using Microsoft.Xna.Framework.Media;

namespace mono.core
{
    public static class SoundManager
    {
        private static Song song;
        private static Microsoft.Xna.Framework.Content.ContentManager Content;

        public static void SetContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Content = content;
        }

        public static void PlayBGM(string name)
        {
            song = Content.Load<Song>("Music/" + name);
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }

    }
}
