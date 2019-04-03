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
        private static Texture2D _foregroundTexture;
        private static Vector2 _size = new Vector2(300, 150);
        private static int _activatedButton = 0;

        public static void Initialize()
        {
            string[] nameButtons = new string[] { "test1", "test2", "test3" };

            if (nameButtons.Length > 4)
                _listButton = new ButtonList(nameButtons, 4, _size);
            else
                _listButton = new ButtonList(nameButtons, nameButtons.Length, _size);

            Button.ActionButton[] actions = { ActionButton1, ActionButton2, ActionButton4 };

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
                _activatedButton = Util.mod(_activatedButton - 1, _listButton.numberButton);
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

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice, Player player, Tilemap map, SpriteFont font)
        {
            Rendering.BeginDraw(spriteBatch);
            map.DrawDecor(spriteBatch, am);
            player.Draw(spriteBatch, am);
            map.Draw(spriteBatch, am);
            map.DrawObjects(spriteBatch, am);

            spriteBatch.Draw(ForegroundTexture(GraphicsDevice), Vector2.Zero, Color.White);
            _listButton.Draw(GraphicsDevice, spriteBatch, font);
        }

        private static Texture2D ForegroundTexture(GraphicsDevice GraphicsDevice)
        {
            if (_foregroundTexture == null)
            {
                Color foregroundColor = new Color(40, 40, 40, 150);

                _foregroundTexture = Util.GetRectangleTexture(GraphicsDevice, foregroundColor, (int)(Rendering.VirtualWidth / Rendering.zoomFactor), (int)(Rendering.VirtualHeight / Rendering.zoomFactor));
            }
            return _foregroundTexture;
        }

        private static State ActionButton1()
        {
            Console.WriteLine("test1");
            return State.Pause;
        }

        private static State ActionButton2()
        {
            Console.WriteLine("test2");
            return State.Pause;
        }

        private static State ActionButton3()
        {
            Console.WriteLine("test3");
            return State.Pause;
        }

        private static State ActionButton4()
        {
            Console.WriteLine("test4");
            return State.Main;
        }

        private static State ActionButton5()
        {
            Console.WriteLine("test5");
            return State.Pause;
        }
    }
}