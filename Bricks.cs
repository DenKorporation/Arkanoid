using SFML.System;

namespace Arkanoid;

public class Bricks
{
    public event EventHandler<EventArgs>? Cleared; 
    public Brick?[,]? FieldBricks { get; set; }

    public int RowNumbers => FieldBricks?.GetLength(0)?? -1;

    public int ColumnNumbers => FieldBricks?.GetLength(1) ?? -1;

    private int _destroyableCount { get; set; } = -1;
    private int DestroyableCount
    {
        get => _destroyableCount;
        set
        {
            _destroyableCount = value;
            if (_destroyableCount == 0)
            {
                Cleared?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void SetConfiguration(int rowNumbers, int columnNumbers, int destroyableCount)
    {
        FieldBricks = new Brick[rowNumbers, columnNumbers];
        DestroyableCount = destroyableCount;
    }

    public bool RemoveBrick(Brick brick)
    {
        for (int i = 0; i < FieldBricks?.GetLength(0); i++)
        {
            for (int j = 0; j < FieldBricks?.GetLength(1); j++)
            {
                if (FieldBricks[i, j] == brick)
                {
                    FieldBricks[i, j] = null;
                    DestroyableCount--;

                    brick.BonusItem.Velocity = new Vector2f(0f, BonusItem.BonusVelocity);
                    brick.BonusItem.IsActive = true;
                    brick.BonusItem = null;
                    return true;
                }
            }
        }

        return false;
    }
}