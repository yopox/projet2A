using Microsoft.Xna.Framework;
using mono.RenderEngine;

namespace mono.core
{
    public static class Camera
    {

        public static Vector2 center = new Vector2(0, 0);
        public static Vector2 offset = new Vector2((float)-Util.playerWidth / 2, Util.playerHeight / 2);

        // (2*wBox, 2*hBox) est la dimension de la boîte de mouvement libre
        static readonly int _wBox = 64;
        static readonly int _hBox = 32;

        /// <summary>
        /// Change la position de la caméra selon la position du joueur.
        /// </summary>
        /// <param name="player">Player.</param>
        public static void Update(Player player, int worldWidth)
        {

            // Mouvement sans déplacement de caméra pour une largeur 2*wBox
            if (player.position.X > center.X + _wBox + offset.X)
            {
                center.X = player.position.X - _wBox - offset.X;
            }
            else if (player.position.X < center.X - _wBox + offset.X)
            {
                center.X = player.position.X + _wBox - offset.X;
            }

            if (player.position.X - offset.X < Util.virtualWidth / 2)
            {
                center.X = Util.virtualWidth / 2;
            }
            else if (player.position.X + offset.X > worldWidth - Util.virtualWidth / 2)
            {
                center.X = worldWidth - Util.virtualWidth / 2;
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

        public static Vector2 GetScreenPosition(Vector2 absPos)
        {
            return Util.center + (absPos - center);
        }

    }
}
