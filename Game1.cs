
using Comp_296_project_ARMA.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Comp_296_project_ARMA
{
    public class Game1 : Game
    {

        private InputManager inputManager;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private SpriteFont _font;
        private Texture2D _background;

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");
            _background = Content.Load<Texture2D>("background");

            _screenManager = new ScreenManager();
            _screenManager.SetScreen(new MainMenuScreen(_font, _background));
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            inputManager = new InputManager();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _screenManager.Update(gameTime);
            inputManager.Update();

            if (inputManager.IsActionPressed(GameAction.Pause))
            {
                // Handle pause action (e.g., toggle pause menu)
            }

            if (inputManager.IsActionPressed(GameAction.HitLane1))
            {
                // Check if a note is in range for Lane 1 and register a hit
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
    // Enum to represent game actions
    public enum GameAction
    {
        HitLane1,
        HitLane2,
        HitLane3,
        HitLane4,
        Pause
    }

    public class InputManager
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }
        public bool IsActionPressed(GameAction action)
        {
            switch (action)
            {
                case GameAction.HitLane1:
                    return IsKeyPressed(Keys.D);
                case GameAction.HitLane2:
                    return IsKeyPressed(Keys.F);
                case GameAction.HitLane3:
                    return IsKeyPressed(Keys.J);
                case GameAction.HitLane4:
                    return IsKeyPressed(Keys.K);
                case GameAction.Pause:
                    return IsKeyPressed(Keys.Escape);
                default:
                    return false;
            }
        }
        private bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }
    }

    }
