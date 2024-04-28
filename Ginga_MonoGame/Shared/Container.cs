using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace GingaGame.Shared;

public class Container
{
    private const int VerticalTopMargin = 70;
    public Vector2 TopLeft { get; private set; }
    public Vector2 TopRight { get; private set; }
    public Vector2 BottomLeft { get; private set; }
    public Vector2 BottomRight { get; private set; }

    public void InitializeGameMode1(float canvasWidth, float height)
    {
        const float verticalMargin = VerticalTopMargin;
        const float horizontalLength = 310;
        var horizontalMargin = (canvasWidth - horizontalLength) / 2;

        TopLeft = new Vector2(horizontalMargin, verticalMargin);
        TopRight = new Vector2(canvasWidth - horizontalMargin, verticalMargin);
        BottomLeft = new Vector2(horizontalMargin, height - verticalMargin);
        BottomRight = new Vector2(canvasWidth - horizontalMargin, height - verticalMargin);
    }

    public void InitializeGameMode2(float canvasWidth, float height, int verticalMargin = VerticalTopMargin,
        float horizontalMargin = 0)
    {
        if (horizontalMargin <= 0) horizontalMargin = (canvasWidth - canvasWidth / 3) / 2; // 1/3 of the width

        TopLeft = new Vector2(horizontalMargin, verticalMargin);
        TopRight = new Vector2(canvasWidth - horizontalMargin, verticalMargin);
        BottomLeft = new Vector2(horizontalMargin, height);
        BottomRight = new Vector2(canvasWidth - horizontalMargin, height);
    }

    public void Render(SpriteBatch spriteBatch, float yOffset = 0)
    {
        // Adjust the Y position with the offset
        var adjustedTopLeft = TopLeft + new Vector2(0, -yOffset);
        var adjustedBottomLeft = BottomLeft + new Vector2(0, -yOffset);
        var adjustedTopRight = TopRight + new Vector2(0, -yOffset);
        var adjustedBottomRight = BottomRight + new Vector2(0, -yOffset);

        // Draw the lines with the adjusted Y positions
        //spriteBatch.DrawLine(adjustedTopLeft, adjustedBottomLeft, Color.White);
        //spriteBatch.DrawLine(adjustedBottomLeft, adjustedBottomRight, Color.White);
        //spriteBatch.DrawLine(adjustedBottomRight, adjustedTopRight, Color.White);
    }
}