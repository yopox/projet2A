using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.RenderEngine
{
    static class Rendering
    {
        static private GraphicsDeviceManager _graphicsDeviceManager;

        static int _width; //Largeur réelle de notre fenetre
        static int _height; // Hauteur réelle de notre fenetre
        static int _virtualWidth; //Largeur de la fenetre de dessin
        static int _virtualHeight; //Hauteur de notre fenetre de dessin
        static int _realWidth; //largeur réelle de la fenetre d'affichage
        static int _realHeight; //Hauteur réelle de la fenetre d'affichage
        static bool _dirtyMatrix = true; //Représente l'état de notre matrice de dessin
        static Matrix _scaleMatrix; //Matrice de l'échelle du dessin

        static Texture2D _textureOverflow;


        public static void Init(ref GraphicsDeviceManager graphicsDeviceManager, int width, int heigth)
        {
            _width = graphicsDeviceManager.PreferredBackBufferWidth;
            _height = graphicsDeviceManager.PreferredBackBufferHeight;
            _graphicsDeviceManager = graphicsDeviceManager;
            _dirtyMatrix = true;

            ApplyResolution();

        }

        /// <summary>
        /// Set la résolution dans laquelle on va dessiner notre environnement
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void setVirtualResolution(int width, int height)
        {
            _virtualHeight = height;
            _virtualWidth = width;

            _dirtyMatrix = true;
        }

        /// <summary>
        /// Set la résolution réelle de notre affichage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void setResolution(int width, int height)
        {
            _height = height;
            _width = width;
            ApplyResolution();
        }

        /// <summary>
        /// Donne la matrice d'échelle
        /// </summary>
        /// <returns>Matrice scalaire donnant les rapports d'échelle</returns>
        public static Matrix getScaleMatrix()
        {
            if (_dirtyMatrix)
                RecreateScaleMatrix();
            return _scaleMatrix;
        }

        public static float getAspectRatio()
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
                (float)_realWidth / (float)_virtualWidth,
                (float)_realHeight / (float)_virtualHeight,
                1f);
        }


        /// <summary>
        /// Définie le viewport comme la zone maximale possible
        /// </summary>
        public static void FullViewport()
        {
            Viewport viewport = new Viewport();
            viewport.X = 0;
            viewport.Y = 0;
            viewport.Width = _width;
            viewport.Height = _height;

            _graphicsDeviceManager.GraphicsDevice.Viewport = viewport;
        }


        /// <summary>
        /// Définie le viewport adapté à la résolution de l'affichage
        /// </summary>
        public static void RealViewport()
        {
            float aspectRatio = getAspectRatio();
            int height = _graphicsDeviceManager.PreferredBackBufferHeight;
            int width = (int)(height * aspectRatio);

            if (width > _graphicsDeviceManager.PreferredBackBufferWidth)
            {
                width = _graphicsDeviceManager.PreferredBackBufferWidth;
                height = (int)(width / aspectRatio);
            }

            Viewport realViewport = new Viewport();
            realViewport.X = _width / 2 - width / 2;
            realViewport.Y = _height / 2 - height / 2;
            realViewport.Width = width;
            realViewport.Height = height;

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
            FullViewport();
            Vector2[] position = new Vector2[2];
            position = Rendering.getPositionOverflow();
            spriteBatch.Draw(Rendering.getTextureOverflow(), position[0], Color.Black);
            spriteBatch.Draw(Rendering.getTextureOverflow(), position[1], Color.Black);

            RealViewport();
            _graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        public static Texture2D getTextureOverflow()
        {
            if(_textureOverflow == null)
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

        public static Vector2[] getPositionOverflow()
        {
            if(_width == _realWidth)
            {
                return new[] { new Vector2(0, 0), new Vector2(0, _height / 2 + _realHeight / 2) };
            }
            else
            {
                return new[] { new Vector2(0, 0), new Vector2(0, _width / 2 + _realWidth / 2) };
            }
        }
        
    }
}
