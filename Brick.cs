using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Brick : DisplayObject
{
    private int _healthPoint { get; set; }
    public BonusItem BonusItem { get; set; }

    public RectangleShape _shape { get; set; }= new();

    public Vector2f Size
    {
        get => _shape.Size;
        set
        {
            _shape.Size = value;
            _shape.Origin = _shape.Size / 2f;
            Point1 = RefPoint - _shape.Size / 2f;
            Point2 = RefPoint + _shape.Size / 2f;
        }
    }

    public new bool IsDestroyable { get; set; } = true;

    public bool IsDestroyed => _healthPoint <= 0;

    public sealed override Color ForeGroundColor
    {
        get => _shape.FillColor;
        set => _shape.FillColor = value;
    }

    public sealed override Color BackGroundColor
    {
        get => _shape.OutlineColor;
        set => _shape.OutlineColor = value;
    }

    public Brick(Vector2f size, int healthPoint)
    {
        _healthPoint = healthPoint;
        _shape.Size = size - new Vector2f(4, 4);
        _shape.Origin = _shape.Size / 2f;
        Point1 = RefPoint - size / 2f;
        Point2 = RefPoint + size / 2f;
        ForeGroundColor = Color.Blue;
        BackGroundColor = Color.White;
        _shape.OutlineThickness = 2;

        Collision += HandleCollision;
    }


    private void TakeHit(int hit)
    {
        _healthPoint -= hit;
        if (_healthPoint >= 300)
        {
            ForeGroundColor = Color.Blue;
        }else if (_healthPoint >= 200)
        {
            ForeGroundColor = Color.Yellow;
        }
        else if (_healthPoint >= 100)
        {
            ForeGroundColor = Color.Red;
        } 
        if (_healthPoint < 0)
        {
            _healthPoint = 0;
        }
    }

    public override void HandleCollision(object? sender, CollisionEventArgs args)
    {
        if (args.Object is not null)
        {
            if (args.Object is Ball && IsDestroyable)
            {
                TakeHit(Ball.HitValue);
                if (IsDestroyed && args.Container is GameField gameField)
                {
                    gameField.RemoveDisplayObjectRequest(this);
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