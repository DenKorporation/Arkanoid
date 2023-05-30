using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arkanoid;

public class InputText : Transformable, Drawable
{
    public event EventHandler<TextEventArgs>? TextEntered;

    private readonly RectangleShape _backgroundShape = new();
    private readonly Text? _content;

    public string Content
    {
        get => _content?.DisplayedString ?? "";
        set
        {
            if (_content is not null)
            {
                _content.DisplayedString = value;
                SetOrigin();
            }
        }
    }

    public Color ForeGroundColor
    {
        get => _content?.FillColor ?? Color.White;
        set
        {
            if (_content is not null)
            {
                _content.FillColor = value;
            }
        }
        
    }

    public Color BackGroundColor
    {
        get => _content?.FillColor ?? Color.White;
        set
        {
            if (_content is not null)
            {
                _content.FillColor = value;
            }
        }
    }

    public InputText(Vector2f size, uint fontSize, Font? font)
    {
        _content = new Text("", font, fontSize);
        _backgroundShape.Size = size;
        _backgroundShape.Origin = size / 2f;
    }

    private void SetOrigin()
    {
        if (_content is not null)
        {
            FloatRect bound = _content.GetLocalBounds();
            _content.Origin = new Vector2f(bound.Left + bound.Width / 2f, bound.Top + bound.Height / 2f);
        }
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(_backgroundShape, states);
        target.Draw(_content, states);
    }

    public void OnTextEntered(object? sender, TextEventArgs e)
    {
        TextEntered?.Invoke(sender, e);
    }
}