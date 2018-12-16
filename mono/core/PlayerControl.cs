using Microsoft.Xna.Framework.Input;
using mono.core.PhysicsEngine;
using System;

namespace mono.core
{
    static class PlayerControl
    {
        public static void ReadKeypad(Player player, GameState gstate)
        {
            if (Math.Abs(player.speed.Y) < 0.2 && Math.Abs(player.acceleration.Y) < 0.2)
            {
                player.CanJump = true;
            }
            else
            {
                player.CanJump = false;
            }

            if (gstate.ksn.IsKeyDown(Keys.D))
            {
                player.NewFacing = Face.Right;
                player.NewState = State.Walking;

                player.speed.X = 1.5f;
            }
            else if (gstate.ksn.IsKeyDown(Keys.Q))
            {
                player.NewFacing = Face.Left;
                player.NewState = State.Walking;

                player.speed.X = -1.5f;
            }
            else if (Math.Abs(player.speed.X) < 0.2)
            {
                player.NewState = State.Idle;
            }
            else
            {
                player.speed.X = 0;
            }

            if (((gstate.ksn.IsKeyDown(Keys.Z) && gstate.kso.IsKeyUp(Keys.Z)) || (gstate.ksn.IsKeyDown(Keys.Space) && gstate.kso.IsKeyUp(Keys.Space))) && player.CanJump)
            {
                player.forces.Y = -15000;
                player.speed.Y -= 0.8f;
            }

            if (gstate.ksn.IsKeyDown(Keys.S))
            {
                player.forces.Y = 10;
            }

            // Activation mode Debug
            if (gstate.ksn.IsKeyDown(Keys.M) && gstate.kso.IsKeyUp(Keys.M))
            {
                var hitbox = player.GetHitboxes()[0];

                Console.WriteLine("");
                Console.WriteLine("Number of coll : " + CollisionTester.CollidesWithTerrain(hitbox, gstate.map).Count);

                var tiles = gstate.map.GetTerrain(player.GetHitboxes()[0].Center, 4);
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
            player.speed.X = gstate.gamePadState.ThumbSticks.Left.X * 1500;
            if (gstate.gamePadState.IsButtonDown(Buttons.A))
                Console.WriteLine("bouton A");
            //if (gstate.gamePadState.IsConnected)
                //Console.WriteLine("connectée");
        }
    }
}
