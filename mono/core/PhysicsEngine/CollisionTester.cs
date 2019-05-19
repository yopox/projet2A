using System;
using System.Collections.Generic;

namespace mono.core.PhysicsEngine
{
    public static class CollisionTester
    {

        public static List<Polygon> CollidesWithTerrain(Rect hitbox, Tilemap map)
        {
            List<Polygon> collisions = new List<Polygon>();
            int radius = 4;

            // On récupère les tiles autour de la hitbox
            int[][] tiles = map.GetTerrain(hitbox.Center, radius);
            int x = (int)Math.Floor(hitbox.Center.X / Util.TileSize);
            int y = (int)Math.Floor(hitbox.Center.Y / Util.TileSize);

            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    // Coordonnées du tile à tester
                    int x2 = x + j - radius;
                    int y2 = y + i - radius;

                    // On récupère le polygone associé à tiles[i][j]
                    Polygon p = Polygon.FromTile(tiles[i][j], x2 * Util.TileSize, y2 * Util.TileSize);

                    // S'il y a collision on ajoute le polygone à la liste
                    if (hitbox.CollidesWith(p))
                    {
                        collisions.Add(p);
                    }
                }
            }

            return collisions;
        }

    }
}
