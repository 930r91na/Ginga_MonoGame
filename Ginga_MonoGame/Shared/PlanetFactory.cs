using Ginga_MonoGame.Content.States;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GingaGame.Shared;

public class PlanetFactory
{
    private readonly Random _randomGenerator = new();
    private readonly List<int> _unlockedPlanets;
    private readonly State _gameState;
    private readonly Dictionary<int, Texture2D> _planetTextures;

    public PlanetFactory(State gameState, Dictionary<int, Texture2D> planetTextures)
    {
        _gameState = gameState;
        _planetTextures = planetTextures;
        _unlockedPlanets = GetInitialUnlockedPlanets();
    }

    private List<int> GetInitialUnlockedPlanets()
    {
        if (_gameState is GameState1)
        {
            return new List<int> { 0 }; // Start with Pluto
        }
        else if (_gameState is GameState2)
        {
            return new List<int> { 10 }; // Start with Sun
        }
        else
        {
            throw new ArgumentException("Invalid game state");
        }
    }

    public Planet GenerateNextPlanet()
    {
        int nextIndex;
        do
        {
            nextIndex = _gameState switch
            {
                GameState1 => _randomGenerator.Next(0, 5),
                GameState2 => _randomGenerator.Next(6, 11),
                _ => throw new ArgumentException("Invalid game state")
            };
        } while (!_unlockedPlanets.Contains(nextIndex));

        var middleX = _gameState.ScreenSize.Width / 2;

        return new Planet(nextIndex, _planetTextures[nextIndex], new Vector2(middleX, 0), PlanetSizes.Sizes[nextIndex]);
    }

    // Method to unlock a new planet (when merging happens)
    public void UnlockPlanet(int planetIndex)
    {
        if (!_unlockedPlanets.Contains(planetIndex)) _unlockedPlanets.Add(planetIndex);
    }

    public void ResetUnlockedPlanets()
    {
        _unlockedPlanets.Clear();

        _unlockedPlanets.AddRange(_gameState switch
        {
            GameState1 => new List<int> { 0 }, 
            GameState2 => new List<int> { 10 }, 
            _ => throw new ArgumentException("Invalid game state")
        });
    }


}