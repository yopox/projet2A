using Microsoft.Xna.Framework.Input;
using mono.core.PhysicsEngine;
using System;

namespace mono.core
{
    static class PlayerControl
    {
        public static void ReadKeypad(Player player, GameState gstate)
        {
            if (gstate.ksn.IsKeyDown(Keys.D) && gstate.ksn.IsKeyDown(Keys.Q))
                player.speed.X = 0;
            else if (gstate.ksn.IsKeyDown(Keys.D))
                player.Walk(Face.Right);
            else if (gstate.ksn.IsKeyDown(Keys.Q))
                player.Walk(Face.Left);
            else if (Math.Abs(player.speed.X) < 0.2)
                player.Idle();

            if (((gstate.ksn.IsKeyDown(Keys.Z) && gstate.kso.IsKeyUp(Keys.Z)) || (gstate.ksn.IsKeyDown(Keys.Space) && gstate.kso.IsKeyUp(Keys.Space))) && player.CanJump)
                player.Jump();
            if (gstate.ksn.IsKeyDown(Keys.S) && gstate.kso.IsKeyUp(Keys.S))
            {
                player.speed.Y += 1.5f;
            }

            // Activation mode Debug
            if (gstate.ksn.IsKeyDown(Keys.M) && gstate.kso.IsKeyUp(Keys.M))
            {
                var hitbox = player.GetHitbox();

                Console.WriteLine("");
                Console.WriteLine("Number of coll : " + CollisionTester.CollidesWithTerrain(hitbox, gstate.map).Count);

                var tiles = gstate.map.GetTerrain(player.GetHitbox().Center, 4);
                for (int i = 0; i < tiles.Length; i++)
                {
                    Console.WriteLine(String.Join(" ", tiles[i]));
                }
            }

            if (gstate.ksn.IsKeyDown(Keys.F1) && gstate.kso.IsKeyUp(Keys.F1))
            {
                Debuger.debugActors = !Debuger.debugActors;
            }
        }

        public static void ReadController(Player player, GameState gstate)
        {
            player.speed.X = gstate.gsn.ThumbSticks.Left.X * 1.5f;

            if (gstate.gsn.ThumbSticks.Left.X < 0)
                player.NewFacing = Face.Left;
            else if (gstate.gsn.ThumbSticks.Left.X > 0)
                player.NewFacing = Face.Right;
            else if (Math.Abs(player.speed.X) < 0.2)
                player.Idle();

            if (((gstate.gsn.IsButtonDown(Buttons.A) && gstate.gso.IsButtonDown(Buttons.A)) || (gstate.gsn.IsButtonDown(Buttons.A) && gstate.gso.IsButtonDown(Buttons.A))) && player.CanJump)
                player.Jump();
        }
    }
}
