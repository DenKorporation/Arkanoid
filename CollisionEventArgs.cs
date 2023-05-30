namespace Arkanoid;

public class CollisionEventArgs : EventArgs
{
    public float Height;
    public float Width;
    public object? Container;
    public DisplayObject? Object;

    

    public CollisionEventArgs(float height, float width, Object? container = null, DisplayObject? displayObject = null)
    {
        Height = height;
        Width = width;
        Container = container;
        Object = displayObject;
    }
}