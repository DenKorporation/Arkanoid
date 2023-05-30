using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Arkanoid;

public class MessageBox : DisplayObject
{
    public event EventHandler<MouseButtonEventArgs> MouseClicked;
    public event EventHandler<MouseMoveEventArgs> MouseMoved;
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

    public bool IsOpen { get; set; }

    public Label MessageContent;
    public List<Button> Buttons = new ();
    public RectangleShape Background = new ();
    
    
    public override Color ForeGroundColor { get; set; }
    public override Color BackGroundColor { get; set; }

    public void AddButton(Button btn)
    {
        Buttons.Add(btn);
        MouseMoved += btn.OnMouseMoved;
        MouseClicked += btn.OnMouseClicked;
    }
    
    public void Clear()
    {
        foreach (var btn in Buttons)
        {
            MouseMoved -= btn.OnMouseMoved;
            MouseClicked -= btn.OnMouseClicked;   
        }
        Buttons.Clear();
        
    }

    public override void Update(Time elapsedTime)
    {
        throw new NotImplementedException();
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
        target.Draw(Background, states);
        target.Draw(MessageContent, states);

        foreach (var btn in Buttons)
        {
            target.Draw(btn, states);
        }
    }
}