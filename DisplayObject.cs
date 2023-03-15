namespace Arkanoid;

public struct Point
{
    public int X;
    public int Y;
}

public abstract class DisplayObject
{
    protected Point LeftUpCorner { get; set; }
    protected Point RightUpCorner { get; set; }
    public uint ForeGroundColor { get; set; }
    public uint BackGroundColor { get; set; }
    protected bool IsVisible { get; set; }
    protected bool IsDestroyable { get; init; }

    public float Height
    {
        get
        {
            return Math.Abs(RightUpCorner.Y - LeftUpCorner.Y);
        }
    }

    public float Width
    {
        get
        {
            return Math.Abs(RightUpCorner.X - LeftUpCorner.Y);
        }
    }

    public abstract void Draw();
}