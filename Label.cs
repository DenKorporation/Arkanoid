using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Label : DisplayObject
{
    public enum Alignment
    {
        Left,
        Center,
        Right
    }

    private Alignment _curAlignment = Alignment.Center;

    public Alignment CurAlignment
    {
        get => _curAlignment;
        set
        {
            _curAlignment = value;
            CenterOrigin();
        }
    }

    private String? Text
    {
        get => _lblText?.DisplayedString;
        set
        {
            if (_lblText is not null)
            {
                _lblText.DisplayedString = value;
                CenterOrigin();
            }
        }
    }

    private readonly Text? _lblText;

    public Label(String text, int size, Font? font)
    {
        if (font is not null)
        {
            _lblText = new Text(text, font);
            _lblText.CharacterSize = (uint)size;
        }

        CenterOrigin();
    }

    public void SetText(string text)
    {
        if (_lblText is not null)
        {
            _lblText.DisplayedString = text;
            CenterOrigin();
        }
    }

    private void CenterOrigin()
    {
        if (_lblText is not null)
        {
            FloatRect bound = _lblText.GetLocalBounds();
            switch (CurAlignment)
            {
                case Alignment.Left:
                    _lblText.Origin = new Vector2f(0f, bound.Top + bound.Height / 2f);
                    break;
                case Alignment.Center:
                    _lblText.Origin = new Vector2f(bound.Left + bound.Width / 2f, bound.Top + bound.Height / 2f);
                    break;
                case Alignment.Right:
                    _lblText.Origin = new Vector2f(bound.Left + bound.Width, bound.Top + bound.Height / 2f);
                    break;
            }
        }
    }

    public override Color ForeGroundColor
    {
        get => _lblText?.FillColor ?? Color.White;
        set
        {
            if (_lblText is not null)
            {
                _lblText.FillColor = value;
            }
        }
    }

    public override Color BackGroundColor
    {
        get => _lblText?.OutlineColor ?? Color.White;
        set
        {
            if (_lblText is not null)
            {
                _lblText.OutlineColor = value;
            }
        }
    }

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
        target.Draw(_lblText, states);
    }
}