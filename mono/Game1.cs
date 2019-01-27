using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using mono.core;
using mono.PhysicsEngine;
using mono.RenderEngine;
using mono.core.States;

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
        Atlas atlas;

        GameState GameState;
        Atlas tileset;
        State state = State.SplashScreen;

        public Game1()
        {
            // TODO: Taille d'écran réelle et virtuelle dans Util
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            atlas = new Atlas();
            tileset = new Atlas();
            player = new Player(atlas, new Vector2(64, 128));

            Physics.Gravity = Util.gravity;
            Physics.addActor(player);

            Rendering.setZoom(1f);

            GameState.frameTime = 0.1f;

            
            Loading loading = new Loading();
            Title title = new Title();

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
            GameState.map = new Tilemap("Map de test", content, this);
            player.position = GameState.map.GetStartingPosition();

            // On récupère les tiles de terrain
            int[][] tiles = GameState.map.GetTiles("terrain");
            tileset.SetTexture(Content.Load<Texture2D>("Graphics/tileset"), 32, 32, 0, 0);

            atlas.SetTexture(Content.Load<Texture2D>("Graphics/hero"), 64, 128, 0, 0);
            player.AddAnimation(PlayerState.Idle, new[] { 0 }, false);
            player.AddAnimation(PlayerState.Walking, new[] { 0 }, true);
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
                default:
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
            Texture2D texture = new Texture2D(GraphicsDevice, 10, 10);
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
                    Main.Draw(spriteBatch, tileset, GraphicsDevice, player, GameState.map);
                    break;
                default:
                    break;
            }

            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
