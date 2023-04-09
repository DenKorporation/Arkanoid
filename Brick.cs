using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Brick : DisplayObject
{
    private int _healthPoint;
    private bool _isDestroyed;
    private BonusItems _bonusItems;

    private RectangleShape _shape = new ();

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

    public Brick(Vector2f size)
    {
        _shape.Size = size - new Vector2f(4, 4);
        _shape.Origin = _shape.Size / 2f;
        Point1 = RefPoint - size / 2f;
        Point2 = RefPoint + size / 2f;
        ForeGroundColor = Color.Blue;
        BackGroundColor = Color.White;
        _shape.OutlineThickness = 2;
    }


    public void TakeHit(int hit)
    {
        throw new NotImplementedException();
    }

    public override void Update(Time elapsedTime)
    {
        throw new NotImplementedException();
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(_shape, states);
    }
}