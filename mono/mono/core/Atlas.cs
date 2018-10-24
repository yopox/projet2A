using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace mono.Core
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

        public void SetTexture(Texture2D texture, int rows, int columns)
        {
            _texture = texture;
            _width = _texture.Width / columns;
            _heigth = _texture.Height / rows;
            Columns = columns;
            Rows = rows;
        }


        public int Rows { get; set; }
        public int Columns { get; set; }
        private int totalFrames;
        private int _width;
        private int _heigth;


        public int Width { get => _width;}
        public int Heigth { get => _heigth; }

        public Atlas()
        {

        }

        public Atlas(Texture2D texture, int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            totalFrames = rows * columns;
            _texture = texture;

            _width = _texture.Width / Columns;
            _heigth = _texture.Height / Rows;
        }


    }
}


