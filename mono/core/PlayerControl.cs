﻿using Microsoft.Xna.Framework.Input;
using mono.core.PhysicsEngine;
using System;

namespace mono.core
{
    static class PlayerControl
    {
        public static void ReadKeypad(Player player, GameState gstate)
        {
            if (Util.Pressed(gstate, Keys.D) && Util.Pressed(gstate, Keys.Q))
                player.Idle();
            else if (Util.Pressed(gstate, Keys.D))
                player.Move(Face.Right);
            else if (Util.Pressed(gstate, Keys.Q))
                player.Move(Face.Left);
            else
                player.Idle();

            if (player.CanJump && (Util.JustPressed(gstate, Keys.Z) || Util.JustPressed(gstate, Keys.Space)))
                player.Jump();

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
                Debuger.DebugingActors = !Debuger.DebugingActors;
            }
        }

        public static void ReadController(Player player, GameState gstate)
        {/*
            player.Speed.X = gstate.gsn.ThumbSticks.Left.X * 1.5f;

            if (gstate.gsn.ThumbSticks.Left.X < 0)
                player.NewFacing = Face.Left;
            else if (gstate.gsn.ThumbSticks.Left.X > 0)
                player.NewFacing = Face.Right;
            else if (Math.Abs(player.Speed.X) < 0.2)
                player.Idle();

            if (((gstate.gsn.IsButtonDown(Buttons.A) && gstate.gso.IsButtonDown(Buttons.A)) || (gstate.gsn.IsButtonDown(Buttons.A) && gstate.gso.IsButtonDown(Buttons.A))) && player.CanJump)
                player.Jump();*/
        }
    }
}
