using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using Comp_296_project_ARMA;

namespace Comp_296_project_ARMA.Objects
{
    public class Playfield
    {
        private Lanes[] _lanes;
        private int _numLanes;
        private int _laneWidth;
        private float _startX;

        public Playfield(GraphicsDevice graphicsDevice)
        {
            _startX = PlayFieldConstants.StartX;
            _laneWidth = PlayFieldConstants.LaneWidth;
            _numLanes = PlayFieldConstants.LaneCount;

            Texture2D _laneTexture = new Texture2D(graphicsDevice, 1, 1);
            _laneTexture.SetData(new[] { Color.Black });

            Texture2D _receptorTexture = new Texture2D(graphicsDevice, 1, 1);
            _receptorTexture.SetData(new[] { Color.White});

            // Center the playfield on the screen
            _startX = (2560 - (_laneWidth * _numLanes)) / 2f;
            
            _lanes = new Lanes[_numLanes];

            for (int i = 0; i < _numLanes; i++)
            {
                _lanes[i] = new Lanes(i,
                    _startX + i * _laneWidth,
                    _laneWidth,
                    graphicsDevice,
                    _laneTexture,
                    _receptorTexture);
            }
        }

        public void Update(bool[] lanePressed)
        {
            for (int i = 0; i < _numLanes; i++)
            {
                _lanes[i].Update(lanePressed[i]);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var lane in _lanes)
            {
                lane.Draw(spriteBatch);
            }
        }
    }

}
