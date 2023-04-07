namespace Arkanoid;

public class Player
{
    public int Id { get; init; }

    private Statistics _statistics;
    private String _name;

    private Platform _platform;

    public Statistics GetStatistics()
    {
        throw new NotImplementedException();
    }

    public void ProccessGameAction()
    {
        throw new NotImplementedException();
    }

}