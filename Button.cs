using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class Button : DisplayObject
{
    public delegate void BtnAction();
    
    protected Label _btnContent;
    protected BtnAction _btnAction;

    public void OnClickedAction()
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