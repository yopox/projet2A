using Microsoft.Xna.Framework;

namespace mono.core
{
    public class Camera
    {

        public Vector2 _center = new Vector2(0, 0);

        /// <summary>
        /// Change la position de la caméra selon la position du joueur.
        /// </summary>
        /// <param name="player">Player.</param>
        public void Update(Player player)
        {
            _center.X = player.position.X;
            _center.Y = player.position.Y;
        }

    }
}
