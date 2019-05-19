using System;
using Microsoft.Xna.Framework.Media;

namespace mono.core
{

    enum SoundState
    {
        NONE,
        PLAYING,
        FADE_OUT,
        FADE_IN
    }

    public static class SoundManager
    {
        private static Song song;
        public static Microsoft.Xna.Framework.Content.ContentManager Content;
        private static SoundState state = SoundState.NONE;
        private static float step = 0.04f;
        private static string oldBGM = "";
        private static string currentBGM = "";
        private static string nextBGM = "";

        public static void SetContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Content = content;
        }

        public static void PlayBGM(string name)
        {
            nextBGM = name;
            if (state == SoundState.PLAYING)
                state = SoundState.FADE_OUT;
        }

        public static void Play()
        {
            song = Content.Load<Song>("Music/" + nextBGM);
            oldBGM = currentBGM;
            currentBGM = nextBGM;
            nextBGM = "";
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }

        public static void Update()
        {

            Console.WriteLine(state);

            switch (state)
            {
                case SoundState.NONE:
                    if (nextBGM != "")
                    {
                        MediaPlayer.Volume = 0f;
                        state = SoundState.FADE_IN;
                        Play();
                    }
                    break;
                case SoundState.FADE_IN:
                    MediaPlayer.Volume += step;
                    if (MediaPlayer.Volume >= 1f)
                    {
                        MediaPlayer.Volume = 1f;
                        state = SoundState.PLAYING;
                    }
                    break;
                case SoundState.FADE_OUT:
                    MediaPlayer.Volume -= step;
                    if (MediaPlayer.Volume <= 0f)
                    {
                        MediaPlayer.Volume = 0f;
                        Play();
                        state = SoundState.FADE_IN;
                    }
                    break;
            }
        }

    }
}
