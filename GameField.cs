using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class GameField : Transformable, Drawable
{
    public event EventHandler<EventArgs>? GameOver;
    public event EventHandler<EventArgs>? NextLevel;

    public float Width { get; private set; }
    public float Height { get; private set; }

    private readonly DisplayContainer _allObjects = new();
    private readonly DisplayContainer _staticObjects = new();
    private readonly DisplayContainer _dynamicObjects = new();

    private Platforms _platforms = new();
    private Bricks _bricks = new();
    private Balls _balls = new();
    private BonusItems _bonusItems = new();

    public Player? CurPlayer;
    public Font? Font;

    private int _curLevel = 1;

    public int CurLevel
    {
        get => _curLevel;
        set
        {
            _curLevel = value;
            BuildField(new Vector2f(Width, Height), CurPlayer!);
        }
    }

    private Settings.Difficulty _difficulty = Settings.Difficulty.Easy;

    public Settings.Difficulty Difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            BuildField(new Vector2f(Width, Height), CurPlayer!);
        }
    }


    private List<DisplayObject> _removeQueue = new();
    private bool _gameIsStarted;

    public GameField()
    {
        _bricks.Cleared += (_, _) => NextLevel?.Invoke(this, EventArgs.Empty);
    }

    public void BuildField(Vector2f fieldSize, Player currentPlayer)
    {
        CurPlayer = currentPlayer;
        Width = fieldSize.X;
        Height = fieldSize.Y;

        _gameIsStarted = false;
        _allObjects.Objects.Clear();
        _dynamicObjects.Objects.Clear();
        _staticObjects.Objects.Clear();
        _balls.Clear();
        _platforms.Clear();

        Player.PlayerVelocity = Width * 0.6f;
        BonusItem.BonusVelocity = Width * 0.2f;

        int healthPoint = 0;
        int Score = 0;
        int extraScore = 0;
        switch (Difficulty)
        {
            case Settings.Difficulty.VeryEasy:
                healthPoint = 100;
                Score = 10;
                extraScore = 30;
                Ball.BallVelocity = Width * 0.15f;
                break;
            case Settings.Difficulty.Easy:
                healthPoint = 100;
                Score = 20;
                extraScore = 50;
                Ball.BallVelocity = Width * 0.2f;
                break;
            case Settings.Difficulty.Medium:
                healthPoint = 200;
                Score = 30;
                extraScore = 75;
                Ball.BallVelocity = Width * 0.2f;
                break;
            case Settings.Difficulty.Hard:
                healthPoint = 200;
                Score = 40;
                extraScore = 100;
                Ball.BallVelocity = Width * 0.25f;
                break;
            case Settings.Difficulty.Impossible:
                healthPoint = 300;
                Score = 50;
                extraScore = 150;
                Ball.BallVelocity = Width * 0.3f;
                break;
        }

        Random rnd = new Random();

        switch (CurLevel)
        {
            case 1:
            {
                _bricks.SetConfiguration(2, 8, 16);

                float bricksUpBorder = 0.3f * Height;
                float bricksLeftBorder = 0.1f * Width;
                Vector2f brickSize = new Vector2f(Width / 10f, 0.1f * Height);
                for (int i = 0; i < _bricks.RowNumbers; i++)
                {
                    for (int j = 0; j < _bricks.ColumnNumbers; j++)
                    {
                        Brick brick = new Brick(brickSize, healthPoint);
                        brick.RefPoint = new Vector2f(bricksLeftBorder + brickSize.X * j,
                                             bricksUpBorder + brickSize.Y * i) +
                                         brickSize / 2f;

                        _bricks.FieldBricks![i, j] = brick;
                        _allObjects.AddDisplayObject(brick);
                        _staticObjects.AddDisplayObject(brick);
                    }
                }

                break;
            }
            case 2:
            {
                _bricks.SetConfiguration(4, 8, 24);

                float bricksUpBorder = 0.2f * Height;
                float bricksLeftBorder = 0.1f * Width;
                Vector2f brickSize = new Vector2f(Width / 10f, 0.1f * Height);
                for (int i = 0; i < _bricks.RowNumbers; i++)
                {
                    for (int j = 0; j < _bricks.ColumnNumbers; j++)
                    {
                        Brick brick = new Brick(brickSize, healthPoint);
                        brick.RefPoint = new Vector2f(bricksLeftBorder + brickSize.X * j,
                                             bricksUpBorder + brickSize.Y * i) +
                                         brickSize / 2f;
                        _bricks.FieldBricks![i, j] = brick;
                        if (j == 0 || j == 7)
                        {
                            brick.IsDestroyable = false;
                            brick.ForeGroundColor = new Color(60, 60, 60);
                        }

                        _allObjects.AddDisplayObject(brick);
                        _staticObjects.AddDisplayObject(brick);
                    }
                }

                break;
            }
            case 3:
            {
                _bricks.SetConfiguration(6, 8, 20);

                float bricksUpBorder = 0.2f * Height;
                float bricksLeftBorder = 0.1f * Width;
                Vector2f brickSize = new Vector2f(Width / 10f, 0.07f * Height);
                for (int i = 0; i < _bricks.RowNumbers; i++)
                {
                    for (int j = 0; j < _bricks.ColumnNumbers; j++)
                    {
                        Brick brick = new Brick(brickSize, healthPoint);
                        brick.RefPoint = new Vector2f(bricksLeftBorder + brickSize.X * j,
                                             bricksUpBorder + brickSize.Y * i) +
                                         brickSize / 2f;
                        _bricks.FieldBricks![i, j] = brick;
                        if (j == 0 || j == 7 || (i == 5 && (j <= 2 || j >= 5)))
                        {
                            brick.IsDestroyable = false;
                            brick.ForeGroundColor = new Color(60, 60, 60);
                        }

                        _allObjects.AddDisplayObject(brick);
                        _staticObjects.AddDisplayObject(brick);
                    }
                }

                break;
            }
            case 4:
            {
                _bricks.SetConfiguration(10, 15, 100);

                float bricksUpBorder = 0.15f * Height;
                Vector2f brickSize = new Vector2f(Width / 15f, 0.05f * Height);
                for (int i = 0; i < _bricks.RowNumbers; i++)
                {
                    for (int j = 0; j < _bricks.ColumnNumbers; j++)
                    {
                        Brick brick = new Brick(brickSize, healthPoint);
                        brick.RefPoint = new Vector2f(brickSize.X * j, bricksUpBorder + brickSize.Y * i) +
                                         brickSize / 2f;
                        _bricks.FieldBricks![i, j] = brick;
                        if (j == 0 || j == 4 || j == 7 || j == 10 || j == 14)
                        {
                            brick.IsDestroyable = false;
                            brick.ForeGroundColor = new Color(60, 60, 60);
                        }

                        _allObjects.AddDisplayObject(brick);
                        _staticObjects.AddDisplayObject(brick);
                    }
                }

                break;
            }
            default:
            {
                _bricks.SetConfiguration(10, 15, 105);

                float bricksUpBorder = 0.15f * Height;
                Vector2f brickSize = new Vector2f(Width / 15f, 0.05f * Height);
                for (int i = 0; i < _bricks.RowNumbers; i++)
                {
                    for (int j = 0; j < _bricks.ColumnNumbers; j++)
                    {
                        Brick brick = new Brick(brickSize, healthPoint);
                        brick.RefPoint = new Vector2f(brickSize.X * j, bricksUpBorder + brickSize.Y * i) +
                                         brickSize / 2f;
                        _bricks.FieldBricks![i, j] = brick;
                        if (i == 0 || (i == 9 && j != 7) || j == 0 || j == 14)
                        {
                            brick.IsDestroyable = false;
                            brick.ForeGroundColor = new Color(60, 60, 60);
                        }

                        _allObjects.AddDisplayObject(brick);
                        _staticObjects.AddDisplayObject(brick);
                    }
                }

                break;
            }
        }

        foreach (var brick in _bricks.FieldBricks)
        {
            BonusItem.Type type = (BonusItem.Type)rnd.Next(0, 5);
            BonusItem bonusItem = new BonusItem(brick.Size.Y / 3f, type,
                (type == BonusItem.Type.ExtraScore) ? Score + extraScore : Score, Font);
            bonusItem.RefPoint = brick.RefPoint;
            brick.BonusItem = bonusItem;
            
            _allObjects.AddDisplayObject(bonusItem);
            _dynamicObjects.AddDisplayObject(bonusItem);
            _bonusItems.BonusList.Add(bonusItem);
        }

        Platform platform = new Platform(new Vector2f(Width / 8, 15));
        platform.RefPoint = new Vector2f(Width / 2f, 0.95f * Height);
        _platforms.AddPlatform(platform);
        _allObjects.AddDisplayObject(platform);
        _dynamicObjects.AddDisplayObject(platform);
        CurPlayer.Platform = platform;
        
        platform.BonusPickedUp += PlatformOnBonusPickedUp; 

        Ball ball = new Ball(Height / 40f);
        ball.RefPoint = new Vector2f(Width / 2f, 0.9f * Height);
        _balls.AddBall(ball);
        _allObjects.AddDisplayObject(ball);
        _dynamicObjects.AddDisplayObject(ball);
    }

    private void PlatformOnBonusPickedUp(object? sender, Platform.BonusPickedUpArgs e)
    {
        switch (e.Type)
        {
            case BonusItem.Type.IncreasePlatform:
                foreach (var platform in _platforms.PlatformsList)
                {
                    platform.Size = new Vector2f(Width / 6f, platform.Size.Y);
                }
                break;
            case BonusItem.Type.DecreasePlatform:
                foreach (var platform in _platforms.PlatformsList)
                {
                    platform.Size = new Vector2f(Width / 10f, platform.Size.Y);
                }
                break;
            case BonusItem.Type.IncreaseBallSpeed:
                Ball.BallVelocity *= 1.2f;
                foreach (var ball in _balls.BallsList)
                {
                    ball.Velocity *= 1.2f;
                }
                break;
            
        }

        if (CurPlayer != null) CurPlayer.Statistics.CurrentScore += e.Score;
    }

    public void RebuildField(Vector2f fieldSize, Player currentPlayer)
    {
        Vector2f oldSize = new Vector2f(Width, Height);
        Vector2f resizeFactor = new Vector2f(fieldSize.X / oldSize.X, fieldSize.Y / oldSize.Y);

        CurPlayer = currentPlayer;
        Width = fieldSize.X;
        Height = fieldSize.Y;

        Player.PlayerVelocity = Width * 0.6f;
        BonusItem.BonusVelocity = Width * 0.2f;

        switch (Difficulty)
        {
            case Settings.Difficulty.VeryEasy:
                Ball.BallVelocity = Width * 0.15f;
                break;
            case Settings.Difficulty.Easy:
                Ball.BallVelocity = Width * 0.2f;
                break;
            case Settings.Difficulty.Medium:
                Ball.BallVelocity = Width * 0.2f;
                break;
            case Settings.Difficulty.Hard:
                Ball.BallVelocity = Width * 0.25f;
                break;
            case Settings.Difficulty.Impossible:
                Ball.BallVelocity = Width * 0.3f;
                break;
        }

        float bricksUpBorder;
        float bricksLeftBorder = 0f;
        Vector2f brickSize = new Vector2f(0f, 0f);

        switch (CurLevel)
        {
            case 1:
            {
                bricksUpBorder = 0.3f * Height;
                bricksLeftBorder = 0.1f * Width;
                brickSize = new Vector2f(Width / 10f, 0.1f * Height);
                break;
            }
            case 2:
            {
                bricksUpBorder = 0.2f * Height;
                bricksLeftBorder = 0.1f * Width;
                brickSize = new Vector2f(Width / 10f, 0.1f * Height);
                break;
            }
            case 3:
            {
                bricksUpBorder = 0.2f * Height;
                bricksLeftBorder = 0.1f * Width;
                brickSize = new Vector2f(Width / 10f, 0.07f * Height);
                break;
            }
            case 4:
            {
                bricksUpBorder = 0.15f * Height;
                brickSize = new Vector2f(Width / 15f, 0.05f * Height);
                break;
            }
            default:
            {
                bricksUpBorder = 0.15f * Height;
                brickSize = new Vector2f(Width / 15f, 0.05f * Height);
                break;
            }
        }

        for (int i = 0; i < _bricks.RowNumbers; i++)
        {
            for (int j = 0; j < _bricks.ColumnNumbers; j++)
            {
                if (_bricks.FieldBricks[i, j] is not null)
                {
                    _bricks.FieldBricks[i, j].Size = brickSize;
                    _bricks.FieldBricks[i, j].RefPoint = new Vector2f(bricksLeftBorder + brickSize.X * j,
                                                             bricksUpBorder + brickSize.Y * i) +
                                                         brickSize / 2f;
                }
            }
        }


        foreach (var bonusItem in _bonusItems.BonusList)
        {
            bonusItem.ShapeRadius *= resizeFactor.X;
            bonusItem.RefPoint = new Vector2f(bonusItem.RefPoint.X * resizeFactor.X, bonusItem.RefPoint.Y * resizeFactor.Y);
        }

        foreach (var obj in _platforms.PlatformsList)
        {
            obj.Size = new Vector2f(obj.Size.X * resizeFactor.X, 15);
            obj.RefPoint = new Vector2f(obj.RefPoint.X * resizeFactor.X, 0.95f * Height);
        }


        foreach (var obj in _balls.BallsList)
        {
            obj.ShapeRadius = Height / 40f;
            obj.RefPoint = new Vector2f(obj.RefPoint.X * resizeFactor.X, obj.RefPoint.Y * resizeFactor.Y);
            obj.Velocity *= resizeFactor.X;
        }
    }


    public void StartGame()
    {
        if (!_gameIsStarted)
        {
            if (_balls.Length == 0)
            {
                Ball ball = new Ball(Height / 40f);
                ball.RefPoint = new Vector2f(Width / 2f, 0.9f * Height);
                _balls.AddBall(ball);
                _allObjects.AddDisplayObject(ball);
                _dynamicObjects.AddDisplayObject(ball);
            }

            _balls.LaunchBall();
            _gameIsStarted = true;
        }
    }

    public void RemoveDisplayObjectRequest(DisplayObject displayObject)
    {
        _removeQueue.Add(displayObject);
    }

    private void MoveObject(Time elapsedTime)
    {
        _dynamicObjects.Update(elapsedTime);
    }

    private void CheckCollisions()
    {
        foreach (var obj in _dynamicObjects.Objects)
        {
            if (obj.CheckCollision(this, null))
            {
                obj.OnCollision(new CollisionEventArgs(Height, Width, this));
            }

            foreach (var displayObj in _allObjects.Objects)
            {
                if (obj != displayObj && obj.CheckCollision(null, displayObj))
                {
                    obj.OnCollision(new CollisionEventArgs(Height, Width, this, displayObj));
                    if (displayObj.isStaticObject)
                    {
                        displayObj.OnCollision(new CollisionEventArgs(Height, Width, this, obj));
                    }

                    break;
                }
            }
        }

        foreach (var obj in _removeQueue)
        {
            if (obj is Ball ballObj)
            {
                _balls.RemoveBall(ballObj);
                _gameIsStarted = _balls.Length != 0;

                if (_balls.Length == 0)
                {
                    GameOver?.Invoke(this, new());
                }
            }
            else if (obj is Brick brickObj)
            {
                _bricks.RemoveBrick(brickObj);
            }
            else if (obj is BonusItem bonusItem)
            {
                _bonusItems.BonusList.Remove(bonusItem);
            }

            if (obj.isStaticObject)
            {
                _staticObjects.RemoveDisplayObject(obj);
            }
            else
            {
                _dynamicObjects.RemoveDisplayObject(obj);
            }

            _allObjects.RemoveDisplayObject(obj);
        }

        _removeQueue.Clear();
    }

    public void Update(Time elapsedTime)
    {
        MoveObject(elapsedTime);
        CheckCollisions();
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(_allObjects, states);
    }
}