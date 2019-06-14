using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.Definitions;
using mono.core.Entities;
using mono.core.RenderEngine;
using mono.RenderEngine;
using Newtonsoft.Json.Linq;

namespace mono.core
{

    /// <summary>
    /// Représente une couche de la tilemap.
    /// </summary>
    public class Layer
    {
        public readonly string Name;
        public int[][] Tiles;

        public Layer(dynamic layer, int width, int height)
        {
            Name = layer.name;

            // On construit le tableau de tiles
            int[] tiles1 = layer.data.ToObject(typeof(int[]));
            Tiles = new int[height][];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Tiles[i] = tiles1.Skip(i * width).Take(width).ToArray();
                }
            }
        }
    }

    /// <summary>
    /// Permet de charger une tilemap au format JSON.
    /// </summary>
    public class Tilemap
    {

        readonly string regex = "(.*);(.*)";
        public AtlasName TilesetName;

        public static int[] WarpGids = { 1 };
        public static int[] SourceGids = { 13 };
        public static int[] TextGids = { 14 };

        readonly int height;
        public readonly int width;
        public List<ParallaxElement> ParallaxElements = new List<ParallaxElement>();

        readonly int xTileRange = Util.Width / Util.TileSize / 2 + 1;
        readonly int yTileRange = Util.Height / Util.TileSize / 2 + 2;

        List<Layer> layers = new List<Layer>();
        List<Interaction> interactives = new List<Interaction>();
        List<Warp> warps = new List<Warp>();
        List<Source> sources = new List<Source>();

        public Tilemap(string name, string path, AtlasName tilesetName)
        {
            this.TilesetName = tilesetName;

            // On récupère le JSON
            StreamReader stream = File.OpenText(path);
            string json = stream.ReadToEnd();
            stream.Close();

            // On parse la tilemap
            dynamic map = JObject.Parse(json);

            // Informations sur la tilemap
            height = map.height;
            width = map.width;

            foreach (var element in map.properties)
            {
                var matches = Regex.Split((string)element.value, regex);

                ParallaxElement p = new ParallaxElement
                {
                    Factor = (float)Convert.ToDouble(matches[1]),
                    Name = Util.ParseEnum<AtlasName>(matches[2])
                };

                ParallaxElements.Add(p);
            }

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
                        if (WarpGids.Contains(id))
                        {
                            string type = obj.type;
                            warps.Add(new Warp(id, new Vector2(x, y), type));
                        }
                        else if (SourceGids.Contains(id))
                        {
                            string sourceId = (string) obj.properties[0].value;
                            int sourceRadius = (int) obj.properties[1].value;
                            int sourceVolume = (int) obj.properties[2].value;
                            sources.Add(new Source(sourceId, sourceRadius, sourceVolume, x + 16, y - 16));
                        }
                        else if (TextGids.Contains(id))
                        {
                            interactives.Add(new Interaction(id, new Vector2(x, y), (string)obj.properties[0].value));
                        }
                    }
                }
            }
        }

        internal void ActivateSources(Vector2 position)
        {
            foreach (Source source in sources)
            {
                source.Activate(position);
            }
        }

        internal void UpdateSources(Vector2 position)
        {
            foreach (Source source in sources)
            {
                source.SetVolume(position);
            }
        }

        /// <summary>
        /// Renvoie la position de départ du joueur.
        /// </summary>
        internal Vector2 GetStartingPosition()
        {
            foreach (Warp warp in warps)
            {
                if (warp.Type == "starting")
                {
                    return warp.Position - new Vector2(0, Util.PlayerHeight);
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
                if (layer.Name == layerName)
                {
                    return layer.Tiles;
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
        public int[][] GetTerrain(Vector2 position, int radius)
        {
            int diameter = 2 * radius + 1;
            int[][] tiles = new int[diameter][];
            int[][] terrain = GetTiles(Util.SolidLayerName);

            int x = (int)Math.Floor(position.X / Util.TileSize);
            int y = (int)Math.Floor(position.Y / Util.TileSize);

            for (int i = 0; i < diameter; i++)
            {
                int[] subTiles = new int[diameter];

                for (int j = 0; j < diameter; j++)
                {
                    var x2 = x + j - radius;
                    var y2 = y + i - radius;

                    if (x2 >= 0 && y2 >= 0 && x2 < width && y2 < height)
                    {
                        subTiles[j] = terrain[y2][x2];
                    }
                    else
                    {
                        subTiles[j] = -1;
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
        /// <param name="am">AssetManager.</param>
        public void DrawLayer(SpriteBatch spriteBatch, AssetManager am, string name)
        {
            var terrain = GetTiles(name);
            int centerTileX = (int)Camera.Center.X / Util.TileSize;
            int centerTileY = (int)Camera.Center.Y / Util.TileSize;
            var atlas = am.GetAtlas(TilesetName);

            for (int i = centerTileY - (int)Math.Ceiling(yTileRange / Rendering.ZoomFactor); i < centerTileY + (int)Math.Ceiling(yTileRange / Rendering.ZoomFactor); i++)
            {
                for (int j = centerTileX - (int)Math.Ceiling(xTileRange / Rendering.ZoomFactor); j < centerTileX + Math.Ceiling(xTileRange / Rendering.ZoomFactor) + 1; j++)
                {
                    if (0 <= i && i < height && 0 <= j && j < width && terrain[i][j] > 0)
                        spriteBatch.Draw(atlas.Texture, Util.Center + new Vector2(j * Util.TileSize, i * Util.TileSize) - Camera.Center + Rendering.ZoomOffset,
                                     atlas.GetSourceRectangle(terrain[i][j] - 1),
                                     Color.White, 0f, new Vector2(0, 0), 1f,
                                     SpriteEffects.None, 0f);
                }
            }
        }

        public void DrawParallax(SpriteBatch spriteBatch, AssetManager am)
        {
            foreach (var parallaxElement in ParallaxElements)
            {
                BackgroundImage.Draw(spriteBatch, parallaxElement, am);
            }
        }

        internal List<Tuple<string, string>> InteractionText(Player player)
        {
            foreach (Interaction interactive in interactives)
            {
                if (Vector2.Distance(player.Position, interactive.Position) < Interaction.RADIUS)
                {
                    return interactive.text;
                }
            }
            return null;
        }
    }
}
