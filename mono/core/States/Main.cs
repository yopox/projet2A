using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.PhysicsEngine;
using mono.RenderEngine;

namespace mono.core.States
{
    static class Main
    {
        public static State Update(Player player, GameTime gameTime, GameState GameState)
        {
            if (Util.NewState)
                Util.NewState = !Util.FadeIn();
            else
            {
                Physics.UpdateAll(gameTime);
                player.Update(GameState, gameTime);
            }
            Camera.Update(player, GameState.map.width * Util.TileSize);
            GameState.map.UpdateSources(player.Position);

            if (GameState.ksn.IsKeyDown(Keys.Tab) && GameState.kso.IsKeyUp(Keys.Tab))
            {
                // Démarrage du menu pause
                //SoundManager.PlayBGM("0_menuchargement_done");

                return State.Textbox;
            }
            return State.Main;
        }

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice, Player player, Tilemap map)
        {
            Rendering.BeginDraw(spriteBatch);
            map.DrawDecor(spriteBatch, am);
            player.Draw(spriteBatch, am);
            map.Draw(spriteBatch, am);
            map.DrawObjects(spriteBatch, am);
            Debuger.DebugActors(GraphicsDevice, spriteBatch);
        }
    }
}
