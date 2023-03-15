namespace Arkanoid;

public class Button : DisplayObject
{
    public delegate void BtnAction();
    
    private string _btnContent;
    private BtnAction _btnAction;
    
    public Button(string btnContent, BtnAction btnAction)
    {
        _btnContent = btnContent;
        _btnAction = btnAction;

    }

    public void OnClickedAction()
    {
        throw new NotImplementedException();
    }
    
    public override void Draw()
    {
        throw new NotImplementedException();
    }
}