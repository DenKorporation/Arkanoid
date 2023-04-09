namespace Arkanoid;

class App
{
    public static void Main(string[] args)
    {
        try
        {
            Game game = new Game();
            game.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}