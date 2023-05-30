using SFML.Graphics;
using SFML.Window;

namespace Arkanoid;

public class Menu: Transformable, Drawable
{
    public event EventHandler<MouseButtonEventArgs>? MouseClicked;
    public event EventHandler<MouseMoveEventArgs>? MouseMoved;
    public void OnMouseClicked(object? sender, MouseButtonEventArgs args){
        if (IsOpen)
        {
            MouseClicked?.Invoke(sender, args);
        }
    }

    public void OnMouseMoved(object? sender, MouseMoveEventArgs args)
    {
        if (IsOpen)
        {
            MouseMoved?.Invoke(sender, args);   
        }
    }
    
    public readonly List<MenuItem> MenuItems = new ();
    
    public bool IsOpen { get; set; }

    public void AddItem(MenuItem item)
    {
        MenuItems.Add(item);
        MouseMoved += item.OnMouseMoved;
        MouseClicked += item.OnMouseClicked;
    }

    public void Clear()
    {
        foreach (var item in MenuItems)
        {
            MouseMoved -= item.OnMouseMoved;
            MouseClicked -= item.OnMouseClicked;   
        }
        MenuItems.Clear();
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        foreach (var item in MenuItems)
        {
            target.Draw(item, states);
        }
    }
}