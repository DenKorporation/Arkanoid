using SFML.Graphics;
using SFML.System;

namespace Arkanoid;

public class GameField : Transformable, Drawable
{
    public float Width { get; set; }
    public float Height { get; set; }

    private DisplayContainer AllObjects = new();
    private DisplayContainer _staticObjects = new();
    private DisplayContainer _dynamicObjects = new();

    private Platforms _platforms = new ();
    private Bricks _bricks;
    private Menu _menu;
    private Balls _balls = new ();
    private MessageBox _messageBox;
    private BonusItems _bonusItems;
    private StatusBar _statusBar;

    public GameField(Vector2f windowsSize, Player currentPlayer)
    {
        Width = windowsSize.X;
        Height = windowsSize.Y;

        _bricks = new Bricks(10, 20);
        
        float bricksUpBorder = 0.15f * Height;
        Vector2f brickSize = new Vector2f(Width / 20f, 0.05f * Height);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Brick brick = new Brick(brickSize);
                brick.RefPoint = new Vector2f(brickSize.X * j, bricksUpBorder + brickSize.Y * i) + brickSize / 2f;
                _bricks.FieldBricks[i, j] = brick;
                AllObjects.AddDisplayObject(brick);
                _staticObjects.AddDisplayObject(brick);
            }
        }

        Platform platform = new Platform(new Vector2f(Width / 10f, 15));
        platform.RefPoint = new Vector2f(Width / 2f, 0.95f * Height);
        _platforms.AddPlatform(platform);
        AllObjects.AddDisplayObject(platform);
        _dynamicObjects.AddDisplayObject(platform);
        currentPlayer.Platform = platform;

        Ball ball = new Ball(20);
        ball.RefPoint = new Vector2f(Width / 2f, 0.9f * Height);
        _balls.AddBall(ball);
        AllObjects.AddDisplayObject(ball);
        _dynamicObjects.AddDisplayObject(ball);
    }

    public void StartGame()
    {
        _balls.LaunchBall();
    }

    private void MoveObject(Time elapsedTime)
    {
        _dynamicObjects.Update(elapsedTime);
    }

    private void CheckCollisons()
    {
        foreach (var obj in _dynamicObjects.Objects)
        {
            if (obj is Platform platform)
            {
                if (platform.Point1.X < 0)
                {
                    platform.RefPoint = new Vector2f(platform.Width / 2f, platform.RefPoint.Y);
                    continue;
                }

                if (platform.Point2.X > Width)
                {
                    platform.RefPoint = new Vector2f(Width - platform.Width / 2f, platform.RefPoint.Y);
                    continue;
                }
            }
            if (obj is Ball ball)
            {
                if (ball.Point1.X < 0)
                {
                    ball.RefPoint = new Vector2f(ball.Width / 2f, ball.RefPoint.Y);
                    ball.Velocity = new Vector2f(-ball.Velocity.X, ball.Velocity.Y);
                    continue;
                }

                if (ball.Point2.X > Width)
                {
                    ball.RefPoint = new Vector2f(Width - ball.Width / 2f, ball.RefPoint.Y);
                    ball.Velocity = new Vector2f(-ball.Velocity.X, ball.Velocity.Y);
                    continue;
                }
                if (ball.Point1.Y < 0)
                {
                    ball.RefPoint = new Vector2f(ball.RefPoint.X, ball.Height / 2f);
                    ball.Velocity = new Vector2f(ball.Velocity.X, -ball.Velocity.Y);
                    continue;
                }

                if (ball.Point2.Y > Height)
                {
                    //delete ball
                }
            }
        }
    }

    public void Update(Time elapsedTime)
    {
        MoveObject(elapsedTime);
        CheckCollisons();
    }
    public void Draw(RenderTarget target, RenderStates states)
    {
        states.Transform *= Transform;
        target.Draw(AllObjects, states);
    }
}