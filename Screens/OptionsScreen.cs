using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comp_296_project_ARMA;
using Comp_296_project_ARMA.Data;

namespace Comp_296_project_ARMA.Screens
{
    public class OptionsScreen : GameScreen
    {
        private ScreenManager _screenManager;
        private SpriteFont _font;
        private Texture2D _background;
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;
        private DatabaseManager _databaseManager;

        public OptionsScreen(SpriteFont font, Texture2D background, SpriteBatch spriteBatch,
            ScreenManager screenManager, GraphicsDevice graphicsDevice, DatabaseManager databaseManager)
        {
            _font = font;
            _background = background;
            _spriteBatch = spriteBatch;
            _screenManager = screenManager;
            _graphicsDevice = graphicsDevice;
            _databaseManager = databaseManager;
        }

        //Settings options
        private List<string> _options = new List<string>
        {
            "Keybinds",
            "Volume",
            "Back"
        };

        private int _selectedIndex = 0;

        private KeyboardState _currentState;
        private KeyboardState _previousState;

       
        private bool _isFirstFrame = true;

        public void Update(GameTime gameTime)
        {
            if (_isFirstFrame)
            {
                _isFirstFrame = false;
                _previousState = Keyboard.GetState();
                _currentState = Keyboard.GetState();
                return;
            }
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            //Navigate options screen
            if (_currentState.IsKeyUp(Keys.Up) && !_previousState.IsKeyUp(Keys.Up))
            {
                _selectedIndex = Math.Max(0, _selectedIndex - 1);
            }
            if (_currentState.IsKeyDown(Keys.Down) && _previousState.IsKeyUp(Keys.Down))
            {
                _selectedIndex = Math.Min(_options.Count - 1, _selectedIndex + 1);
            }
            // Select Option
            if (_currentState.IsKeyDown(Keys.Enter) && _previousState.IsKeyUp(Keys.Enter))
            {
                SelectOption();
            }
            // For simplicity, pressing Escape will return to the main menu
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _screenManager.SetScreen(new MainMenuScreen(_font, _background, _spriteBatch, _screenManager, _graphicsDevice, _databaseManager));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            // Draw the options menu
            for (int i = 0; i < _options.Count; i++)
            {
                Color color = (i == _selectedIndex) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, _options[i], new Vector2(200, 100 + i * 40), color);
            }

            spriteBatch.DrawString(_font, "ESC to go back", new Vector2(100, 300), Color.White);
        }

        public void SelectOption()
        {
            switch (_selectedIndex)
            {
                case 0:
                    // Implement keybinds settings screen
           
                    break;
                case 1:
                    // Implement volume settings screen
                    break;
            }
        }
    }
}



