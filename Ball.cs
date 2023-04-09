using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Ball : DisplayObject
{
    private CircleShape _shape;
    private const float BallVelocity = 200f; 

    public float ShapeRadius
    {
        get { return _shape.Radius; }
        set { _shape.Radius = value; }
    }

    public override Color ForeGroundColor
    {
        get { return _shape.FillColor; }
        set { _shape.FillColor = value; }
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
    }


    public void Launch()
    {
        Velocity = new Vector2f(0, -BallVelocity);
    }

    public override void Update(Time elapsedTime)
    {
        Move(elapsedTime);
    }

    public void HandleCollison(DisplayObject obj)
    {
        throw new NotImplementedException();
    }


    public override void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(_shape, states);
    }
}