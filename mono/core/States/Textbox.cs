using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace mono.core.States
{
    enum TextboxState
    {
        FADE_IN,
        MAIN,
        FADE_OUT
    }

    static class Textbox
    {
        private static TextboxState state = TextboxState.FADE_IN;
        private static string text = "This is an example text.\nWow second line ;o\nEven third line, this is so awesome.";
        private static string displayedText = string.Empty;

        private static int frame = 0;
        private static readonly int MAX_FRAME = 12;
        private static int charFrame = 0;
        private static readonly int CHAR_SPEED = 2;

        private static double WIDTH = Util.Width * 0.8;
        private static double HEIGHT = Util.Height * 0.24;
        private static Vector2 pos = new Vector2(Util.Width * 0.1f, Util.Height * 0.70f);
        private static float textAlpha = 1f;
        private static Texture2D background;

        public static State Update(GameState gameState, GraphicsDevice graphicsDevice)
        {
            switch (state)
            {
                case TextboxState.FADE_IN:
                    frame++;
                    // MAJ texture rectangle
                    background = Util.GetRectangleTexture(graphicsDevice, Color.Black * (frame / 20f), (int)WIDTH, (int)HEIGHT);
                    if (frame == MAX_FRAME)
                        state = TextboxState.MAIN;
                    break;
                case TextboxState.FADE_OUT:
                    frame--;
                    textAlpha = frame / (float)MAX_FRAME;
                    // MAJ texture rectangle
                    background = Util.GetRectangleTexture(graphicsDevice, Color.Black * (frame / 20f), (int)WIDTH, (int)HEIGHT);
                    if (frame == 0)
                    {
                        ResetState();
                        return State.Main;
                    }
                    break;
                case TextboxState.MAIN:
                    if (displayedText.Length < text.Length)
                    {
                        if (charFrame == 0)
                        {
                            var newChar = text[displayedText.Length];
                            displayedText += newChar;
                            switch (newChar)
                            {
                                case ',':
                                    charFrame = 2 * CHAR_SPEED;
                                    break;
                                case '\n':
                                case '.':
                                case '!':
                                case '?':
                                    charFrame = 3 * CHAR_SPEED;
                                    break;
                                default:
                                    charFrame = CHAR_SPEED;
                                    break;
                            }
                        }
                        else
                        {
                            charFrame--;
                        }
                    }
                    else
                    {
                        // On attend un input
                        if (Util.JustPressed(gameState, Keys.Space))
                        {
                            state = TextboxState.FADE_OUT;
                        }
                    }
                    break;
            }
            return State.Textbox;
        }

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice graphicsDevice)
        {

            // Dessin de la boîte
            if (background != null)
                spriteBatch.Draw(background, pos, Color.Black);

            // Dessin du texte
            spriteBatch.DrawString(Util.Font,
                displayedText,
                pos + new Vector2(48, 22),
                Color.White * textAlpha, 0.0f, Vector2.Zero, 3f, new SpriteEffects(), 0.0f);
        }
        
        private static void ResetState()
        {
            text = "C'est cool d'essayer une deuxième fois Nico,\nça veut dire que la textbox doit te plaire ;p";
            displayedText = string.Empty;
            frame = 0;
            textAlpha = 1f;
            state = TextboxState.FADE_IN;
        }
    }
}
