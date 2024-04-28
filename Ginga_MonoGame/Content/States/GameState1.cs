using GingaGame.GameMode1;
using GingaGame.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Ginga_MonoGame.Content.States
{
    public class GameState1 : State
    {
        private List<Component> _components;
        private Texture2D _backgroundTexture;
        private SpriteFont _textFont;
        private SpriteFont _titleFont;
        private int screenWidth = 1540;
        private int screenHeight = 846;

        // Add your game variables here
        private Planet _currentPlanet;
        private Planet _nextPlanet;
        private PlanetFactory _planetFactory;
        private Score _score;
        private Scoreboard _scoreboard;
        private CollisionHandler _collisionHandler;
        private GameStateHandler _gameStateHandler;
        private Scene _scene;
        private Container _container;

        public GameState1(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics) : base(game, graphicsDevice, content, graphics)
        {
            _backgroundTexture = _content.Load<Texture2D>("Resources/Background2");
            _textFont = _content.Load<SpriteFont>("Fonts/Font");
            _titleFont = _content.Load<SpriteFont>("Fonts/Title");

            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();
            _game.Window.Title = "Game mode 1";

            // Initialize your game variables here
            _container = new Container();
            _container.InitializeGameMode1(screenWidth, screenHeight);
            _score = new Score();
            _scoreboard = new Scoreboard();
            _scene = new Scene();
            //_currentPlanet = new Planet(0, new Vector2(0, 0)) { IsPinned = true };
            _scene.AddPlanet(_currentPlanet);
            //_planetFactory = new PlanetFactory(GameMode.Mode1);
            //_collisionHandler = new CollisionHandler(_scene, _planetFactory, _score, _container, GameMode.Mode1);
           //_gameStateHandler = new GameStateHandler(_scene, _score, _scoreboard, this);

            _components = new List<Component>() { /* Add your components here */ };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw background
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            // Draw UI components
            DrawUI(spriteBatch);

            // Draw scene and container
            _scene.Draw(spriteBatch, screenHeight);
            _container.Render(spriteBatch);
            spriteBatch.End();
        }

        private void DrawUI(SpriteBatch spriteBatch)
        {
            // Draw preview area for upcoming planets
            //spriteBatch.Draw(_planetFactory.GetNextPlanetTexture(), new Rectangle(10, 10, 100, 100), Color.White);

            // Draw score
            //spriteBatch.DrawString(_scoreFont, $"Score: {_score.CurrentScore}", new Vector2(10, 120), Color.White);

            // Draw top scores, evolution cycle, etc.
            // Assume _topScores is a List<int> and _evolutionTexture is a Texture2D
            //for (int i = 0; i < _scoreboard.TopScores.Count; i++)
            //{
            //    spriteBatch.DrawString(_scoreFont, $"{i + 1}. {_scoreboard.TopScores[i]}", new Vector2(10, 150 + 20 * i), Color.White);
            //}
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // Add any post-update logic here
        }

        public override void Update(GameTime gameTime)
        {
            //foreach (var planet in _scene.Planets)
            //    planet.Update(gameTime);

            // Check and handle collisions
            //_collisionHandler.CheckCollisions();

            // Update the game state based on events
            //_gameStateHandler.CheckGameState();
        }
    }
}
