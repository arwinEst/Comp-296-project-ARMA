using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comp_296_project_ARMA.Objects;
using Comp_296_project_ARMA.Audio;

namespace Comp_296_project_ARMA.Screens
{
    public class GamePlayScreen : GameScreen
    {
        private SoundEngine _soundEngine;
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private Texture2D _background;
        private GraphicsDevice _graphicsDevice;
        private Playfield _playfield;
        private KeyboardState _currentState;
        private KeyboardState _previousState;

        private NoteRenderer _noteRenderer;
        private double _currentTime = 0;
        private double _startDelay = 3000;
        private bool _started = false;
        private double _timer = 0;
        private Chart _chart;

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

            _chart = ChartLoader.LoadChart("C:\\Comp-296-project-ARMA\\Content\\Charts\\chart1.json");

            _noteRenderer = new NoteRenderer(graphicsDevice, _chart.Notes);

            // Initialize the sound engine and load the song
            _soundEngine = new SoundEngine();
            _soundEngine.LoadSong("Content/Audio/" + _chart.AudioFile + ".mp3");
            _soundEngine.SetVolume(0.2f); // Set volume to 50%
        }


        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();


            //Delay the start of the song
            if (!_started)
            {
                _timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timer >= _startDelay)
                {
                    _started = true;
                    _soundEngine.Play();

                }
                return;
            }

            _currentTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check which lanes are pressed
            bool dPressed = _currentState.IsKeyDown(Keys.D);
            bool fPressed = _currentState.IsKeyDown(Keys.F);
            bool jPressed = _currentState.IsKeyDown(Keys.J);
            bool kPressed = _currentState.IsKeyDown(Keys.K);

            _lanePressed[0] = dPressed;
            _lanePressed[1] = fPressed;
            _lanePressed[2] = jPressed;
            _lanePressed[3] = kPressed;

            if (dPressed && !_previousState.IsKeyDown(Keys.D)) TryHit(0);
            if (fPressed && !_previousState.IsKeyDown(Keys.F)) TryHit(1);
            if (jPressed && !_previousState.IsKeyDown(Keys.J)) TryHit(2);
            if (kPressed && !_previousState.IsKeyDown(Keys.K)) TryHit(3);



            _playfield.Update(_lanePressed);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _playfield.Draw(spriteBatch);
            _noteRenderer.Draw(spriteBatch, _currentTime);
        }

        bool TryHit(int lane)
        {
            for (int i=0; i < _chart.Notes.Count; i++)
            {
                var note = _chart.Notes[i];
                if (note.Lane == lane)
                {
                    double timeDiff = Math.Abs(note.HitTime - _currentTime);
                    if (timeDiff < 150) // 150ms hit window
                    {
                        _chart.Notes.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Dispose()
        {
            _soundEngine.Dispose();
        }

    }
}
