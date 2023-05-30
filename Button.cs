using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arkanoid;

public class Button : DisplayObject
{
    public event EventHandler<EventArgs> Clicked;

    public Label BtnContent;
    protected RectangleShape _backgroundShape;

    private bool _isSelected = false;

    public void Deselect()
    {
        _isSelected = false;
        SetNormalStyle();
    }
    
    public Button(Vector2f size)
    {
        isStaticObject = false;
        _backgroundShape = new RectangleShape(size);
        Point1 -= size / 2f;
        Point2 += size / 2f;
        _backgroundShape.Origin = size / 2f;
    }

    public void OnMouseClicked(object? sender, MouseButtonEventArgs args)
    {
        if (_isSelected && args.Button == Mouse.Button.Left)
        {
            Clicked?.Invoke(this, new EventArgs());
        }
    }

    public void OnMouseMoved(object? sender, MouseMoveEventArgs args)
    {
        
        if (args.X >= Point1.X && args.X <= Point2.X && args.Y >= Point1.Y && args.Y <= Point2.Y)
        {
            if (!_isSelected)
            {
                _isSelected = true;
                SetSelectedStyle();
            }
        }
        else if (_isSelected)
        {
            _isSelected = false;
            SetNormalStyle();
        }
    }

    public void SetSelectedStyle()
    {
        BtnContent.ForeGroundColor = ForeSelectedColor;
        _backgroundShape.FillColor = BackSelectedColor;
    }

    public void SetNormalStyle()
    {
        BtnContent.ForeGroundColor = ForeGroundColor;
        _backgroundShape.FillColor = BackGroundColor;
    }
    
    public Color ForeSelectedColor { get; set; }
    public Color BackSelectedColor { get; set; }
    public override Color ForeGroundColor { get; set; }
    public override Color BackGroundColor { get; set; }

    public override void Update(Time elapsedTime)
    {
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
        target.Draw(_backgroundShape, states);
        target.Draw(BtnContent, states);
    }
}