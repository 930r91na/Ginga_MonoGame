using Ginga_MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.Mime;

public class DialogBox : Component
{
    private readonly string _message;
    private readonly Action _onConfirm;
    private SpriteFont _font;
    private Texture2D _pixel;
    private ContentManager _content;
    public DialogBox(string message, Action onConfirm, SpriteFont font = null, Texture2D pixel = null)
    {
        _message = message;
        _onConfirm = onConfirm;
        _font = font
                ?? _content.Load<SpriteFont>("Fonts/Font");
        _pixel = pixel;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        var dialogBoxRect = new Rectangle(100, 100, 500, 200); // Position and size of the dialog box
        var messagePosition = new Vector2(150, 150); // Position of the message
        var buttonRect = new Rectangle(300, 250, 100, 50); // Position and size of the button

        // Draw the dialog box
        spriteBatch.Draw(_pixel, dialogBoxRect, Color.White);

        // Draw the message
        spriteBatch.DrawString(_font, _message, messagePosition, Color.Black);

        // Draw the button
        spriteBatch.Draw(_pixel, buttonRect, Color.Gray);
    }

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        var buttonRect = new Rectangle(300, 250, 100, 50); // Position and size of the button

        // Check if the button is clicked
        if (mouseState.LeftButton == ButtonState.Pressed && buttonRect.Contains(mouseState.Position))
        {
            _onConfirm();
        }
    }
}

