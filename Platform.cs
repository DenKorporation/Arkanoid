using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Platform : DisplayObject
{

    private RectangleShape _shape;

    public override Color ForeGroundColor
    {
        get
        {
            return _shape.FillColor;
        }
        set
        {
            _shape.FillColor = value;
        }
    }

    public override Color BackGroundColor
    {
        get
        {
            return _shape.OutlineColor;
        }
        set
        {
            _shape.OutlineColor = value;
        }
    }

    public Platform(Vector2f size)
    {
        isStaticObject = false;
        _shape = new RectangleShape(size);
        ForeGroundColor = Color.White;
        Point1 -= size / 2f;
        Point2 += size / 2f;
        _shape.Origin = size / 2f;
    }

    public override void Update(Time elapsedTime)
    {
        Move(elapsedTime);
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(_shape, states);
    }
}