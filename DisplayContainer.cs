using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class DisplayContainer : Transformable, Drawable
{
    public List<DisplayObject> Objects = new();

    public void AddDisplayObject(DisplayObject obj)
    {
        Objects.Add(obj);
    }

    public void Update(Time elapsedTime)
    {
        foreach (var obj in Objects)
        {
            obj.Update(elapsedTime);
        }
    }

    public bool RemoveDisplayObject(DisplayObject obj)
    {
        return Objects.Remove(obj);
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        foreach (var obj in Objects)
        {
            target.Draw(obj,states);
        }
    }
}