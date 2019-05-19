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
        private readonly string[] names;
        //Nombre maximal de boutons affichés sur l'écran
        private readonly int maxPrinted;
        //indice du premier bouton affiché sur l'écran
        private int firstPrinted;
        public List<Button> Buttons { get; }
        //Sélection du bouton
        private int activatedButton;

        //position de l'affichage des boutons sur l'écran
        private List<Vector2> positions;

        //Nombre total de boutons
        public readonly int NumberButton;

        public ButtonList(string[] names, int maxPrinted, Vector2 size)
        {
            this.names = names;
            this.maxPrinted = maxPrinted;
            firstPrinted = 0;

            //décalage selon Y entre chaque bouton
            int shiftY = 32;

            Buttons = new List<Button>();
            positions = new List<Vector2>();

            Vector2 position = new Vector2(Rendering.Center.X - size.X / 2, Rendering.Center.Y - this.maxPrinted * (shiftY + size.Y) / 2);

            //création de la liste de boutons, ainsi que de la liste des positions
            for (int i = 0; i < names.Length; i++)
            {
                Buttons.Add(new Button(names[i], size));
                //on n'a qu'un nombre _maxPrinted de boutons à afficher sur un même écran
                if(i < this.maxPrinted)
                    positions.Add(position);
                position.Y += size.Y + shiftY;
            }

            NumberButton = names.Length;
        }

        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            Color color;
            //dessine les boutons suivant lequel est sélectionné
            for (int i = firstPrinted; i < firstPrinted + maxPrinted;i++)
            {
                if(i != activatedButton)
                    color = new Color(89, 87, 87);
                else
                    color = new Color(178, 127, 73);

                Buttons[i].Draw(GraphicsDevice, spriteBatch, positions[Util.Mod(i - firstPrinted, maxPrinted)], color);
            }
        }

        public void Update(int activatedButton)
        {
            //actualise la valeur du bouton sélectionné
            this.activatedButton = activatedButton;
            if (this.activatedButton >= maxPrinted)
                firstPrinted = this.activatedButton - maxPrinted + 1;
            else if (this.activatedButton < firstPrinted)
                firstPrinted--;
        }
    }
}
