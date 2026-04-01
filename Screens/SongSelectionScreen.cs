using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comp_296_project_ARMA.Screens
{
    public class SongSelectionScreen : GameScreen
    {
        private SpriteFont _font;
        private Texture2D _background;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;

        public SongSelectionScreen(SpriteFont font, Texture2D background, SpriteBatch spriteBatch, ScreenManager screenManager, GraphicsDevice graphicsDevice)
        {
            _font = font;
            _background = background;
            _spriteBatch = spriteBatch;
            _screenManager = screenManager;
            _graphicsDevice = graphicsDevice;

        }
        //Placeholder songs
        private List<String> _songs = new List<string>
        {
            "Song 1",
            "Song 2",
            "Song 3"

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
            // Handle song selection input and logic here
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            //Navigate song selection screen
            if (_currentState.IsKeyUp(Keys.Up) && !_previousState.IsKeyUp(Keys.Up))
            {
                _selectedIndex = Math.Max(0, _selectedIndex - 1);
            }
            if (_currentState.IsKeyDown(Keys.Down) && _previousState.IsKeyUp(Keys.Down))
            {
                _selectedIndex = Math.Min(_songs.Count - 1, _selectedIndex + 1);
            }
            // Select Song
            if (_currentState.IsKeyDown(Keys.Enter) && _previousState.IsKeyUp(Keys.Enter))
            {
                SelectSong();
            }
            if (_currentState.IsKeyDown(Keys.Escape) && _previousState.IsKeyUp(Keys.Escape))
            {
                GoBack();
            }
        }
        private void SelectSong()
        {
            // Logic to start the game with the selected song
            // For example, you could switch to the gameplay screen and pass the selected song
            switch (_selectedIndex)
            {
                case 0: // Song 1
                    _screenManager.SetScreen(new GamePlayScreen(_font, _background, _spriteBatch, _screenManager, _graphicsDevice));
                    break;
                case 1: // Song 2
                    break;
                case 2: // Song 3
                    break;
            }
        }

        private void GoBack()
        {
            // Logic to return to the main menu
            // For example, you could switch back to the MainMenuScreen
            _screenManager.SetScreen(new MainMenuScreen(_font, _background, _spriteBatch, _screenManager, _graphicsDevice));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the song selection UI here
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            spriteBatch.DrawString(_font, "Song Select:", new Vector2(100, 100), Color.White);

            for (int i = 0; i < _songs.Count; i++)
            {
                Color color = (i == _selectedIndex) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, _songs[i], new Vector2(300, 150 + i * 50), color);
            }
            spriteBatch.DrawString(_font, "Press Enter to select, Esc to go back",
                new Vector2(100, 400), Color.White);


        }
    }
}
