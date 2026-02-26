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
    public class MainMenuScreen : GameScreen
{
    private SpriteFont _font;
    private Texture2D _background;
    private List<String> _menuItems = new List<string>
        {
            "Play",
            "Settings",
            "Exit"
        };
    private int _selectedIndex = 0;
    private KeyboardState _currentState;
    private KeyboardState _previousState;

    public MainMenuScreen(SpriteFont font, Texture2D background)
    {
        _font = font;
        _background = background;
    }

    public override void Update(GameTime gameTime)
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();

        //Navigate menu
        if (_currentState.IsKeyUp(Keys.Up) && !_previousState.IsKeyUp(Keys.Up))
        {
            _selectedIndex = Math.Max(0, _selectedIndex - 1);
        }
        if (_currentState.IsKeyDown(Keys.Down) && _previousState.IsKeyUp(Keys.Down))
        {
            _selectedIndex = Math.Min(_menuItems.Count - 1, _selectedIndex + 1);
        }
        // Select option
        if (_currentState.IsKeyDown(Keys.Enter) && _previousState.IsKeyUp(Keys.Enter))
        {
            SelectOption();
        }
    }
    public void SelectOption()
    {
        switch (_selectedIndex)
        {
            case 0: // Play - switch to song select screen
                break;
            case 1: // Settings
                break;
            case 2: // Exit game
                System.Environment.Exit(0);
                break;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        //Draw background
        spriteBatch.Draw(_background, Vector2.Zero, Color.White);

        //Draw title items
        spriteBatch.DrawString(_font, "Arma", new Vector2(100, 50), Color.White);

        //Draw menu items
        for (int i = 0; i < _menuItems.Count; i++)
        {
            Color color = (i == _selectedIndex) ? Color.Yellow : Color.White;
            spriteBatch.DrawString(_font, _menuItems[i], new Vector2(100, 150 + i * 50), color);
        }
    }
}
}
