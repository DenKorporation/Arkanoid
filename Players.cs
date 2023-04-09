using System.Reflection;
using SFML.Window;

namespace Arkanoid;

public class Players
{
    private List<Player> _players = new();
    public event EventHandler<KeyEventArgs> KeyPressed = null;
    public event EventHandler<KeyEventArgs> KeyReleased = null;

    public Players()
    {
    }

    public void AddPlayer(Player player)
    {
        _players.Add(player);
        KeyPressed += player.OnKeyPressed;
        KeyReleased += player.OnKeyReleased;
    }
    
    public bool RemovePlayer(Player player)
    {
        return _players.Remove(player);
    }

    public void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        Console.WriteLine(GetType() + ":" + MethodInfo.GetCurrentMethod().Name);
        KeyPressed(sender, e);
    }

    public void OnKeyReleased(object? sender, KeyEventArgs e)
    {
        Console.WriteLine(GetType() + ":" + MethodInfo.GetCurrentMethod().Name);
        KeyReleased(sender, e);
    }
}