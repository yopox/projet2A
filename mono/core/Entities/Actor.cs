using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.Definitions;
using mono.core.PhysicsEngine;

namespace mono.core

{
    /// <summary>
    /// Classe parent de tous les acteurs : monstres, joueur.
    /// </summary>
    public class Actor
    {
        public AtlasName AtlasName; // Nom du spritesheet de l'acteur
        public PlayerState State { get; set; } = PlayerState.Idle;

        internal Dictionary<PlayerState, Animation> animations { get; } = new Dictionary<PlayerState, Animation>();

        public Face Facing = Face.Right; // Direction à laquelle l'acteur fait face
        public Vector2 Size;
        public Vector2 Position;
        public Vector2 Center;
        public Vector2 Speed = new Vector2(0, 0);
        public Vector2 Acceleration = new Vector2(0, 0);
        public Vector2 Forces = new Vector2(0, 0);
        public bool DebugMode;

        public Actor(AtlasName atlasName, Vector2 position, Vector2 size)
        {
            AtlasName = atlasName;
            Position = position;
            Size = size;
            Center = new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
            Util.ToIntVector2(ref Center);
        }

        /// <summary>
        /// Update la frame de l'animation
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameState gstate, GameTime gameTime)
        {
            List<Polygon> listPolygon = CollisionTester.CollidesWithTerrain(GetHitbox(), gstate.map);

            while (listPolygon.Count != 0)
            {
                CollisionSolver.ActorTerrain(this, listPolygon[0], gameTime);
                listPolygon = CollisionTester.CollidesWithTerrain(GetHitbox(), gstate.map);
            }

            animations[State].UpdateFrame();
        }

        /// <summary>
        /// Rajoute une animation au player
        /// </summary>
        /// <param name="state">Etat de l'animation</param>
        /// <param name="frames">Nombre de frames de l'animation</param>
        /// <param name="isLooping">condition de répétition de l'animation</param>
        public void AddAnimation(PlayerState state, int[] frames,int duration, bool isLooping)
        {
            animations.Add(state, new Animation(state, frames, duration, isLooping));
        }

        /// <summary>
        /// Dessine un actor
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, AssetManager am)
        {
            var displayPos = Camera.GetScreenPosition(Position);
            animations[State].Draw(spriteBatch, am.GetAtlas(AtlasName), displayPos, Facing);
        }

        public Rect GetHitbox()
        {
            return new Rect((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        public void SetX(int positionX, int speedX, int accelerationX)
        {
            Position.X = positionX;
            Speed.X = speedX;
            Acceleration.X = accelerationX;
        }

        public void SetY(int positionY, float speedY, float accelerationY)
        {
            Position.Y = positionY;
            Speed.Y = speedY;
            Acceleration.Y = accelerationY;
        }
    }
}
