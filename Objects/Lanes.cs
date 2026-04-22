using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Comp_296_project_ARMA.Objects
{
    public class Lanes
    {
        public int LaneIndex { get; private set; }
        public float XPosition { get; private set; }
        public float Width { get; private set; }
        public Color LaneColor { get; private set; }
        public bool isPressed { get; set; }

        private GraphicsDevice _graphicsDevice;
        private Texture2D _laneTexture;
        private Texture2D _receptorTexture;

        private Rectangle _receptorRect;
        private Rectangle _lanesRect;

        public Lanes(int laneIndex, float xPosition, float width, GraphicsDevice graphicsDevice, Texture2D laneTexture, 
            Texture2D receptorTexture)
        {
            LaneIndex = laneIndex;
            XPosition = xPosition;
            Width = width;
            _laneTexture = laneTexture;
            _graphicsDevice = graphicsDevice;
            _receptorTexture = receptorTexture;

            // Set up the rectangles for drawing
            _lanesRect = new Rectangle(
                (int)XPosition, 0,
                (int)Width, 1080); // Full height of the screen

            _receptorRect = new Rectangle(
                (int)XPosition, 900,
                (int)Width, 20); // Position the receptor near the bottom
        }

    public void Update(bool isPressed)
        {
            this.isPressed = isPressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_laneTexture, _lanesRect, Color.Black);

            // Draw the lane border           
            spriteBatch.Draw(_laneTexture, 
                new Rectangle ((int)XPosition, 0, 2, 1080),
                new Color(80, 80, 80));

            // Receptor changes color when pressed
            Color receptorColor = isPressed ? Color.White : Color.Gray;
            spriteBatch.Draw(_receptorTexture, _receptorRect, receptorColor);
        }

    }
}

