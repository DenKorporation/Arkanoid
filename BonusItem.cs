using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class BonusItem : DisplayObject
{
    public static float BonusVelocity { get; set; } = 300f; 
    
    public enum Type
    {
        None,
        IncreasePlatform,
        DecreasePlatform,
        IncreaseBallSpeed,
        ExtraScore
    }

    public float ShapeRadius
    {
        get => _shape.Radius;
        set
        {
            _shape.Radius = value;
            _shape.Origin = new Vector2f(ShapeRadius, ShapeRadius);
            Point1 = new Vector2f(RefPoint.X - ShapeRadius, RefPoint.Y - ShapeRadius);
            Point2 = new Vector2f(RefPoint.X + ShapeRadius, RefPoint.Y + ShapeRadius);

            _content.CharacterSize = (uint)(ShapeRadius / 1.5f);
            CenterOrigin();
        }
    }

    public string Content
    {
        get => _content.DisplayedString;
        set
        {
            _content.DisplayedString = value;
            CenterOrigin();
        }
    }

    public sealed override Color ForeGroundColor
    {
        get => _content.FillColor;
        set => _content.FillColor = value;
    }

    public sealed override Color BackGroundColor
    {
        get => _shape.FillColor;
        set => _shape.FillColor = value;
    }

    public bool IsActive { get; set; } = false;

    public Text _content { get; set; } = new();
    public CircleShape _shape { get; set; } = new();

    public Type CurType { get;  }
    public int ScoreValue { get; set; }

    public BonusItem(float shapeRadius, Type type, int scoreValue, Font? font)
    {
        _content.Font = font;
        ShapeRadius = shapeRadius;
        _shape.OutlineColor = Color.White;
        _shape.OutlineThickness = 1f;
        ScoreValue = scoreValue;
        
        Content = scoreValue.ToString();

        isStaticObject = false;

        CurType = type;
        switch (CurType)
        {
            case Type.None:
                BackGroundColor = Color.Blue;
                ForeGroundColor = Color.White;
                break;
            case Type.IncreasePlatform:
                BackGroundColor = Color.Green;
                ForeGroundColor = Color.White;
                break;
            case Type.DecreasePlatform:
                BackGroundColor = Color.Red;
                ForeGroundColor = Color.White;
                break;
            case Type.IncreaseBallSpeed:
                BackGroundColor = Color.Yellow;
                ForeGroundColor = Color.Black;
                break;
            case Type.ExtraScore:
                BackGroundColor = Color.Magenta;
                ForeGroundColor = Color.White;
                break;
        }

        Collision += HandleCollision;
    }

    private void CenterOrigin()
    {
        FloatRect bound = _content.GetLocalBounds();
        _content.Origin = new Vector2f(bound.Left + bound.Width / 2f, bound.Top + bound.Height / 2f);
    }

    public override void Update(Time elapsedTime)
    {
        Move(elapsedTime);
    }

    public override void HandleCollision(object? sender, CollisionEventArgs args)
    {
        if (Point1.Y > args.Height)
        {
            if (args.Container is GameField gameField)
            {
                gameField.RemoveDisplayObjectRequest(this);
            }

            return;
        }

        if (args.Object is Platform)
        {
            if (args.Container is GameField gameField)
            {
                gameField.RemoveDisplayObjectRequest(this);
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
        if (IsActive)
        {
            states.Transform *= Transform;
            target.Draw(_shape, states);
            target.Draw(_content, states);
        }
    }
}