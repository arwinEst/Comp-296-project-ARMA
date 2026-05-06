using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Comp_296_project_ARMA.Objects;

namespace Comp_296_project_ARMA.Objects
{
    public class NoteRenderer
    {
        private Texture2D _noteTexture;
        private List<NoteObject> _notes;
        private float _scrollSpeed = 2.2f; // Adjust as needed

        public NoteRenderer(GraphicsDevice graphicsDevice, List<NoteObject> notes)
        {
            _notes = notes;
            _noteTexture = new Texture2D(graphicsDevice, 1, 1);
            _noteTexture.SetData(new[] { Color.White }); // Placeholder white texture 
        }

        public void Draw(SpriteBatch spriteBatch, double _currentTime, List<NoteObject> activeHoldNotes)
        {

            // Draw hold bodies
            foreach (var note in activeHoldNotes)
            {
                double timeDiff = note.HitTime - _currentTime;

                float _noteYPosition = PlayFieldConstants.ReceptorY;

                float _noteXPosition = PlayFieldConstants.StartX + note.Lane * PlayFieldConstants.LaneWidth;

                Color _noteColor = note.Lane switch
                {
                    0 => Color.Red,
                    1 => Color.CornflowerBlue,
                    2 => Color.CornflowerBlue,
                    3 => Color.Red,
                    _ => Color.White
                };

                DrawHoldNote(spriteBatch, note, _noteXPosition, _noteYPosition, _noteColor, _currentTime);
            }

            foreach (var note in _notes)
            {
                double timeDiff = note.HitTime - _currentTime;

                if (timeDiff > 3000 || timeDiff < -500) continue;

                float _noteYPosition = PlayFieldConstants.ReceptorY - (float)(timeDiff * _scrollSpeed);

                float _noteXPosition = PlayFieldConstants.StartX + note.Lane * PlayFieldConstants.LaneWidth;

                Color _noteColor = note.Lane switch
                {
                    0 => Color.Red,
                    1 => Color.CornflowerBlue,
                    2 => Color.CornflowerBlue,
                    3 => Color.Red,
                    _ => Color.White
                };

                if (note.IsHold)
                {
                    // Draw unstarted hold notes normally
                    DrawHoldNote(spriteBatch, note, _noteXPosition, _noteYPosition, _noteColor, _currentTime);
                }
                else
                {
                    // Draw regular notes
                    Rectangle noteRect = new Rectangle(
                        (int)_noteXPosition + 2,
                        (int)_noteYPosition,
                        PlayFieldConstants.LaneWidth - 4,
                        PlayFieldConstants.NoteHeight
                    );
                    spriteBatch.Draw(_noteTexture, noteRect, _noteColor);
                }
                
            }


        }

    

        public void DrawHoldNote(SpriteBatch spriteBatch, NoteObject note,
            float _noteXPosition, float _noteYPosition, Color noteColor, double currentTime)
        {
            double _tailTimeDiff = note.HoldEndTime - currentTime;
            float _tailY = PlayFieldConstants.ReceptorY - (float)(_tailTimeDiff * _scrollSpeed);

            _tailY = Math.Min(_tailY, PlayFieldConstants.ReceptorY);

            // Draw body
            Rectangle holdRect = new Rectangle(
                    (int)_noteXPosition + 10,
                    (int)_tailY,
                    PlayFieldConstants.LaneWidth - 20,
                    (int)(_noteYPosition - _tailY)
            );
            spriteBatch.Draw(_noteTexture, holdRect, noteColor
             * 0.5f); // Semi-transparent body
            
            // Draw head
            Rectangle headRect = new Rectangle(
                    (int)_noteXPosition + 2,
                    (int)_noteYPosition,
                    PlayFieldConstants.LaneWidth - 4,
                    PlayFieldConstants.NoteHeight
            );
            spriteBatch.Draw(_noteTexture, headRect, noteColor);

            // Draw tail
            Rectangle tailRect = new Rectangle(
                    (int)_noteXPosition + 2,
                    (int)_tailY,
                    PlayFieldConstants.LaneWidth - 4,
                    PlayFieldConstants.NoteHeight
            );
            spriteBatch.Draw(_noteTexture, tailRect, noteColor);
        }


    }

}
