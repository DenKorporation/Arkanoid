using SFML.Graphics;
using SFML.Window;

namespace Arkanoid;

public class StatusBar : Transformable, Drawable
{
    public event EventHandler<MouseButtonEventArgs> MouseClicked;
    public event EventHandler<MouseMoveEventArgs> MouseMoved;

    public void OnMouseClicked(object? sender, MouseButtonEventArgs args)
    {
        MouseClicked?.Invoke(sender, args);
    }

    public void OnMouseMoved(object? sender, MouseMoveEventArgs args)
    {
        MouseMoved?.Invoke(sender, args);
    }

    public List<Label> Labels = new();

    public List<Button> Buttons = new();

    public void AddButton(Button btn)
    {
        Buttons.Add(btn);
        MouseClicked += btn.OnMouseClicked;
        MouseMoved += btn.OnMouseMoved;
    }

    public void Clear()
    {
        foreach (var btn in Buttons)
        {
            MouseClicked -= btn.OnMouseClicked;
            MouseMoved -= btn.OnMouseMoved;
        }

        Buttons.Clear();
        Labels.Clear();
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        foreach (var label in Labels)
        {
            target.Draw(label, states);
        }

        foreach (var button in Buttons)
        {
            target.Draw(button, states);
        }
    }
}