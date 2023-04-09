using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Label : DisplayObject
{
    private String text;
    private int size;

    public void Show()
    {
        throw new NotImplementedException();
    }

    public void Hide()
    {
        throw new NotImplementedException();
    }

    public override Color ForeGroundColor { get; set; }
    public override Color BackGroundColor { get; set; }

    public override void Update(Time elapsedTime)
    {
        throw new NotImplementedException();
    }

    public override void Draw(RenderTarget target, RenderStates states)
    {
        throw new NotImplementedException();
    }
}