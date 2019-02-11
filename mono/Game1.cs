using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.core;
using mono.PhysicsEngine;
using mono.RenderEngine;
using mono.core.States;
using mono.core.Definitions;
using mono.core.PhysicsEngine;

namespace mono
{

    public struct GameState
    {
        public KeyboardState kso;
        public KeyboardState ksn;
        public GamePadState gso;
        public GamePadState gsn;
        public Tilemap map;
        public float frameTime;
        public GamePadState gamePadState;
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        GameState GameState;
        State state = State.SplashScreen;
        readonly AssetManager am;

        public Game1()
        {
            // Graphismes
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            am = new AssetManager(Content);

            // Affichage
            // TODO: Taille d'écran réelle dans Util
            Rendering.Init(ref graphics);
            Rendering.SetResolution(1280, 720);
            Rendering.SetVirtualResolution(Util.virtualWidth, Util.virtualHeight);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Création du joueur
            player = new Player(new Vector2(64, 128));

            // Gravité
            Physics.Gravity = Util.gravity;
            Physics.addActor(player);

            // Rendering
            Rendering.setZoom(1f);
            GameState.frameTime = 0.1f;

            // Création des Game States
            Loading loading = new Loading();
            Title title = new Title();

            base.Initialize();
            Polygon.test();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Chargement de la map
            GameState.map = new Tilemap("Map de test", "Content/maps/tilemap.json", AtlasName.Tileset1);
            player.position = GameState.map.GetStartingPosition();
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

            GameState.ksn = Keyboard.GetState();
            GameState.gsn = GamePad.GetState(PlayerIndex.One);

            switch (state)
            {
                case State.SplashScreen:
                    state = SplashScreen.Update(gameTime);
                    break;
                case State.Loading:
                    break;
                case State.Title:
                    break;
                case State.Main:
                    Main.Update(player, gameTime, GameState);
                    break;
            }

            base.Update(gameTime);

            GameState.kso = GameState.ksn;
            GameState.gso = GameState.gsn;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Rendering.GetScaleMatrix());

            switch (state)
            {
                case State.SplashScreen:
                    SplashScreen.Draw();
                    break;
                case State.Loading:
                    break;
                case State.Title:
                    break;
                case State.Main:
                    Main.Draw(spriteBatch, am, GraphicsDevice, player, GameState.map);
                    break;
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
