using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using mono.core;
using mono.PhysicsEngine;
using mono.RenderEngine;

namespace mono
{

    public struct GameState
    {
        public KeyboardState kso;
        public KeyboardState ksn;
        public GameTime gameTime;
        public Tilemap map;
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Atlas atlas;

        GameState state;
        Atlas tileset;

        Physics physics;
        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Rendering.Init(ref graphics, 640, 360);
            Rendering.setResolution(640, 360);
            Rendering.setVirtualResolution(640, 360);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            atlas = new Atlas();
            tileset = new Atlas();
            var initialPos = new Vector2(4 * 16, 10 * 16 + 2);
            player = new Player(atlas, initialPos);

            physics = new Physics(new Vector2(0, 0));
            physics.addActor(player);

            camera = new Camera();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Utile pour dessiner des textures
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Chargement de la map
            // TODO: Déplacer le chargement des maps dans [core.Tilemap]
            StreamReader stream = File.OpenText("Content/maps/tilemap.json");
            string content = stream.ReadToEnd();
            stream.Close();
            state.map = new Tilemap("Map de test", content);
            player.position = state.map.GetStartingPosition();

            // On récupère les tiles de terrain
            int[][] tiles = state.map.GetTiles("terrain");
            tileset.SetTexture(Content.Load<Texture2D>("Graphics/tileset"), 32, 32, 0, 0);

            atlas.SetTexture(Content.Load<Texture2D>("Graphics/mario"), 16, 30, 0, 0);
            player.AddAnimation(State.Idle, new[] { 0 }, false);
            player.AddAnimation(State.Walking, new[] { 0, 1, 2 }, true);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            state.ksn = Keyboard.GetState();

            player.Move(Keyboard.GetState());
            player.Update(gameTime, 0.1f);
            camera.Update(player);
            physics.Update(gameTime);
            base.Update(gameTime);

            state.kso = state.ksn;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 10, 10);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Rendering.getScaleMatrix());
            Rendering.BeginDraw(spriteBatch);
            state.map.DrawDecor(spriteBatch, tileset, camera);
            player.Draw(spriteBatch, camera);
            state.map.Draw(spriteBatch, tileset, camera);
            state.map.DrawObjects(spriteBatch, tileset, camera);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
