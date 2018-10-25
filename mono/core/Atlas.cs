using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace mono.core
{
    public class Atlas
    {
        private Texture2D _texture;

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }
        



        private int _rows;
        private int _columns;
        private int _padding;
        private int _border;

        private int _width;
        private int _heigth;

        public int Width { get => _width;}
        public int Heigth { get => _heigth; }
        public int Rows { get => _rows; set => _rows = value; }
        public int Columns { get => _columns; set => _columns = value; }
        public int Padding { get => _padding; set => _padding = value; }
        public int Border { get => _border; set => _border = value; }

        public Atlas()
        {

        }

        public Atlas(Texture2D texture, int width, int heigth, int padding, int border)
        {
            _texture = texture;
            _width = width;
            _heigth = heigth;
            _padding = padding;
            _border = border;

            _rows = (_texture.Height - 2 * _border) / (_heigth + _padding);
            _columns = (_texture.Width - 2 * _border) / (_width + _padding);
        }

        public void SetTexture(Texture2D texture, int width, int heigth, int padding, int border)
        {
            _texture = texture;
            _width = width;
            _heigth = heigth;
            _padding = padding;
            _border = border;

            _rows = (_texture.Height - 2 * _border) / (_heigth + _padding);
            _columns = (_texture.Width - 2 * _border) / (_width + _padding);
        }

        public Rectangle GetTexture(int indexElement)
        {
            int row = (int)((float)indexElement / (float)this.Columns);
            int column = indexElement % this.Columns;

            return new Rectangle(_width * column, _heigth * row, _width, _heigth);
        }

    }
}


