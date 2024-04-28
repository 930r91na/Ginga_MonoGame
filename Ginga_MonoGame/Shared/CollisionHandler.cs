using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Ginga_MonoGame.Content.States;

namespace GingaGame.Shared;

public class CollisionHandler
{
    private readonly List<Planet> _planets;
    private readonly List<(Planet, Planet)> _potentialCollisionPairs = new List<(Planet, Planet)>();
    private readonly Scene scene;
    private readonly PlanetFactory planetFactory;
    private readonly Score score;
    private readonly Container container;
    private readonly State gameState;

    public CollisionHandler(
        Scene scene,
        PlanetFactory planetFactory,
        Score score,
        Container container,
        State gameState)
    {
        this.scene = scene;
        this.planetFactory = planetFactory;
        this.score = score;
        this.container = container;
        this.gameState = gameState;
        _planets = scene.Planets;
    }

    public void CheckCollisions()
    {
        _potentialCollisionPairs.Clear();

        // Step 1: Broad Phase
        BroadPhaseCheck();

        // Step 2: Narrow Phase
        NarrowPhaseCheck();
    }

    private void BroadPhaseCheck()
    {
        for (var i = 0; i < _planets.Count; i++)
        for (var j = i + 1; j < _planets.Count; j++)
        {
            var planet1 = _planets[i];
            var planet2 = _planets[j];

            // If the planets are pinned, they cannot collide
            if (planet1.IsPinned || planet2.IsPinned) continue;

            // Calculate bounding boxes
            if (!DoBoundingBoxesIntersect(planet1, planet2)) continue; // No potential collision

            // Potential collision - pass to Narrow Phase
            _potentialCollisionPairs.Add((planet1, planet2));
        }
    }

    private static bool DoBoundingBoxesIntersect(Planet planet1, Planet planet2)
    {
        var box1 = new RectangleF(planet1.Position.X - planet1.Radius, planet1.Position.Y - planet1.Radius,
            planet1.Radius * 2, planet1.Radius * 2);
        var box2 = new RectangleF(planet2.Position.X - planet2.Radius, planet2.Position.Y - planet2.Radius,
            planet2.Radius * 2, planet2.Radius * 2);

        return box1.IntersectsWith(box2);
    }

    private void NarrowPhaseCheck()
    {
        // If there are no potential collision pairs, no need to check further
        if (_potentialCollisionPairs.Count == 0) return;

        // Create a copy of the list
        var potentialCollisionPairsCopy = new List<(Planet, Planet)>(_potentialCollisionPairs);

        // Iterate through pairs of planets that passed the broad phase
        foreach (var (planet1, planet2) in potentialCollisionPairsCopy)
        {
            // Recalculate distance (more accurate, as in broad-phase it might be an overestimate)
            var distanceX = planet1.Position.X - planet2.Position.X;
            var distanceY = planet1.Position.Y - planet2.Position.Y;
            var distanceSquared = distanceX * distanceX + distanceY * distanceY;
            var sumOfRadiiSquared = (planet1.Radius + planet2.Radius) * (planet1.Radius + planet2.Radius);

            if (distanceSquared <= sumOfRadiiSquared) // Collision detected
                HandleCollision(planet1, planet2);
        }
    }

    private void HandleCollision(Planet planet1, Planet planet2)
    {
        // Merging planets 
        if (planet1.PlanetType == planet2.PlanetType)
        {
            // Handle same planet collision
            MergePlanets(planet1, planet2);

            // Handle collisions again, as the new planet might collide with others
            CheckCollisions();
        }
        else
        {
            // Handle different planet collision
            HandleDifferentPlanetCollision(planet1, planet2);
        }
    }

    private void MergePlanets(Planet planet1, Planet planet2)
    {
        scene.RemovePlanet(planet1);
        scene.RemovePlanet(planet2);

        // Unlock new planet (if needed)
        if (!UnlockNextPlanetType(planet1, planet2)) return;

        // Create a new planet
        var newPlanet = CreateMergedPlanet(planet1, planet2);

        // Update the current planet in GameMode2 if needed
        if (gameState is GameState2)
        {
            //var currentPlanet = gameMode2Control.GetCurrentPlanet();
            //if (currentPlanet == planet1 || currentPlanet == planet2)
            //    gameMode2Control.SetCurrentPlanet(newPlanet);
        }

        // Add the new planet to the scene
        scene.AddPlanet(newPlanet);

        // Update scores for game mode 1
        if (gameState is not GameState1) return;
        UpdateScoreWithPlanetPoints(newPlanet.Points);
    }

    private bool UnlockNextPlanetType(Planet planet1, Planet planet2)
    {
        switch (gameState)
        {
            case GameState1:
                if (planet1.PlanetType + 1 >= 11) // if the largest planet is reached
                {
                    const int largestPlanetScore = 100;
                    UpdateScoreWithPlanetPoints(largestPlanetScore);
                    return false; // No new planet to unlock
                }

                // Unlock the next planet
                planetFactory.UnlockPlanet(planet1.PlanetType + 1);
                break;

            case GameState2:
                if (planet2.PlanetType - 1 <= 0) // if the smallest planet is reached
                    return false; // No new planet to unlock

                // Unlock the previous planet
                planetFactory.UnlockPlanet(planet2.PlanetType - 1);
                break;
            default:
                throw new ArgumentException("Invalid game mode");
        }

        return true;
    }

    private Planet CreateMergedPlanet(Planet planet1, Planet planet2)
    {
        // The position of the new planet will be the middle point between the two planets
        var middlePoint = (planet1.Position + planet2.Position) / 2;

        var newPlanet = gameState switch
        {
            GameState1 => new Planet(1, null, new Vector2(0, 0), 0),
            GameState2 => new Planet(1, null, new Vector2(0, 0), 0),
            _ => throw new ArgumentException("Invalid game state")
        };
        return newPlanet;
    }

    private void UpdateScoreWithPlanetPoints(int largestPlanetScore)
    {
        score.IncreaseScore(largestPlanetScore);
        score.HasChanged = true;
    }

    private static void HandleDifferentPlanetCollision(Planet planet1, Planet planet2)
    {
        // 1. Overlap Correction
        CorrectOverlap(planet1, planet2);

        // 2. Simulate Bounce if velocity is high enough
        SimulateBounce(planet1, planet2);
    }

    private static void CorrectOverlap(Planet planet1, Planet planet2)
    {
        const float overlapCorrectionFactor = 0.5f;

        var overlap = planet1.Radius + planet2.Radius - Vector2.Distance(planet1.Position, planet2.Position);
        var normal = (planet1.Position - planet2.Position).Normalized(); // Collision direction
        var positionAdjustment = normal * overlap * overlapCorrectionFactor;

        var totalMass = planet1.Mass + planet2.Mass;
        var massRatio1 = planet2.Mass / totalMass;
        var massRatio2 = planet1.Mass / totalMass;

        planet1.Position += positionAdjustment * massRatio1;
        planet2.Position -= positionAdjustment * massRatio2;
    }

    private static void SimulateBounce(Planet planet1, Planet planet2)
    {
        // Check if the velocity is high enough for a bounce
        const float velocityThreshold = 0.8f;
        var relativeVelocity = planet1.Velocity - planet2.Velocity;
        var normal = (planet1.Position - planet2.Position).Normalized();
        var velocityAlongNormal = relativeVelocity.Dot(normal);

        // If the velocity is not high enough, no bounce
        if (Math.Abs(velocityAlongNormal) < velocityThreshold) return;

        // Bounce the planets
        const float bounceFactor = 0.1f;
        var separationVelocity = normal * bounceFactor;
        planet1.Position += separationVelocity;
        planet2.Position -= separationVelocity;

        planet1.HasCollided = true;
        planet2.HasCollided = true;
    }

    public void CheckConstraints(Planet planet)
    {
        WallConstraints(planet);
        ContainerConstraints(planet);
        if (gameState is GameState1) FloorConstraints(planet);
    }

    private static void WallConstraints(Planet planet)
    {
        // Check if the point is outside the top boundary of the wall
        if (planet.Position.Y < planet.Radius) planet.Position.Y = planet.Radius;
    }

    private void ContainerConstraints(Planet planet)
    {
        // Check if the point is outside the left boundary of the container
        if (container != null && planet.Position.X < container.TopLeft.X + planet.Radius)
            planet.Position.X = container.TopLeft.X + planet.Radius;

        // Check if the point is outside the right boundary of the container
        if (container != null && planet.Position.X > container.TopRight.X - planet.Radius)
            planet.Position.X = container.TopRight.X - planet.Radius;

        // Check if the point is outside the bottom boundary of the container
        if (container != null && planet.Position.Y > container.BottomLeft.Y - planet.Radius)
            planet.Position.Y = container.BottomLeft.Y - planet.Radius;
    }

    private void FloorConstraints(Planet planet)
    {
        if (gameState is not GameState2) return; // Apply only in GameMode2

        // Find the current floor
        var floor = scene.Floors.FirstOrDefault(f =>
            f.StartPositionY <= planet.Position.Y && planet.Position.Y <= f.EndPositionY);

        if (floor == null) return; // Planet is outside the floor range

        // Check if the planet can pass through the floor
        if (planet.PlanetType <= floor.NextPlanetIndex)
            // Can pass - no collision
            return;

        const int floorEndPositionHeight = 30;

        // Handle Collision (similar to container boundaries)
        if (!(planet.Position.Y > floor.EndPositionY - floorEndPositionHeight - planet.Radius)) return;

        if (floor.NextPlanetIndex == -1) // Last floor
            // Game Won
            //gameMode2Control.GameWon();
        planet.Position.Y = floor.EndPositionY - floorEndPositionHeight - planet.Radius;
    }

    public bool IsGameOver()
    {
        // Check if a planet has passed the end line
        return _planets.Any(planet =>
            planet.Position.Y < container.TopLeft.Y + planet.Radius && planet.HasCollided);
    }
}