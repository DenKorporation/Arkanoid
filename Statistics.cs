namespace Arkanoid;

public class Statistics
{
    public event EventHandler<EventArgs>? ScoreChanged; 

    private int _currentScore;
    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            if (_currentScore > BestScore)
            {
                BestScore = CurrentScore;
            }
            ScoreChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int BestScore { get; set; }
}