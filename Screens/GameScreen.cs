using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Xna.Framework.GraphicsDeviceManager;

namespace Comp_296_project_ARMA.Screens
{
    public abstract class GameScreen
    {
     public abstract void Update(GameTime gameTime);
     public abstract void Draw(SpriteBatch spriteBatch);
    }

   }
