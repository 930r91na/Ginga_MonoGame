using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GingaGame.Shared;

public class Scene
{
    private Container _container;
    public List<Planet> Planets { get; } = new List<Planet>();
    public List<Floor> Floors { get; } = new List<Floor>();

    public void AddPlanet(Planet planet)
    {
        Planets.Add(planet);
    }

    public void RemovePlanet(Planet planet)
    {
        Planets.Remove(planet);
    }

    private void AddFloor(Floor floor)
    {
        Floors.Add(floor);
    }

    public void AddContainer(Container container)
    {
        _container = container;
    }

    public void InitializeFloors(int floorHeight, int verticalTopMargin, int totalFloors = 4,
        List<int> planetsPerFloor = null)
    {
        // If the planets per floor list is not provided, set it to the default values
        planetsPerFloor ??= new List<int> { 3, 3, 4, 1 };

        var nextPlanetIndex = PlanetSizes.Sizes.Count - planetsPerFloor[0] - 1;
        for (var i = 0; i < totalFloors; i++)
        {
            var floor = new Floor
            {
                StartPositionY = i * floorHeight + verticalTopMargin,
                EndPositionY = (i + 1) * floorHeight + verticalTopMargin,
                Index = i,
                NextPlanetIndex = nextPlanetIndex
            };
            AddFloor(floor);

            switch (nextPlanetIndex)
            {
                // Set the next planet index for the next floor
                case > 0:
                    nextPlanetIndex -= planetsPerFloor[i + 1];
                    continue;
                // Set the next planet index to -1 for the last floor
                case <= 0:
                    nextPlanetIndex = -1;
                    break;
            }
        }
    }

    public void Draw(SpriteBatch spb, float canvasHeight, float yOffset = 0)
    {
        // Calculate the visible range
        var visibleStartY = yOffset;
        var visibleEndY = yOffset + canvasHeight;
        
        // Check if the planets are within the visible range
        foreach (var planet in Planets.Where(planet =>
                     planet.Position.Y + planet.Radius >= visibleStartY &&
                     planet.Position.Y - planet.Radius <= visibleEndY))
            planet.Draw(spb, yOffset);
        
        // Check if the floor is within the visible range
        foreach (var floor in Floors.Where(floor =>
                     floor.EndPositionY >= visibleStartY && floor.StartPositionY <= visibleEndY))
            //floor.Draw(_container, yOffset);
        
        // Render the container if it's within the visible range
        if (_container.BottomLeft.Y >= visibleStartY && _container.TopLeft.Y <= visibleEndY) { 
            //_container.Draw(yOffset);
        }
    }

    public void Clear()
    {
        Planets.Clear();
    }
}