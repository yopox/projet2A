using Microsoft.Xna.Framework;

namespace mono.core
{
    public static class Camera
    {

        public static Vector2 Center = new Vector2(0, 0);
        public static Vector2 Offset = new Vector2((float)-Util.PlayerWidth / 2, Util.PlayerHeight / 2);

        // (2*wBox, 2*hBox) est la dimension de la boîte de mouvement libre
        static readonly int wBox = 64;
        static readonly int hBox = 32;

        /// <summary>
        /// Change la position de la caméra selon la position du joueur.
        /// </summary>
        /// <param name="player">Player.</param>
        public static void Update(Player player, int worldWidth)
        {

            // Mouvement sans déplacement de caméra pour une largeur 2*wBox
            if (player.Position.X > Center.X + wBox + Offset.X)
            {
                Center.X = player.Position.X - wBox - Offset.X;
            }
            else if (player.Position.X < Center.X - wBox + Offset.X)
            {
                Center.X = player.Position.X + wBox - Offset.X;
            }

            if (player.Position.X - Offset.X +wBox < Util.VirtualWidth / 2)
            {
                Center.X = Util.VirtualWidth / 2;
            }
            else if (player.Position.X + Offset.X > worldWidth - Util.VirtualWidth / 2)
            {
                Center.X = worldWidth - Util.VirtualWidth / 2;
            }

            // Mouvement sans déplacement de caméra pour une hauteur 2*hBox
            if (player.Position.Y > Center.Y + hBox - Offset.Y)
            {
                Center.Y = player.Position.Y - hBox + Offset.Y;
            }
            else if (player.Position.Y < Center.Y - hBox - Offset.Y)
            {
                Center.Y = player.Position.Y + hBox + Offset.Y;
            }
        }

        public static Vector2 GetScreenPosition(Vector2 absPos)
        {
            return Util.Center + (absPos - Center);
        }

    }
}
