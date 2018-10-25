using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        readonly int height = 0;
        readonly int width = 0;
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

    }
}
