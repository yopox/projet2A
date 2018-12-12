using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

namespace mono.core
{

    /// <summary>
    /// Représente une couche de la tilemap.
    /// </summary>
    public class Layer
    {
        public readonly string name;
        public int[][] tiles;

        public Layer(dynamic layer, int width, int height)
        {
            name = layer.name;

            // On construit le tableau de tiles
            int[] tiles1 = layer.data.ToObject(typeof(int[]));
            tiles = new int[height][];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tiles[i] = tiles1.Skip(i * width).Take(width).ToArray();
                }
            }
        }
    }

    /// <summary>
    /// Permet de charger une tilemap au format JSON.
    /// </summary>
    public class Tilemap
    {

        public static int[] warpGids = new int[] { 1 };

        readonly int height;
        readonly int width;

        readonly int xTileRange = Util.width / Util.tileSize / 2 + 1;
        readonly int yTileRange = Util.height / Util.tileSize / 2 + 2;

        List<Layer> layers = new List<Layer>();
        List<MapObject> objects = new List<MapObject>();
        List<Warp> warps = new List<Warp>();

        public Tilemap(string name, string json)
        {
            // On parse la tilemap
            dynamic map = JObject.Parse(json);

            // Informations sur la tilemap
            height = map.height;
            width = map.width;

            // Couches
            foreach (dynamic layer in map.layers)
            {
                if (layer.type == "tilelayer")
                {
                    layers.Add(new Layer(layer, width, height));
                }
                else if (layer.type == "objectgroup")
                {
                    foreach (dynamic obj in layer.objects)
                    {
                        int x = obj.x;
                        int y = obj.y;
                        int id = obj.gid;
                        if (warpGids.Contains(id))
                        {
                            string type = obj.type;
                            Debug.Print(type);
                            warps.Add(new Warp(id, new Vector2(x, y), type));
                        }
                        else
                        {
                            objects.Add(new MapObject(id, new Vector2(x, y)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Renvoie la position de départ du joueur.
        /// </summary>
        internal Vector2 GetStartingPosition()
        {
            foreach (Warp warp in warps)
            {
                if (warp.type == "starting")
                {
                    return warp.position - new Vector2(0, Util.playerHeight);
                }
            }
            return new Vector2(0, 0);
        }

        /// <summary>
        /// Permet de récupérer les tiles correspondant à la couche de nom <param>layerName</param>.
        /// </summary>
        /// <param name="layerName">Nom du calque</param>
        public int[][] GetTiles(string layerName)
        {
            foreach (Layer layer in layers)
            {
                if (layer.name == layerName)
                {
                    return layer.tiles;
                }
            }

            return new int[height][];
        }

        /// <summary>
        /// Permet de récupérer les tiles de terrain autour d'un point.
        /// </summary>
        /// <returns>The terrain.</returns>
        /// <param name="position">La position centrale.</param>
        /// <param name="radius">Nombre de tiles à récupérer de chaque côté.</param>
        public int[][] GetTiles(Vector2 position, int radius)
        {
            int diametre = 2 * radius + 1;
            int[][] tiles = new int[diametre][];
            int[][] terrain = GetTiles(Util.solidLayerName);

            int x = (int)Math.Floor(position.X / Util.tileSize);
            int y = (int)Math.Floor(position.Y / Util.tileSize);

            for (int i = 0; i < diametre; i++)
            {
                int[] subTiles = new int[diametre];

                for (int j = 0; j < diametre; j++)
                {
                    var x2 = x + j - radius;
                    var y2 = y + i - radius;

                    if (x2 >= 0 && y2 >= 0 && x2 < width && y2 < height)
                    {
                        subTiles[j] = terrain[y2][x2];
                    }
                    else
                    {
                        subTiles[j] = -2;
                    }
                }
                tiles[i] = (int[])subTiles.Clone();
            }

            return tiles;
        }

        /// <summary>
        /// Dessine la tilemap.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        /// <param name="atlas">Atlas du tileset.</param>
        public void Draw(SpriteBatch spriteBatch, Atlas atlas, Camera camera)
        {
            var terrain = GetTiles("terrain");
            int centerTileX = (int)camera.center.X / Util.tileSize;
            int centerTileY = (int)camera.center.Y / Util.tileSize;

            for (int i = centerTileY - yTileRange; i < centerTileY + yTileRange; i++)
            {
                for (int j = centerTileX - xTileRange; j < centerTileX + xTileRange; j++)
                {
                    if (0 <= i && i < height && 0 <= j && j < width && terrain[i][j] > 0)
                        spriteBatch.Draw(atlas.Texture, Util.center + new Vector2(j * Util.tileSize, i * Util.tileSize) - camera.center,
                                     atlas.GetSourceRectangle(terrain[i][j] - 1),
                                     Color.White, 0f, new Vector2(0, 0), 1f,
                                     SpriteEffects.None, 0f);
                }
            }
        }

        /// <summary>
        /// Dessine la tilemap.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        /// <param name="atlas">Atlas du tileset.</param>
        public void DrawDecor(SpriteBatch spriteBatch, Atlas atlas, Camera camera)
        {
            var terrain = GetTiles("decor");
            int centerTileX = (int)camera.center.X / Util.tileSize;
            int centerTileY = (int)camera.center.Y / Util.tileSize;

            for (int i = centerTileY - yTileRange; i < centerTileY + yTileRange; i++)
            {
                for (int j = centerTileX - xTileRange; j < centerTileX + xTileRange; j++)
                {
                    if (0 <= i && i < height && 0 <= j && j < width && terrain[i][j] > 0)
                        spriteBatch.Draw(atlas.Texture, Util.center + new Vector2(j * Util.tileSize, i * Util.tileSize) - camera.center,
                                     atlas.GetSourceRectangle(terrain[i][j] - 1),
                                     Color.White, 0f, new Vector2(0, 0), 1f,
                                     SpriteEffects.None, 0f);
                }
            }
        }

        public void DrawObjects(SpriteBatch spriteBatch, Atlas atlas, Camera camera)
        {
            int centerTileX = (int)camera.center.X / Util.tileSize;
            int centerTileY = (int)camera.center.Y / Util.tileSize;

            foreach (MapObject mobj in objects)
            {
                // Condition sur la position de l'objet
                if (Math.Abs(mobj.position.X - camera.center.X) < xTileRange * Util.tileSize &&
                    Math.Abs(mobj.position.Y - camera.center.Y) < yTileRange * Util.tileSize)
                {
                    mobj.Draw(spriteBatch, atlas, camera);
                }
            }
        }

    }
}
