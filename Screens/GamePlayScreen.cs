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
using System.IO;

namespace Comp_296_project_ARMA.Screens
{
    public class GamePlayScreen : GameScreen
    {
        private SoundEngine _soundEngine;
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;
        private Texture2D _background;
        private Texture2D _songBackground;
        private GraphicsDevice _graphicsDevice;
        private Playfield _playfield;
        private KeyboardState _currentState;
        private KeyboardState _previousState;

        private NoteRenderer _noteRenderer;
        private double _currentTime = -3000;
        private double _startDelay = 3000;
        private double _timer = 0;
        private bool _started = false;
        private Chart _chart;

        public ScoreProcessor _scoreProcessor;
        public HitWindowSet _hitWindows;
        private List<NoteObject> _activeNotes;
        public JudgementResult _lastJudgement;
        private DatabaseManager _databaseManager;

        private bool [] _lanePressed = new bool[4];
        private bool[] _holdingNote = new bool[4];
        private NoteObject[] _activeHolds = new NoteObject[4];
        private List<NoteObject> _activeHoldNotes = new List<NoteObject>();

        private double _endTimer = 0;
        private bool _songEnding = false;

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
            

            // Load song background
            string bgPath = Path.Combine("C:\\Comp-296-project-ARMA\\Content\\Charts\\", _chart.Background);
            using(var stream = new FileStream(bgPath, FileMode.Open))
            {
                _songBackground = Texture2D.FromStream(graphicsDevice, stream);
            }
            
            _activeNotes = new List<NoteObject>(_chart.Notes);
            _noteRenderer = new NoteRenderer(graphicsDevice, _activeNotes);

            // Initialize the sound engine and load the song
            _soundEngine = new SoundEngine();
            _soundEngine.LoadSong("Content/Audio/" + _chart.AudioFile + ".mp3");
            _soundEngine.SetVolume(0.2f); // Set volume to 50%

            // Initialize score processor and hit windows
            _hitWindows = new HitWindowSet(20, 40, 60, 80, 120);
            _scoreProcessor = new ScoreProcessor(_chart.Notes.Count);
            _databaseManager = databaseManager;

            // Register chart in database and get the assigned ID
            int dbId = _databaseManager.RegisterChart(_chart, "C:\\Comp-296-project-ARMA\\Content\\Charts\\chart1.json");
            _chart.ChartId = dbId; // ensure in-memory chart uses DB primary key
        }

        public void Update(GameTime gameTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            _currentTime += gameTime.ElapsedGameTime.TotalMilliseconds;

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
            
            CheckMissedNotes();

            if (_started && _activeNotes.Count == 0 && _activeHoldNotes.Count == 0
                && _soundEngine.CurrentTime > 1000)
            {
                _songEnding = true;
            }

            if (_songEnding)
            {
                _endTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_endTimer >= 3000)
                {
                    SaveAndExit();
                    return;
                }
            }

            

            // Check which lanes are pressed
            for (int lane = 0; lane < 4; lane++)
            {
                Keys key = lane switch
                {
                0 => Keys.A,
                1 => Keys.S,
                2 => Keys.K,
                3 => Keys.L,
                _ => Keys.A
                };
                bool keyDown = _currentState.IsKeyDown(key);
                bool isPressed = keyDown && !_previousState.IsKeyDown(key);
                bool isReleased = !keyDown && _previousState.IsKeyDown(key);

                if (isPressed)
                {
                    TryHit(lane);
                }

                if (keyDown && _holdingNote[lane])
                {
                    if (_currentTime >= _activeHolds[lane].HoldEndTime)
                    {
                        // Successfull completed hold note
                        _scoreProcessor.ApplyJudgement(new JudgementResult
                        {
                            Judgement = _hitWindows.GetJudgement(0),
                            Lane = lane
                        });
                        _activeHoldNotes.Remove(_activeHolds[lane]);
                        _holdingNote[lane] = false;
                        _activeHolds[lane] = null;
                    }
                }

                if (isReleased && _holdingNote[lane])
                {
                    double timeLeft = _activeHolds[lane].HoldEndTime - _currentTime;
                    if (timeLeft <= 100) // If we're within 100ms of the hold note ending, apply judgement for the hold note
                    {
                    // Successfull completed hold note
                    _scoreProcessor.ApplyJudgement(new JudgementResult
                    {
                        Judgement = _hitWindows.GetJudgement(timeLeft),
                        Lane = lane
                    });
                    }
                    else
                    {
                        // Released too early, apply judgement for the hold note
                        _scoreProcessor.ApplyJudgement(new JudgementResult
                        {
                            Judgement = Judgement.Miss,
                            Lane = lane
                            });
                    }
                    _activeHoldNotes.Remove(_activeHolds[lane]);
                    _holdingNote[lane] = false;
                    _activeHolds[lane] = null;
                }
            }

            _lanePressed[0] = _currentState.IsKeyDown(Keys.A);
            _lanePressed[1] = _currentState.IsKeyDown(Keys.S);
            _lanePressed[2] = _currentState.IsKeyDown(Keys.K);
            _lanePressed[3] = _currentState.IsKeyDown(Keys.L);

            _playfield.Update(_lanePressed);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_songBackground, 
                new Rectangle
                (0, 0, _graphicsDevice.Viewport.Width,
                _graphicsDevice.Viewport.Height), 
                Color.White);
           
            _playfield.Draw(spriteBatch);
            _noteRenderer.Draw(spriteBatch, _currentTime, _activeHoldNotes);

            // Score Display
            spriteBatch.DrawString(_font, $"Score: {_scoreProcessor.Score}", new Vector2(50, 50), Color.White);

            spriteBatch.DrawString(_font, $"Combo: {_scoreProcessor.Combo}", new Vector2(50, 90), Color.White);

            spriteBatch.DrawString(_font, $"Accuracy: {_scoreProcessor.Accuracy:F2}%", new Vector2(50, 130), Color.White);

            // Last Judgement Display
            if (_lastJudgement != null)
            {
                Color judgementColor = _lastJudgement.Judgement switch
                {
                    Judgements.Judgement.Marvelous => Color.Purple,
                    Judgements.Judgement.Perfect => Color.Green,
                    Judgements.Judgement.Great => Color.Blue,
                    Judgements.Judgement.Good => Color.Yellow,
                    Judgements.Judgement.Bad => Color.Orange,
                    Judgements.Judgement.Miss => Color.Red,
                    _ => Color.White
                };

                spriteBatch.DrawString(_font, _lastJudgement.Judgement.ToString(),
                    new Vector2(1230 , 500), judgementColor);
            }
        }

        private void SaveAndExit()
        {
            _soundEngine.Stop();
            _soundEngine.Dispose();

            // Save score to database
            int scoreId = _databaseManager.SaveScore(
                _chart.ChartId, // or another appropriate chartId value
                _chart.Title,
                _scoreProcessor.Score,
                _scoreProcessor.MaxCombo,
                _scoreProcessor.Accuracy,
                _scoreProcessor.MarvelousCount,
                _scoreProcessor.PerfectCount,   
                _scoreProcessor.GreatCount,
                _scoreProcessor.GoodCount,
                _scoreProcessor.BadCount,
                _scoreProcessor.MissCount,
                _scoreProcessor.GetGrade()
            );

            // Switch to result screen
            _screenManager.SetScreen(new ResultScreen(
                _font, _background, _spriteBatch,
                _screenManager, _graphicsDevice, _databaseManager, scoreId));
        }

        bool TryHit(int lane)
        {

            for (int i = 0; i < _activeNotes.Count; i++)
            {

                var note = _activeNotes[i];
                if (note.Lane != lane) continue;

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

                        if (note.IsHold)
                        {
                            _holdingNote[lane] = true;
                            _activeHolds[lane] = note;
                            _activeHoldNotes.Add(note); 
                            _scoreProcessor.ApplyJudgement(_lastJudgement); // Apply initial judgement for hold note
                        }
                        else
                        {
                            _scoreProcessor.ApplyJudgement(_lastJudgement);
                        }
                        _activeNotes.RemoveAt(i);
                        return true;
                    }
                
            }
            return false;
        }

        public void CheckMissedNotes()
        {
            if(!_started) return;

            for (int i = _activeNotes.Count - 1; i >= 0; i--)
            {
                var note = _activeNotes[i];
                if (_currentTime - note.HitTime > _hitWindows.Bad)
                {
                   _scoreProcessor.ApplyJudgement(new JudgementResult
                    {
                        Judgement = Judgements.Judgement.Miss,
                        Lane = note.Lane
                    });
                    _activeNotes.RemoveAt(i);
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
