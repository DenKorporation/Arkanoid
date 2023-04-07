using System.Drawing;
using System.Numerics;

namespace Arkanoid;

public struct Point
{
    public int X;
    public int Y;
}

public abstract class DisplayObject
{
    protected Point RefPoint { get; set; }
    protected int X1 { get; set; }
    protected int Y1 { get; set; }
    protected int X2 { get; set; }
    protected int Y2 { get; set; }
    public Color ForeGroundColor { get; set; }
    public Color BackGroundColor { get; set; }
    protected byte IsVisible { get; set; }
    protected bool IsDestroyable { get; init; }
    public Vector2 Velocity;

    public bool isStaticObject { get; set; }

    public float Height
    {
        get;
    }

    public float Width
    {
        get;
    }

    public abstract void Update();
    public abstract void Collision();
    public abstract void Draw();

    public void Move(long elapsedMilliseconds)
    {
        throw new NotImplementedException();
    }
}