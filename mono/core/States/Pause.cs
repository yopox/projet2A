using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.core.States
{
    static class Pause
    {
        private static Texture2D _foregroundTexture;

        public static State Update(GameState GameState)
        {
            if (GameState.ksn.IsKeyDown(Keys.Tab) && GameState.kso.IsKeyUp(Keys.Tab))
                return State.Main;
            else
                return State.Pause;
        }

        public static void Draw(SpriteBatch spriteBatch, AssetManager am, GraphicsDevice GraphicsDevice, Player player, Tilemap map)
        {
            Rendering.BeginDraw(spriteBatch);
            map.DrawDecor(spriteBatch, am);
            player.Draw(spriteBatch, am);
            map.Draw(spriteBatch, am);
            map.DrawObjects(spriteBatch, am);
            
            spriteBatch.Draw(ForegroundTexture(GraphicsDevice), Vector2.Zero, Color.White);
            //spriteBatch.DrawString(SpriteFont.Glyph, "test", Vector2.Zero, Color.White);

        }

        private static Texture2D ForegroundTexture(GraphicsDevice GraphicsDevice)
        {
            if(_foregroundTexture == null)
            {
                Color foregroundColor = new Color(40, 40, 40, 150);

                _foregroundTexture = new Texture2D(GraphicsDevice,
                (int)(Rendering.VirtualWidth / Rendering.zoomFactor),
                (int)(Rendering.VirtualHeight / Rendering.zoomFactor));
                Color[] data = new Color[(int)(Rendering.VirtualWidth / Rendering.zoomFactor) * (int)(Rendering.VirtualHeight / Rendering.zoomFactor)];
                for (int i = 0; i < data.Length; ++i) data[i] = foregroundColor;
                _foregroundTexture.SetData(data);
            }
            return _foregroundTexture;
        }
    }
}
