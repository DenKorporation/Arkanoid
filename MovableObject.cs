using System.Numerics;

namespace Arkanoid;

public abstract class MovableObject: DisplayObject
{
    public Vector2 Velocity;

    public void Move(long elapsedMilliseconds)
    {
        throw new NotImplementedException();
    }

    public void HandleCollision()
    {
        throw new NotImplementedException();
    }
}