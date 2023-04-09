namespace Arkanoid;

public class Balls
{
    private List<Ball> _balls = new ();

    public void AddBall(Ball ball)
    {
        _balls.Add(ball);
    }

    public bool RemoveBall(Ball ball)
    {
        return _balls.Remove(ball);
    }

    public bool LaunchBall()
    {
        if (_balls.Count == 1)
        {
            _balls[0].Launch();
            return true;
        }

        return false;
    }
    
}