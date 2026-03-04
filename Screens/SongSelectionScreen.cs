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
        private SpriteBatch _spriteBatch;

        public SongSelectionScreen(SpriteFont font, SpriteBatch spriteBatch)
        {
            _font = font;
            _spriteBatch = spriteBatch;

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
        public override void Update(GameTime gameTime)
        {
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
            // Select option
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
        }

        private void GoBack()
        {
            // Logic to return to the main menu
            // For example, you could switch back to the MainMenuScreen
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the song selection UI here
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
