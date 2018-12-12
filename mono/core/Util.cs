﻿using Microsoft.Xna.Framework;

namespace mono.core
{
    public static class Util
    {
        // Screen
        static public int width = 640;
        static public int height = 360;
        static public Vector2 center = new Vector2(width / 2, height / 2);

        // Tileset
        static public int tileSize = 32;
        static public string solidLayerName = "terrain";

        // Player
        static public int playerSize = 30;
    }
}