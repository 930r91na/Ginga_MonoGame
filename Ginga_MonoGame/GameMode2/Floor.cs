using GingaGame.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = GingaGame.Shared.Vector2;

public class Floor
{
    public int StartPositionY { get; set; }
    public int EndPositionY { get; set; }
    public int Index { get; set; }
    public int NextPlanetIndex { get; set; } // The index of the planet that the next level will allow

    public void Draw(SpriteBatch spriteBatch, Container container, Texture2D pixel, float yOffset = 0)
    {
        // Adjust the Y position with the offset
        var adjustedEndPositionY = EndPositionY - yOffset;

        var isLastFloor = NextPlanetIndex < 0; // Check if the current floor is the last one

        // Set the color to red if it's the last floor, otherwise set it to white
        var rectangleColor = isLastFloor ? new Color(255, 0, 0, 50) : new Color(255, 255, 255, 75);

        const int rectangleHeight = 30; // The height of the rectangle
        const int planetRadius = 15; // The radius of the planet
        var rectangleY = adjustedEndPositionY - rectangleHeight; // The Y position of the rectangle

        DrawFloorRectangle(spriteBatch, container, pixel, rectangleColor, rectangleY, rectangleHeight);

        // If it's not the last floor, draw the planet with a fixed radius to the left of the rectangle
        if (!isLastFloor)
        {
            DrawNextFloorPlanet(spriteBatch, container, planetRadius, rectangleY);
        }
    }

    private static void DrawFloorRectangle(SpriteBatch spriteBatch, Container container, Texture2D pixel, Color rectangleColor, float rectangleY, int rectangleHeight)
    {
        // Draw the rectangle
        var rectangle = new Rectangle((int)container.TopLeft.X, (int)rectangleY, (int)(container.BottomRight.X - container.TopLeft.X), rectangleHeight);
        spriteBatch.Draw(pixel, rectangle, rectangleColor);
    }

    private void DrawNextFloorPlanet(SpriteBatch spriteBatch, Container container, int planetRadius, float rectangleY)
    {
        var planetX = container.TopLeft.X - planetRadius * 2;
        var planetY = rectangleY + planetRadius;

        // Draw the planet
        // TODO Fix this for planet textures
        //var planet = new Planet(NextPlanetIndex, new Vector2(planetX, planetY));

        //planet.Draw(spriteBatch, planetRadius);
    }
}
