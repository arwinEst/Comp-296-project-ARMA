using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Comp_296_project_ARMA.Data;
using Comp_296_project_ARMA.Objects;

namespace Comp_296_project_ARMA.Screens
{
    public class SongSelectionScreen : GameScreen
    {
        private SpriteFont _font;
        private Texture2D _background;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;

        private List<ChartEntry> GetAllCharts()
        {
            var charts = new List<ChartEntry>();

            using (var connection = new SqliteConnection("Data Source = arma.db"))
            {
                connection.Open();
                string query = "SELECT * From Charts ORDER BY Title";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            charts.Add(new ChartEntry
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Artist = reader.GetString(2),
                                AudioFile = reader.GetString(3),
                                BPM = reader.GetDouble(4),
                                Offset = reader.GetDouble(5),
                                FilePath = reader.GetString(6)
                            });
                        }
                    }
                }
                return charts;
            }
        }

        private List<ChartEntry> _charts;
        //private DatabaseManager _databaseManager;

        public SongSelectionScreen(SpriteFont font, Texture2D background, 
            SpriteBatch spriteBatch, ScreenManager screenManager, 
            GraphicsDevice graphicsDevice)
        {
            _font = font;
            _background = background;
            _spriteBatch = spriteBatch;
            _screenManager = screenManager;
            _graphicsDevice = graphicsDevice;

            //Load charts from database
            _charts = GetAllCharts();
        }

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
                _selectedIndex = Math.Min(_charts.Count - 1, _selectedIndex + 1);
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
            ChartEntry selected = _charts[_selectedIndex];
            Chart chart = ChartLoader.LoadChart(selected.FilePath);
            _screenManager.SetScreen(new GamePlayScreen(_font, _background, _spriteBatch,
                _screenManager, _graphicsDevice));
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

            for (int i = 0; i < _charts.Count; i++)
            {
                Color color = (i == _selectedIndex) ? Color.Yellow : Color.White;
                string display = $"{_charts[i].Title} - {_charts[i].Artist}";
                spriteBatch.DrawString(_font, display, new Vector2(100, 150 + i *50), color);
            }
            spriteBatch.DrawString(_font, "Press Enter to select, Esc to go back",
                new Vector2(100, 400), Color.White);


        }
    }
}
