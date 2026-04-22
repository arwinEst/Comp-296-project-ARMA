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
        private float _scrollSpeed = 1.5f; // Adjust as needed

        public NoteRenderer(GraphicsDevice graphicsDevice, List<NoteObject> notes)
        {
            _noteTexture = new Texture2D(graphicsDevice, 1, 1);
            _noteTexture.SetData(new[] { Color.White }); // Placeholder white texture

            _notes = notes;
            
        }

        public void Draw(SpriteBatch spriteBatch, double _currentTime)
        {
            foreach (var note in _notes)
            {
                double timeDiff = note.HitTime - _currentTime;

                if (timeDiff > 3000 || timeDiff < -500) continue;

                float _noteYPosition = PlayFieldConstants.ReceptorY - (float)(timeDiff * _scrollSpeed);

                float _noteXPosition = PlayFieldConstants.StartX + note.Lane * PlayFieldConstants.LaneWidth;

                Rectangle noteRect = new Rectangle(
                    (int)_noteXPosition + 2,
                    (int)_noteYPosition,
                    PlayFieldConstants.LaneWidth - 4,
                    PlayFieldConstants.NoteHeight
            );
                spriteBatch.Draw(_noteTexture, noteRect, Color.White);
            }

        }

       
    }

}
