using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.RenderEngine;
using System;

namespace mono.core.States
{
    static class Pause
    {
        private static ButtonList _listButton;
        static Texture2D ForegroundTexture;
        private static Vector2 _size = new Vector2(256, 64);
        private static int _activatedButton;

        public static void Initialize()
        {
            string[] nameButtons = new string[] { "Continuer", "Options", "Quitter" };

            if (nameButtons.Length > 4)
                _listButton = new ButtonList(nameButtons, 4, _size);
            else
                _listButton = new ButtonList(nameButtons, nameButtons.Length, _size);

            Button.ActionButton[] actions = { ActionButton1, ActionButton2, ActionButton3 };

            for (int i = 0; i < actions.Length; i++)
            {
                _listButton.Buttons[i].SetAction(actions[i]);
            }
        }

        public static State Update(GameState GameState)
        {
            State newState;
            if (GameState.ksn.IsKeyDown(Keys.Tab) && GameState.kso.IsKeyUp(Keys.Tab))
                newState = State.Main;
            else if (GameState.ksn.IsKeyDown(Keys.S) && GameState.kso.IsKeyUp(Keys.S))
            {
                _activatedButton = (_activatedButton + 1) % _listButton.numberButton;
                newState = State.Pause;
            }
            else if (GameState.ksn.IsKeyDown(Keys.Z) && GameState.kso.IsKeyUp(Keys.Z))
            {
                _activatedButton = Util.Mod(_activatedButton - 1, _listButton.numberButton);
                newState = State.Pause;
            }
            else if (GameState.ksn.IsKeyDown(Keys.Enter) && GameState.kso.IsKeyUp(Keys.Enter))
            {
                newState = _listButton.Buttons[_activatedButton].Action();
            }
            else
                newState = State.Pause;

            _listButton.Update(_activatedButton);
            return newState;
        }

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice, Player player, Tilemap map)
        {
            Rendering.BeginDraw(spriteBatch);
            map.DrawDecor(spriteBatch, am);
            player.Draw(spriteBatch, am);
            map.Draw(spriteBatch, am);
            map.DrawObjects(spriteBatch, am);

            spriteBatch.Draw(Util.GetTexture(GraphicsDevice, ForegroundTexture, new Color(40, 40, 40, 150)), Vector2.Zero, Color.White);
            _listButton.Draw(GraphicsDevice, spriteBatch);
        }

        private static State ActionButton1() => State.Main;

        private static State ActionButton2() => State.Pause;

        private static State ActionButton3() => State.Pause;

    }
}