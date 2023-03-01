namespace Arkanoid;

 public struct Vector  
{
    public float X { get; set; }
    public float Y { get; set; }
}

public abstract class FieldEntity
{
    protected Vector Position { get; set; }
    protected uint Color { get; set; }
    protected bool IsDestroyable { get; init; }
    public Vector Velocity { get; set; }

    public abstract void draw();
    
    public void move(TimeOnly elapsedTime)
    {
        Vector position = Position;
        position.X += Velocity.X * elapsedTime.Second;
        position.Y += Velocity.Y * elapsedTime.Second;
    }
}