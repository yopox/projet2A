using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core;

namespace mono.RenderEngine
{
    static class Rendering
    {
        static private GraphicsDeviceManager _graphicsDeviceManager;

        private static bool alreadyDone; // Information si les calculs de fenêtre d'affichage

        private static Texture2D backgroundTexture; // Fond bleu

        public static int WindowWidth { get; private set; } // Largeur réelle de notre fenetre
        public static int WindowHeight { get; private set; } // Hauteur réelle de notre fenetre

        public static Vector2 Center { get; private set; }
        public static Vector2 VirtualCenter { get; private set; }
        public static int VirtualWidth { get; private set; }
        public static int VirtualHeight { get; private set; }
        public static int RealWidth { get; private set; } // Largeur réelle de l'affichage le plus grand dans la fenêtre
        public static int RealHeight { get; private set; } // Hauteur réelle de l'affichage le plus grand dans la fenêtre
        private static bool dirtyMatrix = true; // Représente l'état de notre matrice de dessin
        private static Matrix scaleMatrix; // Matrice de l'échelle du dessin
        public static Vector2 ZoomOffset = Vector2.Zero;
        public static float ZoomFactor = 1f;

        static Texture2D _textureOverflow;

        /// <summary>
        /// Définit la fenêtre d'affichage
        /// </summary>
        /// <param name="graphicsDeviceManager">Fenêtre d'affichage</param>
        public static void Init(ref GraphicsDeviceManager graphicsDeviceManager)
        {
            WindowWidth = graphicsDeviceManager.PreferredBackBufferWidth;
            WindowHeight = graphicsDeviceManager.PreferredBackBufferHeight;
            Center = new Vector2(WindowWidth / 2, WindowHeight / 2);
            _graphicsDeviceManager = graphicsDeviceManager;
            dirtyMatrix = true;

            ApplyResolution();
        }

        /// <summary>
        /// Affecte la résolution dans laquelle on va dessiner notre environnement
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetVirtualResolution(int width, int height)
        {
            VirtualHeight = height;
            VirtualWidth = width;
            VirtualCenter = new Vector2(VirtualWidth / 2, VirtualHeight / 2);

            dirtyMatrix = true;
        }

        /// <summary>
        /// Affecte la taille de la fenêtre
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetResolution(int width, int height)
        {
            Rendering.WindowHeight = height;
            Rendering.WindowWidth = width;
            Center = new Vector2(width / 2, height / 2);
            ApplyResolution();
        }

        /// <summary>
        /// Donne la matrice d'échelle
        /// </summary>
        /// <returns>Matrice scalaire donnant les rapports d'échelle</returns>
        public static Matrix GetScaleMatrix()
        {
            if (dirtyMatrix)
                RecreateScaleMatrix();
            return scaleMatrix;
        }

        /// <summary>
        /// Calcule le ratio de la fenêtre de dessin
        /// </summary>
        /// <returns>Ratio de la fenêtre de dessin</returns>
        public static float GetAspectRatio()
        {
            return (float)VirtualWidth / VirtualHeight;
        }

        /// <summary>
        /// Crée la matrice qui permet de dessiner avec la bonne échelle
        /// </summary>
        public static void RecreateScaleMatrix()
        {
            dirtyMatrix = false;
            scaleMatrix = Matrix.CreateScale(
                ZoomFactor * RealWidth / VirtualWidth,
                ZoomFactor * RealHeight / VirtualHeight,
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
                Width = WindowWidth,
                Height = WindowHeight
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
                X = Rendering.WindowWidth / 2 - width / 2,
                Y = Rendering.WindowHeight / 2 - height / 2,
                Width = width,
                Height = height
            };

            dirtyMatrix = true;
            _graphicsDeviceManager.GraphicsDevice.Viewport = realViewport;

            RealHeight = height;
            RealWidth = width;
        }


        /// <summary>
        /// Applique le changement de résolution à l'affichage
        /// </summary>
        public static void ApplyResolution()
        {
            _graphicsDeviceManager.PreferredBackBufferHeight = WindowHeight;
            _graphicsDeviceManager.PreferredBackBufferWidth = WindowWidth;

            dirtyMatrix = true;

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

                backgroundTexture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, (int)(VirtualWidth / ZoomFactor), (int)(VirtualHeight / ZoomFactor));
                Color[] data = new Color[(int)(VirtualWidth / ZoomFactor) * (int)(VirtualHeight / ZoomFactor)];
                for (int i = 0; i < data.Length; ++i) data[i] = Util.BackgroundColor;
                backgroundTexture.SetData(data);

                alreadyDone = true;
            }

            _graphicsDeviceManager.GraphicsDevice.Clear(Util.ScreenBorderColor);
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
        }

        public static Texture2D GetTextureOverflow()
        {
            if (_textureOverflow == null)
            {
                if (WindowWidth == RealWidth)
                {
                    int width = Rendering.WindowWidth;
                    int height = Rendering.WindowHeight / 2 - RealHeight / 2;
                    Color[] data = new Color[width * height];
                    for (int i = 0; i < data.Length; ++i)
                        data[i] = Color.White;
                    _textureOverflow = new Texture2D(_graphicsDeviceManager.GraphicsDevice, width, height);
                    _textureOverflow.SetData(data);
                    return _textureOverflow;
                }
                else
                {
                    int height = Rendering.WindowHeight;
                    int width = Rendering.WindowWidth / 2 - RealWidth / 2;
                    Color[] data = new Color[width * height];
                    for (int i = 0; i < data.Length; ++i)
                        data[i] = Color.White;
                    _textureOverflow = new Texture2D(_graphicsDeviceManager.GraphicsDevice, width, height);
                    _textureOverflow.SetData(data);
                    return _textureOverflow;
                }
            }
            return _textureOverflow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Vector2[] GetPositionOverflow()
        {
            if (WindowWidth == RealWidth)
            {
                return new[] { new Vector2(0, 0), new Vector2(0, WindowHeight / 2 + RealHeight / 2) };
            }
            return new[] { new Vector2(0, 0), new Vector2(0, WindowWidth / 2 + RealWidth / 2) };
        }

        public static void setZoom(float zFactor)
        {
            ZoomOffset = new Vector2(VirtualCenter.X - zFactor * VirtualWidth / 2, VirtualCenter.Y - zFactor * VirtualHeight / 2) / zFactor;

            Util.ToIntVector2(ref ZoomOffset);
            ZoomFactor = zFactor;
            dirtyMatrix = true;
        }
    }
}
