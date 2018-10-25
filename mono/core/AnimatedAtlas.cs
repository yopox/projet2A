using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace AnimatedAtlas
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            totalFrames = rows * columns;
            Texture = texture;

            currentFrame = 0;
        }

        public void update()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int heigth = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectange = new Rectangle(width * column, heigth * row, width, heigth);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, heigth);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectange, Color.White);
            spriteBatch.End();

        }
    }
}


