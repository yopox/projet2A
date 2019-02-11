using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.Definitions;
using mono.core.PhysicsEngine;

namespace mono.core

{
    /// <summary>
    /// Classe parent de tous les acteurs : monstres, joueur.
    /// 
    /// 
    /// </summary>
    public class Actor
    {
        public AtlasName atlasName; // Nom du spritesheet de l'acteur
        public PlayerState State { get; set; } = PlayerState.Idle;

        internal Dictionary<PlayerState, Animation> Animations { get; } = new Dictionary<PlayerState, Animation>();

        public Face facing = Face.Right; // Direction à laquelle l'acteur fait face
        public Vector2 size;
        public Vector2 position;
        public Vector2 center;
        public Vector2 speed = new Vector2(0, 0);
        public Vector2 acceleration = new Vector2(0, 0);
        public Vector2 forces = new Vector2(0, 0);
        public bool DebugMode;

        public Actor(AtlasName atlasName, Vector2 position, Vector2 size)
        {
            this.atlasName = atlasName;
            this.position = position;
            this.size = size;
            center = new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
            Util.ToIntVector2(ref center);
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
                CollisionSolver.ActorTerrain(this, listPolygon, gameTime);
                listPolygon = CollisionTester.CollidesWithTerrain(GetHitbox(), gstate.map);
            }

            Animations[State].UpdateFrame(gameTime, gstate.frameTime);
        }

        /// <summary>
        /// Rajoute une animation au player
        /// </summary>
        /// <param name="state">Etat de l'animation</param>
        /// <param name="frames">Nombre de frames de l'animation</param>
        /// <param name="isLooping">condition de répétition de l'animation</param>
        public void AddAnimation(PlayerState state, int[] frames, bool isLooping)
        {
            // TODO: Durée d'une frame de l'animation
            Animations.Add(state, new Animation(state, frames, isLooping));
        }

        /// <summary>
        /// Dessine un actor
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, AssetManager am)
        {
            var displayPos = Camera.GetScreenPosition(position);
            Animations[State].Draw(spriteBatch, am.GetAtlas(atlasName), displayPos, facing);
        }

        public Rect GetHitbox()
        {
            return new Rect((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public void SetX(int positionX, int speedX, int accelerationX)
        {
            position.X = positionX;
            speed.X = speedX;
            acceleration.X = accelerationX;
        }

        public void SetY(int positionY, float speedY, float accelerationY)
        {
            position.Y = positionY;
            speed.Y = speedY;
            acceleration.Y = accelerationY;
        }
    }
}
