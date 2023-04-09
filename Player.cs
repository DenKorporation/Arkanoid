using System.Reflection;
using SFML.System;
using SFML.Window;

namespace Arkanoid;

public class Player
{
    private const float PlayerVelocity = 500f;

    public string Id { get; }
    public string Name { get; }

    public Statistics Statistics { get; }

    public Platform Platform { get; set; }

    public Player(string name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }

    public void ProccessGameAction()
    {
        throw new NotImplementedException();
    }

    public void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        Console.WriteLine(GetType() + ":" + MethodInfo.GetCurrentMethod().Name);
        switch (e.Code)
        {
            case Keyboard.Key.A:
                Platform.Velocity += new Vector2f(-PlayerVelocity, 0);
                break;
            case Keyboard.Key.D:
                Platform.Velocity += new Vector2f(PlayerVelocity, 0);
                break;
        }
    }

    public void OnKeyReleased(object? sender, KeyEventArgs e)
    {
        Console.WriteLine(GetType() + ":" + MethodInfo.GetCurrentMethod().Name);
        switch (e.Code)
        {
            case Keyboard.Key.A:
                Platform.Velocity += new Vector2f(PlayerVelocity, 0);
                break;
            case Keyboard.Key.D:
                Platform.Velocity += new Vector2f(-PlayerVelocity, 0);
                break;
        }
    }
}