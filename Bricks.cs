namespace Arkanoid;

public class Bricks
{
    public Brick[,] FieldBricks;
    private BonusItems _bonusItems;

    public int RowNumbers
    {
        get
        {
            return FieldBricks.GetLength(0);
        }
    }

    public int _columnNumbers
    {
        get
        {
            return FieldBricks.GetLength(1);
        }
    }


    public Bricks(int rowNumbers, int columnNumbers)
    {
        FieldBricks = new Brick[rowNumbers, columnNumbers];
    }
}