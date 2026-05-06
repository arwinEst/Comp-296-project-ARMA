
using Comp_296_project_ARMA.Data;
using Comp_296_project_ARMA.Objects;
using Comp_296_project_ARMA.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Comp_296_project_ARMA
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private SpriteFont _font;
        private Texture2D _background;
        private DatabaseManager _databaseManager;

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");
            _background = Content.Load<Texture2D>("background");

            // Initialize database
            _databaseManager = new DatabaseManager();
            _databaseManager.Initialize();

            // Register charts in database
            string chartPath = "Content/Charts/chart1.json";
            Chart chart = ChartLoader.LoadChart(chartPath);
            _databaseManager.RegisterChart(chart, chartPath);

            _screenManager = new ScreenManager();
            _screenManager.SetScreen(new MainMenuScreen(_font, _background, _spriteBatch, _screenManager, GraphicsDevice, _databaseManager));
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _screenManager = new();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // Disable fixed time step to allow for variable frame rates
            IsFixedTimeStep = false;

            // Setup default resolution
            _graphics.PreferredBackBufferWidth = 2560;
            _graphics.PreferredBackBufferHeight = 1440;

            // Runs game in full screen
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _screenManager.Update(gameTime);
            // TODO: Add your update logic here
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _screenManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class ScreenManager
    {
        private GameScreen _currentScreen;

        public void SetScreen(GameScreen screen)
        {
            _currentScreen = screen;
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(spriteBatch);
        }
    }
 }
