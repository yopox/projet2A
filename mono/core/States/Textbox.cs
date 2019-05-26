using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace mono.core.States
{
    enum TextboxState
    {
        FADE_IN,
        MAIN,
        FADE_OUT,
        MORE_TEXT_AWAIT
    }

    static class Textbox
    {
        private static TextboxState state = TextboxState.FADE_IN;
        private static List<Tuple<string, string>> text;
        private static int cursor;
        private static Tuple<string, string> currentText;
        private static string erasedText = string.Empty;
        private static string displayedText = string.Empty;
        private static int lineNb;

        private static int frame;
        private static readonly int MAX_FRAME = 12;
        private static int charFrame;
        private static readonly int CHAR_SPEED = 2;

        private static double WIDTH = Util.Width * 0.8;
        private static double HEIGHT = Util.Height * 0.24;
        private static Vector2 pos = new Vector2(Util.Width * 0.1f, Util.Height * 0.70f);
        private static float textAlpha = 1f;
        private static Texture2D background;

        public static void SetText(List<Tuple<string, string>> newText)
        {
            // Reset state
            displayedText = string.Empty;
            erasedText = string.Empty;
            frame = 0;
            textAlpha = 1f;
            state = TextboxState.FADE_IN;
            cursor = 0;
            lineNb = 0;

            // MAJ texte
            text = newText;
            currentText = text[cursor];
            cursor++;
        }

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
                        return State.Main;
                    break;

                case TextboxState.MAIN:
                    var textLen = displayedText.Length + erasedText.Length;
                    if (textLen < currentText.Item2.Length)
                    {
                        if (charFrame == 0)
                        {
                            // Prochain caractère à afficher
                            var newChar = currentText.Item2[textLen];

                            // Cas particulier : il faut changer de page
                            if (newChar == '\n' && lineNb == 2)
                            {
                                if (Util.JustPressed(gameState, Keys.Space))
                                {
                                    displayedText += newChar;
                                    frame = MAX_FRAME;
                                    state = TextboxState.MORE_TEXT_AWAIT;
                                    erasedText += displayedText;
                                }
                            }

                            // On ne change pas de page
                            else
                            {
                                if (newChar == '\n')
                                    lineNb++;
                                displayedText += newChar;
                                UpdateWaitingTime(newChar);
                            }
                        }
                        else
                        {
                            charFrame--;
                        }
                    }
                    // La prise de parole est finie
                    else
                    {
                        // On attend un input
                        if (Util.JustPressed(gameState, Keys.Space))
                        {
                            if (text.Count < cursor)
                            {
                                currentText = text[cursor];
                                cursor++;
                                frame = MAX_FRAME;
                                state = TextboxState.MORE_TEXT_AWAIT;
                                erasedText = string.Empty;
                            }
                            else
                            {
                                frame = MAX_FRAME;
                                state = TextboxState.FADE_OUT;
                            }
                        }
                    }
                    break;

                case TextboxState.MORE_TEXT_AWAIT:
                    // Maj opacité du texte
                    frame--;
                    textAlpha = frame / (float)MAX_FRAME;

                    // On affiche la suite du texte
                    if (frame == 0)
                    {
                        displayedText = string.Empty;
                        textAlpha = 1;
                        lineNb = 0;
                        state = TextboxState.MAIN;
                    }

                    break;
            }
            return State.Textbox;
        }

        private static void UpdateWaitingTime(char newChar)
        {
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

    }
}
