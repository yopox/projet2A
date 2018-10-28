using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using mono.core;
using mono.PhysicsEngine;
using mono.RenderEngine;
using System.Diagnostics;

namespace mono
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D texture;
        
        Player player;
        Atlas atlas;

        Tilemap map;
        Atlas tileset;

        Physics physics;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Rendering.Init(ref graphics, 982, 452);
            Rendering.setResolution(1800, 1000);
            Rendering.setVirtualResolution(100, 452);
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
            player = new Player(atlas, new Vector2(300, 100));

            physics = new Physics(new Vector2(0, 1000));
            physics.addActor(player);

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
            map = new Tilemap("Map de test", content);

            texture = Content.Load<Texture2D>("Graphics/tileset");
            // On récupère les tiles de terrain
            int[][] tiles = map.GetTiles("terrain");
            tileset.SetTexture(Content.Load<Texture2D>("Graphics/tileset"), 16, 16, 2, 2);

            atlas.SetTexture(Content.Load<Texture2D>("pacman"), 13, 13, 0, 0);
            player.AddAnimation(State.Idle, new[] { 0, 1 }, true);
            player.AddAnimation(State.Walking, new[] { 6, 7 }, true);
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

            player.Move(Keyboard.GetState());
            physics.Update(gameTime);
            player.Update(gameTime, 0.1f);
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Rendering.BeginDraw();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Rendering.getScaleMatrix());
            player.Draw(spriteBatch);
            map.Draw(spriteBatch, tileset);
            //spriteBatch.Draw(texture, Vector2.Zero);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
