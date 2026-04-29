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
using Comp_296_project_ARMA.Judgements;
using Comp_296_project_ARMA.Data;

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

        public ScoreProcessor _scoreProcessor;
        public HitWindowSet _hitWindows;
        private List<NoteObject> _activeNotes;
        public JudgementResult _lastJudgement;
        private DatabaseManager _databaseManager;

        private bool [] _lanePressed = new bool[4];

        public GamePlayScreen(SpriteFont font, Texture2D background, SpriteBatch spriteBatch, ScreenManager screenManager,
            GraphicsDevice graphicsDevice, DatabaseManager databaseManager)
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

            // Initialize score processor and hit windows
            _hitWindows = new HitWindowSet(20, 40, 60, 80, 120);
            _scoreProcessor = new ScoreProcessor(_chart.Notes.Count);
            _activeNotes = new List<NoteObject>(_chart.Notes);
            _databaseManager = databaseManager;
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

            if (_soundEngine.CurrentTime >= _soundEngine.TotalTime || _activeNotes.Count == 0)
            {
                SaveAndExit();
                return;
            }

            CheckMissedNotes();

            _currentTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check which lanes are pressed
            bool aPressed = _currentState.IsKeyDown(Keys.A);
            bool sPressed = _currentState.IsKeyDown(Keys.S);
            bool lPressed = _currentState.IsKeyDown(Keys.L);
            bool oemSemicolonPressed = _currentState.IsKeyDown(Keys.OemSemicolon);

            _lanePressed[0] = aPressed;
            _lanePressed[1] = sPressed;
            _lanePressed[2] = lPressed;
            _lanePressed[3] = oemSemicolonPressed;

            if (aPressed && !_previousState.IsKeyDown(Keys.A)) TryHit(0);
            if (sPressed && !_previousState.IsKeyDown(Keys.S)) TryHit(1);
            if (lPressed && !_previousState.IsKeyDown(Keys.L)) TryHit(2);
            if (oemSemicolonPressed && !_previousState.IsKeyDown(Keys.OemSemicolon)) TryHit(3);



            _playfield.Update(_lanePressed);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _playfield.Draw(spriteBatch);
            _noteRenderer.Draw(spriteBatch, _currentTime);

            // Score Display
            spriteBatch.DrawString(_font, $"Score: {_scoreProcessor.Score}", new Vector2(50, 50), Color.White);

            spriteBatch.DrawString(_font, $"Combo: {_scoreProcessor.Combo}", new Vector2(50, 90), Color.White);

            spriteBatch.DrawString(_font, $"Accuracy: {_scoreProcessor.Accuracy:F2}%", new Vector2(50, 130), Color.White);

            // Last Judgement Display
            if (_lastJudgement != null)
            {
                Color judgementColor = _lastJudgement.Judgement switch
                {
                    Judgements.Judgement.Marvelous => Color.Magenta,
                    Judgements.Judgement.Perfect => Color.Green,
                    Judgements.Judgement.Great => Color.Blue,
                    Judgements.Judgement.Good => Color.Yellow,
                    Judgements.Judgement.Bad => Color.Orange,
                    Judgements.Judgement.Miss => Color.Red,
                    _ => Color.White
                };

                spriteBatch.DrawString(_font, _lastJudgement.Judgement.ToString(),
                    new Vector2(800, 500), judgementColor);
            }
        }

        private void SaveAndExit()
        {
            

            // Save score to database
            _databaseManager.SaveScore(
                _chart.Title,
                _scoreProcessor.Score,
                _scoreProcessor.Combo,
                _scoreProcessor.Accuracy,
                _scoreProcessor.MarvelousCount,
                _scoreProcessor.PerfectCount,   
                _scoreProcessor.GreatCount,
                _scoreProcessor.GoodCount,
                _scoreProcessor.BadCount,
                _scoreProcessor.MissCount,
                _scoreProcessor.GetGrade()
            );

            _screenManager.SetScreen(new ResultScreen(_font, _background, _spriteBatch, _screenManager, _graphicsDevice, _databaseManager));
        }

        bool TryHit(int lane)
        {

            for (int i = 0; i < _chart.Notes.Count; i++)
            {

                var note = _chart.Notes[i];
                if (note.Lane == lane)
                {
                    double timeDiff = note.HitTime - _currentTime;
                    if (Math.Abs(timeDiff) <= _hitWindows.Bad) // 150ms hit window
                    {

                        // Determine judgement
                        Judgement judgement = _hitWindows.GetJudgement(timeDiff);
                        _lastJudgement = new JudgementResult
                        {
                            Judgement = judgement,
                            HitDifference = timeDiff,
                            Lane = lane
                        };

                        _scoreProcessor.ApplyJudgement(_lastJudgement);
                        _chart.Notes.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public void CheckMissedNotes()
        {
            for (int i = _chart.Notes.Count - 1; i >= 0; i--)
            {
                if (_currentTime - _chart.Notes[i].HitTime > _hitWindows.Bad) // If note is past the bad window
                {
                   _scoreProcessor.ApplyJudgement(new JudgementResult
                    {
                        Judgement = Judgement.Miss,
                        Lane = _chart.Notes[i].Lane
                    });
                    _chart.Notes.RemoveAt(i);
                }

            }
        }

        public void Dispose()
        {
            _soundEngine.Dispose();
        }

        // Switch to result Screem

    }
}
