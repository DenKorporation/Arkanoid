﻿namespace Arkanoid;

public class Button : DisplayObject
{
    public delegate void BtnAction();
    
    protected Label _btnContent;
    protected BtnAction _btnAction;

    public void OnClickedAction()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public override void Collision()
    {
        throw new NotImplementedException();
    }

    public override void Draw()
    {
        throw new NotImplementedException();
    }
}