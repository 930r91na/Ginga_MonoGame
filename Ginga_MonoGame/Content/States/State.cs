using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginga_MonoGame.Content.States
{
    public abstract class State
    {
        #region Fields
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;
        protected GraphicsDeviceManager _graphics;
        
        #endregion

        #region Methods
        public Size ScreenSize => new Size(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);
        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _game = game;
            _graphics = graphics;
        }

        #endregion
    }
}
