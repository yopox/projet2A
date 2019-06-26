using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.PhysicsEngine;
using mono.RenderEngine;

namespace mono.core.States
{
    static class Main
    {
        public static State Update(Player player, GameTime gameTime, GameState gameState, bool block = false)
        {
            if (Util.NewState)
            {
                Util.NewState = !Util.FadeIn();
                SoundManager.PlayBGM(gameState.map.song);
            }
            else
            {
                Physics.UpdateAll(gameTime);
                player.Update(gameState, gameTime, block);
            }
            Camera.Update(player, gameState.map.width * Util.TileSize);
            gameState.map.UpdateSources(player.Position);

            if (Util.JustPressed(gameState, Keys.Tab))
            {
                // Démarrage du menu pause
                SoundManager.PlayBGM("0_menuchargement_done");
                return State.Pause;
            }

            if (Util.JustPressed(gameState, Keys.E) && (player.State == PlayerState.Idle || player.State == PlayerState.Walking))
            {
                var text = gameState.map.InteractionText(player);
                if (text != null)
                {
                    Textbox.SetText(text);
                    if (player.State == PlayerState.Walking)
                    {
                        player.NewState = PlayerState.Idle;
                        player.Speed = Vector2.Zero;
                    }
                    return State.Textbox;
                }
            }
            return State.Main;
        }

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice, Player player, Tilemap map)
        {
            map.DrawParallax(spriteBatch, am);
            map.DrawLayer(spriteBatch, am, "decorB3");
            map.DrawLayer(spriteBatch, am, "decorB2");
            map.DrawLayer(spriteBatch, am, "decorB1");
            player.Draw(spriteBatch, am);
            map.DrawLayer(spriteBatch, am, "terrain");
            map.DrawLayer(spriteBatch, am, "decorF");
            Debuger.DebugActors(GraphicsDevice, spriteBatch);
        }
    }
}
