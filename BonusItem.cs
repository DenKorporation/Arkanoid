using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class BonusItem : DisplayObject
{
    private Label _content;
    public enum Type
    {
        
    }
    private Type _type;
    private int _value;
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