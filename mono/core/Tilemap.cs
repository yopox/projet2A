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
        readonly int height;
        readonly int width;

        int xTileRange = Util.width / Util.tileSize / 2 + 1;
        int yTileRange = Util.height / Util.tileSize / 2 + 2;

        List<Layer> layers = new List<Layer>();

        public Tilemap(string name, string json)
        {
            // On parse la tilemap
            dynamic map = JObject.Parse(json);

            // Informations sur la tilemap
            height = map.height;
            width = map.width;

            // Couches
            // TODO: Supporter les object layers
            foreach (dynamic layer in map.layers)
            {
                if (layer.type == "tilelayer")
                {
                    layers.Add(new Layer(layer, width, height));
                }
            }
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
            int centerTileX = (int)camera.center.X / 16;
            int centerTileY = (int)camera.center.Y / 16;


            for (int i = centerTileY - yTileRange; i < centerTileY + yTileRange; i++)
            {
                for (int j = centerTileX - xTileRange; j < centerTileX + xTileRange; j++)
                {
                    if (0 <= i && i < height && 0 <= j && j < width && terrain[i][j] > 0)
                        spriteBatch.Draw(atlas.Texture, Util.center + new Vector2(j * 16, i * 16) - camera.center,
                                     atlas.GetSourceRectangle(terrain[i][j] - 1),
                                     Color.White, 0f, new Vector2(0, 0), 1f,
                                     SpriteEffects.None, 0f);
                }
            }
        }

    }
}
