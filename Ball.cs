using System.ComponentModel;
using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Ball : DisplayObject
{
    public CircleShape _shape { get; set; }
    public static float BallVelocity { get; set; } = 300f;
    public const int HitValue = 100;

    public float ShapeRadius
    {
        get => _shape.Radius;
        set
        {
            _shape.Radius = value;
            _shape.Origin = new Vector2f(ShapeRadius, ShapeRadius);
            Point1 = new Vector2f(RefPoint.X - ShapeRadius, RefPoint.Y - ShapeRadius);
            Point2 = new Vector2f(RefPoint.X + ShapeRadius, RefPoint.Y + ShapeRadius);
        }
    }

    public override Color ForeGroundColor
    {
        get => _shape.FillColor;
        set => _shape.FillColor = value;
    }

    public override Color BackGroundColor
    {
        get { return _shape.OutlineColor; }
        set { _shape.OutlineColor = value; }
    }

    public Ball(float radius)
    {
        Vector2f size = new Vector2f(radius, radius);
        _shape = new CircleShape(radius);
        Point1 -= size;
        Point2 += size;
        _shape.Origin = size;
        ForeGroundColor = Color.Red;
        isStaticObject = false;

        Collision += HandleCollision;
    }


    public void Launch()
    {
        Velocity = new Vector2f(0, -BallVelocity);
    }

    public override void Update(Time elapsedTime)
    {
        Move(elapsedTime);
    }

    public override void HandleCollision(object? sender, CollisionEventArgs args)
    {
        if (Point1.X < 0)
        {
            RefPoint = new Vector2f(Width / 2f, RefPoint.Y);
            Velocity = new Vector2f(-Velocity.X, Velocity.Y);
            return;
        }

        if (Point2.X > args.Width)
        {
            RefPoint = new Vector2f(args.Width - Width / 2f, RefPoint.Y);
            Velocity = new Vector2f(-Velocity.X, Velocity.Y);
            return;
        }

        if (Point1.Y < 0)
        {
            RefPoint = new Vector2f(RefPoint.X, Height / 2f);
            Velocity = new Vector2f(Velocity.X, -Velocity.Y);
            return;
        }

        if (Point1.Y > args.Height)
        {
            if (args.Container is GameField gameField)
            {
                gameField.RemoveDisplayObjectRequest(this);
            }

            return;
        }

        if (args.Object is Platform platform)
        {
            if (RefPoint.X <= platform.Point1.X &&
                Math.Abs(RefPoint.X - platform.Point1.X) > Math.Abs(RefPoint.Y - platform.Point1.Y))
            {
                RefPoint = new Vector2f(platform.Point1.X - ShapeRadius, RefPoint.Y);
                Velocity = new Vector2f(-Velocity.X, Velocity.Y);
            }
            else if (RefPoint.X >= platform.Point2.X &&
                     Math.Abs(RefPoint.X - platform.Point2.X) > Math.Abs(RefPoint.Y - platform.Point1.Y))
            {
                RefPoint = new Vector2f(platform.Point2.X + ShapeRadius, RefPoint.Y);
                Velocity = new Vector2f(-Velocity.X, Velocity.Y);
            }
            else
            {
                RefPoint = new Vector2f(RefPoint.X, platform.Point1.Y - Height / 2f);
                Velocity = new Vector2f(BallVelocity * (RefPoint.X - platform.RefPoint.X) / (platform.Width / 2f),
                    -Velocity.Y);
            }
        }

        if (args.Object is Brick brick)
        {
            if (RefPoint.Y <= brick.RefPoint.Y)
            {
                if (RefPoint.X <= brick.Point1.X &&
                    Math.Abs(RefPoint.X - brick.Point1.X) > Math.Abs(RefPoint.Y - brick.Point1.Y))
                {
                    RefPoint = new Vector2f(brick.Point1.X - ShapeRadius, RefPoint.Y);
                    Velocity = new Vector2f(-Velocity.X, Velocity.Y);
                }
                else if (RefPoint.X >= brick.Point2.X &&
                         Math.Abs(RefPoint.X - brick.Point2.X) > Math.Abs(RefPoint.Y - brick.Point1.Y))
                {
                    RefPoint = new Vector2f(brick.Point2.X + ShapeRadius, RefPoint.Y);
                    Velocity = new Vector2f(-Velocity.X, Velocity.Y);
                }
                else
                {
                    RefPoint = new Vector2f(RefPoint.X, brick.Point1.Y - Height / 2f);
                    Velocity = new Vector2f(Velocity.X, -Velocity.Y);
                }
            }
            else
            {
                if (RefPoint.X <= brick.Point1.X &&
                    Math.Abs(RefPoint.X - brick.Point1.X) > Math.Abs(RefPoint.Y - brick.Point2.Y))
                {
                    RefPoint = new Vector2f(brick.Point1.X - ShapeRadius, RefPoint.Y);
                    Velocity = new Vector2f(-Velocity.X, Velocity.Y);
                }
                else if (RefPoint.X >= brick.Point2.X &&
                         Math.Abs(RefPoint.X - brick.Point2.X) > Math.Abs(RefPoint.Y - brick.Point2.Y))
                {
                    RefPoint = new Vector2f(brick.Point2.X + ShapeRadius, RefPoint.Y);
                    Velocity = new Vector2f(-Velocity.X, Velocity.Y);
                }
                else
                {
                    RefPoint = new Vector2f(RefPoint.X, brick.Point2.Y + Height / 2f);
                    Velocity = new Vector2f(Velocity.X, -Velocity.Y);
                }
            }
        }
    }

    public override string Serialize()
    {
        throw new NotImplementedException();
    }

    public override DisplayObject Deserialize(string str)
    {
        throw new NotImplementedException();
    }


    public override void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(_shape, states);
    }
}