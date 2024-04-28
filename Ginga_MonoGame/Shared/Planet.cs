using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GingaGame.Shared;

public sealed class Planet: VPoint
{
    
    private readonly Texture2D _texture;
    public int PlanetType { get; }
    public int Points { get; private set; }
    public bool HasCollided { get; set; }

    public Planet(int planetType, Texture2D texture, Vector2 position, float radius, bool hasCollided = false)
        : base(position, radius)
    {
        PlanetType = planetType;
        _texture = texture;
        HasCollided = hasCollided;
        Points = PlanetPoints.PointsPerPlanet[planetType];
    }

    public void Draw(SpriteBatch spriteBatch, float radius)
    {
        // TODO: Implement the Draw method for different sizes
        var destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(Radius * 2), (int)(Radius * 2));
        spriteBatch.Draw(_texture, destinationRectangle, Color.White);
    }

    public void Update(GameTime gameTime)
    {
        //base.Update();
    }
}

public static class PlanetTextures
{
    private static readonly Dictionary<int, Texture2D> CachedTextures = new();

    public static Texture2D GetCachedTexture(ContentManager contentManager, int planetType)
    {
        if (CachedTextures.TryGetValue(planetType, out var texture)) return texture;

        // Load and cache the texture
        var textureName = GetTextureName(planetType);
        var loadedTexture = contentManager.Load<Texture2D>("Resources/" + textureName);
        CachedTextures[planetType] = loadedTexture;

        return CachedTextures[planetType];
    }

    private static string GetTextureName(int planetType)
    {
        return planetType switch
        {
            0 => "Pluto",
            1 => "Moon",
            2 => "Mercury",
            3 => "Mars",
            4 => "Venus",
            5 => "Earth",
            6 => "Neptune",
            7 => "Uranus",
            8 => "Saturn",
            9 => "Jupiter",
            10 => "Sun",
            _ => throw new ArgumentException("Invalid planetType")
        };
    }
}

public static class PlanetSizes
{
    public static Dictionary<int, float> Sizes { get; } = new()
    {
        { 0, 25f },
        { 1, 30f },
        { 2, 35f },
        { 3, 40f },
        { 4, 45f },
        { 5, 50f },
        { 6, 55f },
        { 7, 60f },
        { 8, 65f },
        { 9, 70f },
        { 10, 75f }
    };
}

public static class PlanetPoints
{
    public static Dictionary<int, int> PointsPerPlanet { get; } = new()
    {
        { 0, 10 },
        { 1, 12 },
        { 2, 14 },
        { 3, 16 },
        { 4, 18 },
        { 5, 20 },
        { 6, 22 },
        { 7, 24 },
        { 8, 26 },
        { 9, 28 },
        { 10, 30 }
    };
}