using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.RenderEngine;
using System;
using System.Collections.Generic;

namespace mono.core
{
    class ButtonList
    {
        //Tableau des chaines de caractères associées à chaque bouton
        private string[] _names;
        //Nombre maximal de boutons affiché sur l'écran
        private int _maxPrinted;
        //indice du premier bouton affiché sur l'écran
        private int _firstPrinted;

        private List<Button> _buttonList;
        public List<Button> Buttons { get => _buttonList; }
        //Sélection du bouton
        private int _activatedButton;

        //position de l'affichage des boutons sur l'écran
        private List<Vector2> _positions;

        //Nombre total de boutons
        public readonly int numberButton;

        public ButtonList(string[] names, int maxPrinted, Vector2 size)
        {
            _names = names;
            _maxPrinted = maxPrinted;
            _firstPrinted = 0;

            //décalage selon Y entre chaque bouton
            int shiftY = (int)(Rendering.VirtualHeight - maxPrinted * size.Y) / (maxPrinted + 1);

            _buttonList = new List<Button>();
            _positions = new List<Vector2>();

            Vector2 position = new Vector2(Rendering.Center.X - size.X / 2, shiftY);

            //création de la liste de boutons, ainsi que de la liste des positions
            for (int i = 0; i < names.Length; i++)
            {
                _buttonList.Add(new Button(names[i], size));
                //on n'a qu'un nombre _maxPrinted de boutons à afficher sur un même écran
                if(i < _maxPrinted)
                    _positions.Add(position);
                position.Y += size.Y + shiftY;
            }

            numberButton = names.Length;
        }

        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch, SpriteFont font)
        {
            Color color;
            //dessine les boutons suivant lequel est sélectionné
            for (int i = _firstPrinted; i < _firstPrinted + _maxPrinted;i++)
            {
                if(i != _activatedButton)
                    color = new Color(150, 150, 50);
                else
                    color = new Color(150, 70, 50);

                _buttonList[i].Draw(GraphicsDevice, spriteBatch, font, _positions[Util.Mod(i - _firstPrinted, _maxPrinted)], color);
            }
        }

        public void Update(int activatedButton)
        {
            //actualise la valeur du bouton sélectionné
            _activatedButton = activatedButton;
            if (_activatedButton >= _maxPrinted)
                _firstPrinted = _activatedButton - _maxPrinted + 1;
            else if (_activatedButton < _firstPrinted)
                _firstPrinted--;
        }
    }
}
