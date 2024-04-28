using Ginga_MonoGame.Content.Control;
using GingaGame.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;

namespace GingaGame.GameMode1;

public class GameStateHandler
{
    private const float EndLineHeight = 70;
    private const int EndLineThreshold = 70;
    private const int Tolerance = 5;
    private readonly List<Planet> _planets;
    private bool _gameOverTriggered;
    private bool _gameWonTriggered;
    private bool _renderEndLine;
    private Scene _scene;
    private Score _score;
    private Scoreboard _scoreboard;

    public GameStateHandler(Scene scene, Score score, Scoreboard scoreboard)
    {
        _gameOverTriggered = false;
        _gameWonTriggered = false;
        _renderEndLine = false;
        _scene = scene;
        _score = score;
        _scoreboard = scoreboard;
        _planets = _scene.Planets;
    }

    public void CheckGameState()
    {
        RenderEndLine(_renderEndLine);

        // Check if a planet is near the endLine from the bottom
        IsNearEndLine();
        if (!_gameOverTriggered) CheckLoseCondition();

        if (!_gameWonTriggered) CheckWinCondition();
    }

    private void RenderEndLine(bool shouldRenderEndLine)
    {
        //const float verticalTopMargin = 70;
        //var horizontalMargin = (canvas.Width - canvas.Width / 3) / 2;
        //
        //var topLeft = new PointF(horizontalMargin, verticalTopMargin);
        //var topRight = new PointF(canvas.Width - horizontalMargin, verticalTopMargin);
        //
        //var blinkOn = DateTime.Now.Second % 2 == 0;
        //
        //var currentPen = blinkOn ? Pens.Red : Pens.Transparent;
        //
        //// Draw the end line
        //canvas.Graphics?.DrawLine(shouldRenderEndLine ? currentPen : Pens.Transparent, topLeft, topRight);
    }

    private void IsNearEndLine()
    {
        _renderEndLine = false;
        foreach (var unused in _planets.Where(planet =>
                     planet.Position.Y < EndLineHeight + EndLineThreshold + planet.Radius && planet.HasCollided &&
                     !planet.IsPinned))
        {
            _renderEndLine = true;
            break;
        }
    }

    private void CheckLoseCondition()
    {
        if (!_planets.Any(planet =>
                planet.Position.Y < EndLineHeight + planet.Radius - Tolerance && planet.HasCollided)) return;
        _gameOverTriggered = true;

        // Show a dialog box when the game is over
        new DialogBox("Game Over! You lost!", ResetGame);
    }

    private void CheckWinCondition()
    {
        if (_planets.All(planet => planet.PlanetType != 10)) return;
        _gameWonTriggered = true;

        // Show a dialog box when the game is won
        new DialogBox("Congratulations! You won!", ResetGame);
    }

    private void ResetGame()
    {
        // Reset the game state
        //TODO: Implement the reset game logic
        _gameOverTriggered = false;
        _gameWonTriggered = false;
    }
}