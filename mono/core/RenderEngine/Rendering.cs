using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core;
using System;

namespace mono.RenderEngine
{
    static class Rendering
    {
        static private GraphicsDeviceManager _graphicsDeviceManager;

        private static bool alreadyDone = false; // Information si les calculs de fenêtre d'affichage

        private static Texture2D backgroundTexture; // Fond bleu

        static int _width; // Largeur réelle de notre fenetre
        static int _height; // Hauteur réelle de notre fenetre
        static Vector2 _center;
        static Vector2 _virtualCenter;
        public static Vector2 Center { get => _center; }
        public static Vector2 VirtualCenter { get => _virtualCenter; }
        static int _virtualWidth; // Largeur de la fenetre de dessin
        static int _virtualHeight; // Hauteur de notre fenetre de dessin
        static int _realWidth; // Largeur réelle de l'affichage le plus grand dans la fenêtre
        static int _realHeight; // Hauteur réelle de l'affichage le plus grand dans la fenêtre
        static bool _dirtyMatrix = true; // Représente l'état de notre matrice de dessin
        static Matrix _scaleMatrix; // Matrice de l'échelle du dessin
        public static Vector2 zoomOffset = Vector2.Zero;
        public static float zoomFactor = 1f;

        static Texture2D _textureOverflow;

        /// <summary>
        /// Définie la fenêtre d'affichage
        /// </summary>
        /// <param name="graphicsDeviceManager">Fenêtre d'affichage</param>
        /// <param name="width">Largeur de la fenêtre</param>
        /// <param name="heigth">Hauteur de la fenêtre</param>
        public static void Init(ref GraphicsDeviceManager graphicsDeviceManager)
        {
            _width = graphicsDeviceManager.PreferredBackBufferWidth;
            _height = graphicsDeviceManager.PreferredBackBufferHeight;
            _center = new Vector2(_width / 2, _height / 2);
            _graphicsDeviceManager = graphicsDeviceManager;
            _dirtyMatrix = true;

            ApplyResolution();
        }

        /// <summary>
        /// Affecte la résolution dans laquelle on va dessiner notre environnement
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetVirtualResolution(int width, int height)
        {
            _virtualHeight = height;
            _virtualWidth = width;
            _virtualCenter = new Vector2(_virtualWidth / 2, _virtualHeight / 2);

            _dirtyMatrix = true;
        }

        /// <summary>
        /// Affecte la taille de la fenêtre
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetResolution(int width, int height)
        {
            _height = height;
            _width = width;
            _center = new Vector2(width / 2, height / 2);
            ApplyResolution();
        }

        /// <summary>
        /// Donne la matrice d'échelle
        /// </summary>
        /// <returns>Matrice scalaire donnant les rapports d'échelle</returns>
        public static Matrix GetScaleMatrix()
        {
            if (_dirtyMatrix)
                RecreateScaleMatrix();
            return _scaleMatrix;
        }

        /// <summary>
        /// Calcule le ratio de la fenêtre de dessin
        /// </summary>
        /// <returns>Ratio de la fenêtre de dessin</returns>
        public static float GetAspectRatio()
        {
            return (float)_virtualWidth / (float)_virtualHeight;
        }

        /// <summary>
        /// Crée la matrice qui permet de dessiner avec la bonne échelle
        /// </summary>
        public static void RecreateScaleMatrix()
        {
            _dirtyMatrix = false;
            _scaleMatrix = Matrix.CreateScale(
                zoomFactor * (float)_realWidth / (float)_virtualWidth,
                zoomFactor * (float)_realHeight / (float)_virtualHeight,
                1f);
        }


        /// <summary>
        /// Définie le viewport comme la zone maximale possible
        /// </summary>
        public static void FullViewport()
        {
            Viewport viewport = new Viewport
            {
                X = 0,
                Y = 0,
                Width = _width,
                Height = _height
            };

            _graphicsDeviceManager.GraphicsDevice.Viewport = viewport;
        }


        /// <summary>
        /// Définie le viewport adapté à la résolution de l'affichage
        /// </summary>
        public static void RealViewport()
        {
            float aspectRatio = GetAspectRatio();
            int height = _graphicsDeviceManager.PreferredBackBufferHeight;
            int width = (int)(height * aspectRatio);

            if (width > _graphicsDeviceManager.PreferredBackBufferWidth)
            {
                width = _graphicsDeviceManager.PreferredBackBufferWidth;
                height = (int)(width / aspectRatio);
            }

            Viewport realViewport = new Viewport
            {
                X = _width / 2 - width / 2,
                Y = _height / 2 - height / 2,
                Width = width,
                Height = height
            };

            _dirtyMatrix = true;
            _graphicsDeviceManager.GraphicsDevice.Viewport = realViewport;

            _realHeight = height;
            _realWidth = width;
        }


        /// <summary>
        /// Applique le changement de résolution à l'affichage
        /// </summary>
        public static void ApplyResolution()
        {
            _graphicsDeviceManager.PreferredBackBufferHeight = _height;
            _graphicsDeviceManager.PreferredBackBufferWidth = _width;

            _dirtyMatrix = true;

        }

        /// <summary>
        /// Dessine dans le bon viewport
        /// </summary>
        public static void BeginDraw(SpriteBatch spriteBatch)
        {
            if (!alreadyDone)
            {
                FullViewport();
                Vector2[] position = new Vector2[2];
                position = GetPositionOverflow();

                spriteBatch.Draw(GetTextureOverflow(), position[0], Color.Black);
                spriteBatch.Draw(GetTextureOverflow(), position[1], Color.Black);
                RealViewport();

                backgroundTexture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, (int)(_virtualWidth / zoomFactor), (int)(_virtualHeight / zoomFactor));
                Color[] data = new Color[(int)(_virtualWidth / zoomFactor) * (int)(_virtualHeight / zoomFactor)];
                for (int i = 0; i < data.Length; ++i) data[i] = Util.backgroundColor;
                backgroundTexture.SetData(data);

                alreadyDone = true;
            }

            _graphicsDeviceManager.GraphicsDevice.Clear(Util.screenBorderColor);
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
        }

        public static Texture2D GetTextureOverflow()
        {
            if (_textureOverflow == null)
            {
                if (_width == _realWidth)
                {
                    int width = _width;
                    int height = _height / 2 - _realHeight / 2;
                    Color[] data = new Color[width * height];
                    for (int i = 0; i < data.Length; ++i)
                        data[i] = Color.White;
                    _textureOverflow = new Texture2D(_graphicsDeviceManager.GraphicsDevice, width, height);
                    _textureOverflow.SetData(data);
                    return _textureOverflow;
                }
                else
                {
                    int height = _height;
                    int width = _width / 2 - _realWidth / 2;
                    Color[] data = new Color[width * height];
                    for (int i = 0; i < data.Length; ++i)
                        data[i] = Color.White;
                    _textureOverflow = new Texture2D(_graphicsDeviceManager.GraphicsDevice, width, height);
                    _textureOverflow.SetData(data);
                    return _textureOverflow;
                }
            }
            else
            {
                return _textureOverflow;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Vector2[] GetPositionOverflow()
        {
            if (_width == _realWidth)
            {
                return new[] { new Vector2(0, 0), new Vector2(0, _height / 2 + _realHeight / 2) };
            }
            else
            {
                return new[] { new Vector2(0, 0), new Vector2(0, _width / 2 + _realWidth / 2) };
            }
        }

        public static void setZoom(float zFactor)
        {
            zoomOffset = new Vector2(_virtualCenter.X - zFactor * _virtualWidth / 2, _virtualCenter.Y - zFactor * _virtualHeight / 2) / zFactor;

            Util.ToIntVector2(ref zoomOffset);
            zoomFactor = zFactor;
            _dirtyMatrix = true;
        }
    }
}
