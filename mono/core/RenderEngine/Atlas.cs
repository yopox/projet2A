using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace mono.core
{

    public struct AtlasInfo
    {
        public string Location;
        public int Width;
        public int Height;
        public int Padding;
        public int Border;
        public int OffsetX;
        public int OffsetY;

        public AtlasInfo(string location, int width, int height, int padding, int border, int offsetY = 0, int offsetX = 0)
        {
            Location = location;
            Width = width;
            Height = height;
            Padding = padding;
            Border = border;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }
    }

    /// <summary>
    /// Représentation d'une spritesheet
    /// </summary>
    public class Atlas
    {
        public Texture2D Texture { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public int Padding { get; private set; }
        public int Border { get; private set; }

        public Atlas(Texture2D texture, int width, int height, int padding, int border)
        {
            Texture = texture;
            Width = width;
            Height = height;
            Padding = padding;
            Border = border;

            Rows = (Texture.Height - 2 * Border) / (Height + Padding);
            Columns = (Texture.Width - 2 * Border) / (Width + Padding);
        }

        /// <summary>
        /// Attribue une texture et ses caractéristiques à l'atlas
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="width"></param>
        /// <param name="heigth"></param>
        /// <param name="padding">padding entre les sprites</param>
        /// <param name="border">Offset à gauche et en haut de la texture</param>
        public void SetTexture(Texture2D texture, int width, int heigth, int padding, int border)
        {
            Texture = texture;
            Width = width;
            Height = heigth;
            Padding = padding;
            Border = border;

            Rows = (Texture.Height - 2 * Border) / (Height + Padding);
            Columns = (Texture.Width - 2 * Border) / (Width + Padding);
        }


        /// <summary>
        /// Calcule la position du rectangle correspondant à un sprite
        /// </summary>
        /// <param name="indexElement">Index d'un sprite</param>
        /// <returns></returns>
        public Rectangle GetSourceRectangle(int indexElement)
        {
            int row = (int)((float)indexElement / Columns);
            int column = indexElement % Columns;

            return new Rectangle(Border + (Width + Padding) * column, Border + (Height + Padding) * row, Width, Height);
        }

    }
}


