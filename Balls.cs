namespace Arkanoid;

public class Balls
{
    public List<Ball> BallsList { get; set; } = new ();

    public int Length
    {
        get { return BallsList.Count; }
    }

    public void AddBall(Ball ball)
    {
        BallsList.Add(ball);
    }

    public bool RemoveBall(Ball ball)
    {
        return BallsList.Remove(ball);
    }

    public void Clear()
    {
        BallsList.Clear();
    }
    
    public bool LaunchBall()
    {
        if (Length == 1)
        {
            BallsList[0].Launch();
            return true;
        }

        return false;
    }
}