using Microsoft.Xna.Framework;

namespace mono.core
{
    public class Camera
    {

        public Vector2 center = new Vector2(0, 0);
        public Vector2 offset = new Vector2(8, -48);

        // (2*wBox, 2*hBox) est la dimension de la boîte de mouvement libre
        static readonly int _wBox = 16;
        static readonly int _hBox = 8;

        /// <summary>
        /// Change la position de la caméra selon la position du joueur.
        /// </summary>
        /// <param name="player">Player.</param>
        public void Update(Player player)
        {

            // Mouvement sans déplacement de caméra pour une largeur 2*wBox
            if (player.position.X > center.X + _wBox - offset.X)
            {
                center.X = player.position.X - _wBox + offset.X;
            }
            else if (player.position.X < center.X - _wBox - offset.X)
            {
                center.X = player.position.X + _wBox + offset.X;
            }

            // Mouvement sans déplacement de caméra pour une hauteur 2*hBox
            if (player.position.Y > center.Y + _hBox - offset.Y)
            {
                center.Y = player.position.Y - _hBox + offset.Y;
            }
            else if (player.position.Y < center.Y - _hBox - offset.Y)
            {
                center.Y = player.position.Y + _hBox + offset.Y;
            }
        }

    }
}
