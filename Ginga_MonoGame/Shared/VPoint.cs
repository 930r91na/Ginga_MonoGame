namespace GingaGame.Shared;

public class VPoint
{
    private const float Friction = 0.8f;
    private readonly Vector2 _gravity = new(0, 0.9f);
    public readonly float Radius;
    public bool IsPinned;
    public Vector2 OldPosition;
    public Vector2 Position;
    public Vector2 Velocity;

    protected VPoint(Vector2 position, float radius)
    {
        Position = OldPosition = position;
        Radius = radius;
        Mass = radius / 10;
    }

    public float Mass { get; }

    public void Update()
    {
        if (IsPinned) return;

        Velocity = Position - OldPosition;
        Velocity *= Friction;

        // Save current position
        OldPosition = Position;

        // Perform Verlet integration
        Position += Velocity + _gravity;
    }
}