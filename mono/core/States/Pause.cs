using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.RenderEngine;

namespace mono.core.States
{
    static class Pause
    {
        private static ButtonList listButton;
        static Texture2D ForegroundTexture;
        private static Vector2 size = new Vector2(256, 64);
        private static int activatedButton;

        public static void Initialize()
        {
            string[] nameButtons = new string[] { "Continuer", "Quitter" };

            if (nameButtons.Length > 4)
                listButton = new ButtonList(nameButtons, 4, size);
            else
                listButton = new ButtonList(nameButtons, nameButtons.Length, size);

            Button.ActionButton[] actions = { ActionButton1, ActionButton2 };

            for (int i = 0; i < actions.Length; i++)
            {
                listButton.Buttons[i].SetAction(actions[i]);
            }
        }

        public static State Update(GameState GameState)
        {
            State newState;
            if (GameState.ksn.IsKeyDown(Keys.Tab) && GameState.kso.IsKeyUp(Keys.Tab))
            {
                // Retour à la state main
                SoundManager.PlayBGM("3_REMINISCENCE_OBJET_done");
                newState = State.Main;
            }
            else if (GameState.ksn.IsKeyDown(Keys.S) && GameState.kso.IsKeyUp(Keys.S))
            {
                activatedButton = (activatedButton + 1) % listButton.NumberButton;
                newState = State.Pause;
            }
            else if (GameState.ksn.IsKeyDown(Keys.Z) && GameState.kso.IsKeyUp(Keys.Z))
            {
                activatedButton = Util.Mod(activatedButton - 1, listButton.NumberButton);
                newState = State.Pause;
            }
            else if (GameState.ksn.IsKeyDown(Keys.Enter) && GameState.kso.IsKeyUp(Keys.Enter))
            {
                newState = listButton.Buttons[activatedButton].Action();
            }
            else
                newState = State.Pause;

            listButton.Update(activatedButton);
            return newState;
        }

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice, Player player, Tilemap map)
        {
            Rendering.BeginDraw(spriteBatch);

            spriteBatch.Draw(Util.GetTexture(GraphicsDevice, ForegroundTexture, new Color(40, 40, 40, 150)), Vector2.Zero, Color.White);
            listButton.Draw(GraphicsDevice, spriteBatch);
        }

        private static State ActionButton1() => State.Main;

        private static State ActionButton2() => State.Exit;
    }
}