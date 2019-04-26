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
            Physics.UpdateAll(gameTime);
            player.Update(GameState, gameTime);
            Camera.Update(player, GameState.map.width * Util.tileSize);
            GameState.map.UpdateSources(player.position);

            if (GameState.ksn.IsKeyDown(Keys.Tab) && GameState.kso.IsKeyUp(Keys.Tab))
                return State.Pause;
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
