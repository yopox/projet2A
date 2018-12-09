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

        public void PrintMap(int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                Debug.Print(string.Join(" ", tiles[i]) + "\n");
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

        int xTileRange = Util.width / Util.tileSize / 2 + 1;
        int yTileRange = Util.height / Util.tileSize / 2 + 2;

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
                    foreach(dynamic obj in layer.objects)
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

        internal Vector2 GetStartingPosition()
        {
            foreach (Warp warp in warps)
            {
                if (warp.type == "starting")
                {
                    return warp.position - new Vector2(0, 30);
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
        /// Dessine la tilemap.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch.</param>
        /// <param name="atlas">Atlas du tileset.</param>
        public void Draw(SpriteBatch spriteBatch, Atlas atlas, Camera camera)
        {
            var terrain = GetTiles("terrain");
            int centerTileX = (int)camera.center.X / 32;
            int centerTileY = (int)camera.center.Y / 32;

            for (int i = centerTileY - yTileRange; i < centerTileY + yTileRange; i++)
            {
                for (int j = centerTileX - xTileRange; j < centerTileX + xTileRange; j++)
                {
                    if (0 <= i && i < height && 0 <= j && j < width && terrain[i][j] > 0)
                        spriteBatch.Draw(atlas.Texture, Util.center + new Vector2(j * 32, i * 32) - camera.center,
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
            int centerTileX = (int)camera.center.X / 32;
            int centerTileY = (int)camera.center.Y / 32;

            for (int i = centerTileY - yTileRange; i < centerTileY + yTileRange; i++)
            {
                for (int j = centerTileX - xTileRange; j < centerTileX + xTileRange; j++)
                {
                    if (0 <= i && i < height && 0 <= j && j < width && terrain[i][j] > 0)
                        spriteBatch.Draw(atlas.Texture, Util.center + new Vector2(j * 32, i * 32) - camera.center,
                                     atlas.GetSourceRectangle(terrain[i][j] - 1),
                                     Color.White, 0f, new Vector2(0, 0), 1f,
                                     SpriteEffects.None, 0f);
                }
            }
        }

        public void DrawObjects(SpriteBatch spriteBatch, Atlas atlas, Camera camera)
        {
            int centerTileX = (int)camera.center.X / 32;
            int centerTileY = (int)camera.center.Y / 32;

            foreach (MapObject mobj in objects)
            {
                // Condition sur la position de l'objet
                if (Math.Abs(mobj.position.X - camera.center.X) < xTileRange * 32 &&
                    Math.Abs(mobj.position.Y - camera.center.Y) < yTileRange * 32)
                {
                    mobj.Draw(spriteBatch, atlas, camera);
                }
            }
        }

    }
}
