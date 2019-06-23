using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.core;
using mono.PhysicsEngine;
using mono.RenderEngine;
using mono.core.States;
using mono.core.Definitions;

namespace mono
{

    public struct GameState
    {
        public KeyboardState kso;
        public KeyboardState ksn;
        public GamePadState gso;
        public GamePadState gsn;
        public Tilemap map;
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
            SoundManager.SetContent(Content);

            // Affichage
            Rendering.Init(ref graphics);
            Rendering.SetResolution(Util.Width, Util.Height);
            Rendering.SetVirtualResolution(Util.VirtualWidth, Util.VirtualHeight);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //On initialise la pause
            Pause.Initialize();
            SplashScreen.Initialize();

            // Création du joueur
            player = new Player(new Vector2(64, 128));

            // Gravité
            Physics.Gravity = Util.Gravity;
            Physics.addActor(player);

            // Rendering
            Rendering.setZoom(1f);

            SoundManager.PlayBGM("3_REMINISCENCE_OBJET_done");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Cutscene.Load("text1.xml");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Util.Font = Content.Load<SpriteFont>("Fonts/MatchupPro");

            // Chargement de la map
            GameState.map = new Tilemap("Map de test", "Content/maps/tilemap.json", AtlasName.Tileset1);
            player.Position = GameState.map.GetStartingPosition();
            GameState.map.ActivateSources(player.Position);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || GameState.ksn.IsKeyDown(Keys.Escape))
                Exit();

            GameState.ksn = Keyboard.GetState();
            GameState.gsn = GamePad.GetState(PlayerIndex.One);

            SoundManager.Update();

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
                    state = Main.Update(player, gameTime, GameState);
                    break;
                case State.Textbox:
                    Main.Update(player, gameTime, GameState, true);
                    state = Textbox.Update(GameState, GraphicsDevice);
                    break;
                case State.Pause:
                    state = Pause.Update(GameState);
                    break;
                case State.Cutscene:
                    state = Cutscene.Update(GameState, gameTime);
                    break;
                case State.Exit:
                    Exit();
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
                    SplashScreen.Draw(spriteBatch, am);
                    break;
                case State.Loading:
                    break;
                case State.Title:
                    break;
                case State.Main:
                    Main.Draw(spriteBatch, am, GraphicsDevice, player, GameState.map);
                    break;
                case State.Textbox:
                    Main.Draw(spriteBatch, am, GraphicsDevice, player, GameState.map);
                    Textbox.Draw(spriteBatch, am, GraphicsDevice);
                    break;
                case State.Pause:
                    Main.Draw(spriteBatch, am, GraphicsDevice, player, GameState.map);
                    Pause.Draw(spriteBatch, am, GraphicsDevice, player, GameState.map);
                    break;
                case State.Cutscene:
                    Cutscene.Draw(spriteBatch, am, GraphicsDevice);
                    break;
            }

            if (Util.FadingOut || Util.FadingIn)
                Util.DrawFading(spriteBatch, GraphicsDevice);

            spriteBatch.End();
            //base.Draw(gameTime);
        }
    }
}
