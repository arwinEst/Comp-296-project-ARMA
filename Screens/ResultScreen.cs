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
    public class ResultScreen : GameScreen
    {
        private SpriteFont _font;
        private Texture2D _background;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private GraphicsDevice _graphicsDevice;
        private DatabaseManager _databaseManager;

        public ResultScreen(SpriteFont font, Texture2D background,
            SpriteBatch spriteBatch, ScreenManager screenManager,
            GraphicsDevice graphicsDevice, DatabaseManager databaseManager)
        {
            _font = font;
            _background = background;
            _spriteBatch = spriteBatch;
            _screenManager = screenManager;
            _graphicsDevice = graphicsDevice;
            _databaseManager = databaseManager;
        }

        public void Update(GameTime gameTime)
        {
            // Check for Escape key press to return to song selection screen
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _screenManager.SetScreen(new SongSelectionScreen(_font, _background, _spriteBatch, _screenManager, _graphicsDevice, _databaseManager));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White);
            // Display results (this is just a placeholder, you can customize it to show actual results)
            string resultText = "Your Score: 123456\nPress ESC to return to song selection";
            Vector2 textSize = _font.MeasureString(resultText);
            Vector2 position = new Vector2((_graphicsDevice.Viewport.Width - textSize.X) / 2, (_graphicsDevice.Viewport.Height - textSize.Y) / 2);
            _spriteBatch.DrawString(_font, resultText, position, Color.White);
            _spriteBatch.End();
        }
    }
}


