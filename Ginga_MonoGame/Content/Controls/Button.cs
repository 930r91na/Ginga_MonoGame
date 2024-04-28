using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Ginga_MonoGame.Content.Control
{
    public class Button: Component
    {
        #region Fields
        private SpriteFont _font;
        private MouseState _currentMouse;
        private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _texture;
        private Vector2 _scale;

        #endregion

        #region Properties
        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColor { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public string Text { get; internal set; }
        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _scale = new Vector2(0.3f, 0.3f);
            _font = font;
            PenColor = Color.Black;
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(_texture.Width * _scale.X, _texture.Height * _scale.Y);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (_isHovering)
                color = Color.Gray;
            spriteBatch.Draw(_texture, Position, null, color, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            //spriteBatch.Draw(_texture, Rectangle, color);
            if (!string.IsNullOrEmpty(Text))
            {
                var fontScale = new Vector2(1.5f, 1.5f);
                var textSize = _font.MeasureString(Text) * fontScale;
                var x = Position.X + (Size.X / 2) - (textSize.X / 2);
                var y = Position.Y + (Size.Y / 2) - (textSize.Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColor, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
            _isHovering = false;
            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                    Click?.Invoke(this, new EventArgs());
            }
        }

        #endregion
    }
}
