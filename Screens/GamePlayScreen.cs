using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comp_296_project_ARMA.Objects;

namespace Comp_296_project_ARMA.Screens
{
    public class GamePlayScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private Texture2D _background;
        private GraphicsDevice _graphicsDevice;
        private Playfield _playfield;
        private KeyboardState _currentState;
        private KeyboardState _previousState;

        private bool [] _lanePressed = new bool[4];

        public GamePlayScreen(SpriteFont font, Texture2D background, SpriteBatch spriteBatch, ScreenManager screenManager,
            GraphicsDevice graphicsDevice)
        {
            _font = font;
            _spriteBatch = spriteBatch;
            _screenManager = screenManager;
            _background = background;
            _graphicsDevice = graphicsDevice;
            _playfield = new Playfield(graphicsDevice);
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            // Check which lanes are pressed
            _lanePressed[0] = _currentState.IsKeyDown(Keys.D);
            _lanePressed[1] = _currentState.IsKeyDown(Keys.F);
            _lanePressed[2] = _currentState.IsKeyDown(Keys.J);
            _lanePressed[3] = _currentState.IsKeyDown(Keys.K);

            _playfield.Update(_lanePressed);

            // Pause Menu
          

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _playfield.Draw(spriteBatch);
        }

    }
}
