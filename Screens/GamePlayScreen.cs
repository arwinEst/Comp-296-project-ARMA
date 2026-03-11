using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comp_296_project_ARMA.Screens
{
    public class GamePlayScreen : GameScreen
    {
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;

        private KeyboardState _currentState;
        private KeyboardState _previousState;

        public GamePlayScreen(SpriteFont font, SpriteBatch spriteBatch, ScreenManager screenManager)
        {
            _font = font;
            _spriteBatch = spriteBatch;
            _screenManager = screenManager;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Gameplay",
                new Vector2(100, 400), Color.White);
        }

    }
}
