using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mono.Core
{
    public enum State
    {
        Idle,
        Jumping,
        Falling,
        Walking
    }

    class Player : Actor
    {
        public Facing facing { get; set; } = Facing.Right;
        public State state { get; set; } = State.Idle;
        public bool canJump => state == State.Idle || state == State.Walking;
        Dictionary<State, Animation> animations = new Dictionary<State, Animation>();

        public Player(Atlas atlas, Vector2 position) : base(atlas, position)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animations[state].Draw(spriteBatch, position);
        }

        public void AddAnimation(State state, int[] frames, bool isLooping)
        {
            animations.Add(state, new Animation(state, atlas, frames, isLooping));
        }

        public void Update(GameTime gameTime)
        {
            animations[state].UpdateFrame(gameTime);
        }

        public void Move(KeyboardState kbState)
        {
            if (kbState.IsKeyDown(Keys.Z))
            {
                state = State.Walking;
                position.Y--;
            }
        }
    }
}
