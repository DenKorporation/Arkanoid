using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arkanoid;

public abstract class DisplayObject : Transformable, Drawable
{
    public event EventHandler<CollisionEventArgs>? Collision;

    private Vector2f _refPoint;
    
    public Vector2f RefPoint
    {
        get => _refPoint;
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

    public bool CheckCollision(GameField? gameField, DisplayObject? obj)
    {
        if (gameField is not null)
        {
            return Point1.X < 0 || Point1.Y < 0 || Point2.X > gameField.Width || Point2.Y > gameField.Height;
        }
        else if (obj is not null)
        {
            return (Point1.Y <= obj.Point2.Y && Point2.Y >= obj.Point1.Y) &&
                (Point1.X <= obj.Point2.X && Point2.X >= obj.Point1.X);
        }
        else
        {
            return false;
        }
    }

    public virtual void HandleCollision(object? sender, CollisionEventArgs args)
    {
    }
    
    public abstract String Serialize();
    public abstract DisplayObject Deserialize(String str);
    

    public void Move(Time elapsedTime)
    {
        RefPoint += Velocity * elapsedTime.AsSeconds();
    }

    public abstract void Draw(RenderTarget target, RenderStates states);

    public void OnCollision(CollisionEventArgs e)
    {
        Collision?.Invoke(this, e);
    }
}