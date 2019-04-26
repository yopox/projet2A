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
        private readonly string[] _names;
        //Nombre maximal de boutons affichés sur l'écran
        private readonly int _maxPrinted;
        //indice du premier bouton affiché sur l'écran
        private int _firstPrinted;
        public List<Button> Buttons { get; }
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
            int shiftY = 32;

            Buttons = new List<Button>();
            _positions = new List<Vector2>();

            Vector2 position = new Vector2(Rendering.Center.X - size.X / 2, Rendering.Center.Y - _maxPrinted * (shiftY + size.Y) / 2);

            //création de la liste de boutons, ainsi que de la liste des positions
            for (int i = 0; i < names.Length; i++)
            {
                Buttons.Add(new Button(names[i], size));
                //on n'a qu'un nombre _maxPrinted de boutons à afficher sur un même écran
                if(i < _maxPrinted)
                    _positions.Add(position);
                position.Y += size.Y + shiftY;
            }

            numberButton = names.Length;
        }

        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            Color color;
            //dessine les boutons suivant lequel est sélectionné
            for (int i = _firstPrinted; i < _firstPrinted + _maxPrinted;i++)
            {
                if(i != _activatedButton)
                    color = new Color(89, 87, 87);
                else
                    color = new Color(178, 127, 73);

                Buttons[i].Draw(GraphicsDevice, spriteBatch, _positions[Util.Mod(i - _firstPrinted, _maxPrinted)], color);
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
