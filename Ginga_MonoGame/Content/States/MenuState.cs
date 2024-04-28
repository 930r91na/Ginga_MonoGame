using Ginga_MonoGame.Content.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginga_MonoGame.Content.States
{
    public class MenuState : State
    { 
        private List<Component> _components;
        private Texture2D _backgroundTexture;
        private Texture2D _logoTexture;
        int screenWidth = 1003;
        int screenHeight = 571;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics) : base(game, graphicsDevice, content, graphics)
        {
            _logoTexture = _content.Load<Texture2D>("Resources/Logo");
            _backgroundTexture = _content.Load<Texture2D>("Resources/Background");
            var buttonTexture = _content.Load<Texture2D>("Controls/button_green");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            // Set the screen size
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();

            var gameMode1Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(screenWidth/2, 350),
                Text = "Game Mode 1",
            };
            gameMode1Button.Position = new Vector2(screenWidth / 2 - gameMode1Button.Size.X/2 , 325);

            gameMode1Button.Click += GameMode1Button_Click;

            var gameMode2Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(screenWidth/2, 450),
                Text = "Game Mode 2",
            };
            gameMode2Button.Position = new Vector2(screenWidth / 2 - gameMode2Button.Size.X / 2, 425);

            gameMode2Button.Click += GameMode2Button_Click;

            var exitGame = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(screenWidth / 2, 200),
                Text = "Exit",
            };
            exitGame.Position = new Vector2(screenWidth / 2 - exitGame.Size.X / 2, 500);

            exitGame.Click += (sender, args) => game.Exit();

            _components = new List<Component>()
            { 
                gameMode1Button,
                gameMode2Button,
                //exitGame,
            };
        }

        private void GameMode1Button_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Game Mode 1");
            _game.ChangeState(new GameState1(_game, _graphicsDevice, _content, _graphics));
        }

        private void GameMode2Button_Click(object sender, EventArgs e)
        { 
            Console.WriteLine("Game Mode 2");
            _game.ChangeState(new GameState2(_game, _graphicsDevice, _content, _graphics));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            Vector2 logoScale = new Vector2(0.3f, 0.3f); 
            Vector2 logoPosition = new Vector2(screenWidth / 2 - (_logoTexture.Width * logoScale.X) / 2, 30);
            spriteBatch.Draw(_logoTexture, logoPosition, null, Color.White, 0f, Vector2.Zero, logoScale, SpriteEffects.None, 0f);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);

            }
        }
        public override void PostUpdate(GameTime gameTime)
        {
            // Remove sprites if they are not needed

        }
    }
}
