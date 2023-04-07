namespace Arkanoid;

public class GameField
{
    public int Width { get; set; }
    public int Height { get; set; }

    private DisplayContainer AllObjects;
    private DisplayContainer _staticObjects;
    private DisplayContainer _dynamicObjects;

    private Platforms _platforms;
    private Bricks _bricks;
    private Menu _menu;
    private Balls _balls;
    private MessageBox _messageBox;
    private BonusItems _bonusItems;
    private StatusBar _statusBar;

    
    public void Draw()
    {}
    
    public void MoveObject(long elapsedMillisecond)
    {
    }

    public void CheckCollisons()
    {
    }

    public void HandleCollison()
    {
    }
    
}