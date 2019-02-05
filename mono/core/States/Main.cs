﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.PhysicsEngine;
using mono.RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.core.States
{
    static class Main
    {
        public static void Update(Player player, GameTime gameTime, GameState GameState)
        {
            Physics.UpdateAll(gameTime);
            player.Update(GameState, gameTime);
            Camera.Update(player);
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
