using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.Definitions;
using mono.RenderEngine;
using System;
using System.Collections.Generic;

namespace mono.core.States
{
    static class Cutscene
    {
        static Queue<CutsceneAction> actions;
        static AtlasName bgImage;
        static float scale = 2f;
        static CutsceneAction nextAction;
        static private List<Tuple<string, string>> _text;

        public static void Load(string path)
        {
            actions = Util.ParseScript(path);
        }

        public static void Update()
        {
            nextAction = actions.Dequeue();
            switch (nextAction.type)
            {
                case CutsceneActionType.Background:
                    bgImage = Util.ParseEnum<AtlasName>(nextAction.content);
                    break;
                case CutsceneActionType.Text:
                    _text = (Util.ParseDialog(nextAction.content));
                    break;
                case CutsceneActionType.NewPage:
                    break;
                case CutsceneActionType.Wait:
                    break;
                case CutsceneActionType.Sfx:
                    break;
                case CutsceneActionType.Bgm:
                    break;
                case CutsceneActionType.State:
                    break;
            }
        }

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice, SpriteFont font)
        {
            var texture = am.GetAtlas(bgImage).Texture;
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);

            DrawDialog(spriteBatch, font);
        }

        static void DrawDialog(SpriteBatch spriteBatch, SpriteFont font)
        {
            Vector2 size = Vector2.Zero;
            int indString = 0;
            int indCharacter = 0;

            for (int i = 0; i < _text.Capacity; i++)
            {
                size.Y += (int)(font.MeasureString(_text[i].Item2).Y * scale);
                if ((int)(font.MeasureString(_text[i].Item2).X * scale) > size.X)
                    size.X = (int)(font.MeasureString(_text[i].Item2).X * scale);
            }

            Vector2 positionStr = new Vector2(Rendering.VirtualWidth / 2 - size.X / 2,
                Rendering.VirtualHeight - size.Y / 2);

            Vector2 offset = Vector2.Zero;

            for (int i = 0; i < indString; i++)
            {
                spriteBatch.DrawString(font,
                _text[i].Item2,
                positionStr + offset,
                Util.ColorStringDictionary[_text[i].Item1],
                0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);

                offset.Y += font.MeasureString(_text[i].Item2).Y * scale;
            }

            for (int i = 0; i <= indCharacter; i++)
            {
                spriteBatch.DrawString(font,
                _text[indString].Item2,
                positionStr + offset,
                Util.ColorStringDictionary[_text[indString].Item1],
                0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);
            }

            if(indCharacter == _text[indString].Item2.Length - 1)
            {
                if(indString < _text.Capacity)
                {
                    indString++;
                    indCharacter = 0;
                }
            }
            else
            {
                indCharacter++;
            }
        }
    }
}
