using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public abstract class DisplayObject : Transformable, Drawable
{
    private Vector2f _refPoint;
    
    public Vector2f RefPoint
    {
        get
        {
            return _refPoint;
        }
        set
        {
            Vector2f delta = value - _refPoint;
            Point1 += delta;
            Point2 += delta;
            Position = value;
            _refPoint = value;
        }
    }
    public Vector2f Point1 { get; set; }
    public Vector2f Point2 { get; set; }
    
    public abstract Color ForeGroundColor { get; set; }
    public abstract Color BackGroundColor { get; set; }
    public bool IsVisible { get; set; } = true;
    public bool IsDestroyable { get; init; } = false;
    
    public bool isStaticObject { get; set; } = true;
    
    public Vector2f Velocity { get; set; }

    public float Height
    {
        get
        {
            return Point2.Y - Point1.Y;
        }
    }

    public float Width
    {
        get
        {
            return Point2.X - Point1.X;
        }
    }

    public abstract void Update(Time elapsedTime);

    public bool CheckCollision(DisplayObject obj) => (Point1.Y >= obj.Point2.Y && Point2.Y <= obj.Point1.Y) &&
                                                (Point1.X <= obj.Point2.X && Point2.X >= obj.Point1.X);

    public virtual void HandleCollison(DisplayObject obj)
    {
    }
    

    public void Move(Time elapsedTime)
    {
        RefPoint += Velocity * elapsedTime.AsSeconds();
    }

    public abstract void Draw(RenderTarget target, RenderStates states);
}