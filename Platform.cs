using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Platform : DisplayObject
{
    public event EventHandler<BonusPickedUpArgs>? BonusPickedUp;
    
    public class BonusPickedUpArgs: EventArgs
    {
        public BonusItem.Type Type;
        public int Score;

        public BonusPickedUpArgs(BonusItem.Type type, int score)
        {
            Type = type;
            Score = score;
        }
    }
    
    private RectangleShape _shape;

    public sealed override Color ForeGroundColor
    {
        get { return _shape.FillColor; }
        set { _shape.FillColor = value; }
    }

    public override Color BackGroundColor
    {
        get { return _shape.OutlineColor; }
        set { _shape.OutlineColor = value; }
    }

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


    public Platform(Vector2f size)
    {
        isStaticObject = false;
        _shape = new RectangleShape(size);
        ForeGroundColor = Color.White;
        Point1 -= size / 2f;
        Point2 += size / 2f;
        _shape.Origin = size / 2f;
        Collision += HandleCollision;
    }

    public override void HandleCollision(object? sender, CollisionEventArgs args)
    {
        if (Point1.X < 0)
        {
            RefPoint = new Vector2f(Width / 2f, RefPoint.Y);
            return;
        }

        if (Point2.X > args.Width)
        {
            RefPoint = new Vector2f(args.Width - Width / 2f, RefPoint.Y);
            return;
        }

        if (args.Object is Ball ball)
        {
            if (RefPoint.X < Point1.X)
            {
                RefPoint = new Vector2f(ball.Width + Width / 2f, RefPoint.Y);
                return;
            }

            if (RefPoint.X > Point2.X)
            {
                RefPoint = new Vector2f(args.Width - ball.Width - Width / 2f, RefPoint.Y);
                return;
            }
            
        }
        else if(args.Object is BonusItem bonusItem)
        {
            BonusPickedUp?.Invoke(this, new BonusPickedUpArgs(bonusItem.CurType, bonusItem.ScoreValue));
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
        Move(elapsedTime);
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(_shape, states);
    }
}