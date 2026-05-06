using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Comp_296_project_ARMA.Data;
using Comp_296_project_ARMA.Objects;
using Comp_296_project_ARMA.Judgements;

namespace Comp_296_project_ARMA.Screens
{
    public class ResultScreen : GameScreen
    {
        private SpriteFont _font;
        private Texture2D _background;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;
        private DatabaseManager _databaseManager;
        private ScoreEntry _latestScore;

        private bool _isFirstFrame = true;
        private KeyboardState _currentState;
        private KeyboardState _previousState;

        public ResultScreen(SpriteFont font, Texture2D background,
            SpriteBatch spriteBatch, ScreenManager screenManager,
            GraphicsDevice graphicsDevice, DatabaseManager databaseManager, int scoreId)
        {
            _font = font;
            _background = background;
            _spriteBatch = spriteBatch;
            _screenManager = screenManager;
            _graphicsDevice = graphicsDevice;
            _databaseManager = databaseManager;
            _latestScore = _databaseManager.GetScore(scoreId);
        }

        public void Update(GameTime gameTime)
        {
            if (_isFirstFrame)
            {
                _isFirstFrame = false;
                _previousState = _currentState;
                _currentState = Keyboard.GetState();
                return;
            }
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            // Check for Escape key press to return to song selection screen
            if (_currentState.IsKeyDown(Keys.Escape) && _previousState.IsKeyUp(Keys.Escape))
            {
                _screenManager.SetScreen(new SongSelectionScreen(_font, _background, _spriteBatch, _screenManager, _graphicsDevice, _databaseManager));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            if (_latestScore != null)
            {
                spriteBatch.DrawString(_font, $"Song: {_latestScore.SongName}", new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(_font, $"Score: {_latestScore.Score}", new Vector2(100, 150), Color.White);
                spriteBatch.DrawString(_font, $"Accuracy: {_latestScore.Accuracy:F2}%", new Vector2(100, 175), Color.White);
                spriteBatch.DrawString(_font, $"Grade: {_latestScore.Grade}", new Vector2(100, 200), Color.White);
                spriteBatch.DrawString(_font, $"Max Combo: {_latestScore.MaxCombo}", new Vector2(100, 225), Color.White);
                spriteBatch.DrawString(_font, $"Marvelous: {_latestScore.MarvelousCount}", new Vector2(100, 250), Color.White);
                spriteBatch.DrawString(_font, $"Perfect: {_latestScore.PerfectCount}", new Vector2(100, 275), Color.White);
                spriteBatch.DrawString(_font, $"Great: {_latestScore.GreatCount}", new Vector2(100, 300), Color.White);  
                spriteBatch.DrawString(_font, $"Good: {_latestScore.GoodCount}", new Vector2(100, 325), Color.White);
                spriteBatch.DrawString(_font, $"Bad: {_latestScore.BadCount}", new Vector2(100, 350), Color.White);
                spriteBatch.DrawString(_font, $"Miss: {_latestScore.MissCount}", new Vector2(100, 375), Color.White);


                spriteBatch.DrawString(_font, "Press ESC to return to song selection", new Vector2(100, 600), Color.Yellow);

            }
        }
    }
}


