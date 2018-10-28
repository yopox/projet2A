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

        private static int _width; //Largeur réelle de notre fenetre
        private static int _height; // Hauteur réelle de notre fenetre
        private static int _virtualWidth; //Largeur de la fenetre de dessin
        private static int _virtualHeigth; //Hauteur de notre fenetre de dessin
        private static int _realWidth;
        private static int _realHeigth;
        private static bool _dirtyMatrix = true; //Représente l'état de notre matrice de dessin
        private static Matrix _scaleMatrix; //Matrice de l'échelle du dessin


        public static void Init(ref GraphicsDeviceManager graphicsDeviceManager)
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
            _virtualHeigth = height;
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
            return (float)_virtualWidth / (float)_virtualHeigth;
        }

        /// <summary>
        /// Crée la matrice qui permet de dessiner avec la bonne échelle
        /// </summary>
        public static void RecreateScaleMatrix()
        {
            _dirtyMatrix = false;
            _scaleMatrix = Matrix.CreateScale(
                (float)_realWidth / (float)_graphicsDeviceManager.PreferredBackBufferWidth,
                (float)_realHeigth / (float)_graphicsDeviceManager.PreferredBackBufferHeight,
                1f);

            Debug.Print("Echelle x " + (float)_realWidth / (float)_graphicsDeviceManager.PreferredBackBufferWidth);
            Debug.Print("Echelle y " + (float)_realHeigth / (float)_graphicsDeviceManager.PreferredBackBufferHeight);

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

        public static void RealViewport()
        {
            float aspectRatio = getAspectRatio();
            int heigth = _graphicsDeviceManager.PreferredBackBufferHeight;
            int width = (int)(heigth * aspectRatio);

            //Debug.Print("" + width + " " + heigth + " " + aspectRatio);

            if(width > _graphicsDeviceManager.PreferredBackBufferWidth)
            {
                width = _graphicsDeviceManager.PreferredBackBufferWidth;
                heigth = (int)(width / aspectRatio);
            }

            Viewport realViewport = new Viewport();
            realViewport.X = _graphicsDeviceManager.PreferredBackBufferWidth / 2 - width / 2;
            realViewport.Y = _graphicsDeviceManager.PreferredBackBufferHeight / 2 - heigth / 2;
            realViewport.Width = width;
            realViewport.Height = heigth;

            _dirtyMatrix = true;
            _graphicsDeviceManager.GraphicsDevice.Viewport = realViewport;

            _realHeigth = heigth;
            _realWidth = width;

            Debug.Print("hauteur" + heigth);
            Debug.Print("Largeur" + width);

        }

        public static void ApplyResolution()
        {
            /*if(_width < _graphicsDeviceManager.PreferredBackBufferWidth || _height < _graphicsDeviceManager.PreferredBackBufferHeight)
            {
                _graphicsDeviceManager.PreferredBackBufferHeight = _height;
                _graphicsDeviceManager.PreferredBackBufferWidth = _width;
                _dirtyMatrix = true;
                _graphicsDeviceManager.ApplyChanges();
            }

            _width = _graphicsDeviceManager.PreferredBackBufferWidth;
            _height = _graphicsDeviceManager.PreferredBackBufferHeight;*/

            _graphicsDeviceManager.PreferredBackBufferHeight = _height;
            _graphicsDeviceManager.PreferredBackBufferWidth = _width;

            _dirtyMatrix = true;

        }

        public static void BeginDraw()
        {
            FullViewport();
            _graphicsDeviceManager.GraphicsDevice.Clear(Color.Black);
            

            RealViewport();
            _graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}
