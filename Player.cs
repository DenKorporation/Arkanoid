using SFML.System;
using SFML.Window;

namespace Arkanoid;

public class Player : IComparable
{
    public static float PlayerVelocity = 500f;
    
    public string Name { get; set; }

    public Statistics Statistics { get; } = new ();

    public Platform? Platform { get; set; }

    public Player(string name)
    {
        Name = name;
    }

    public void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        if (Platform is not null)
        {
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
    }

    public void OnKeyReleased(object? sender, KeyEventArgs e)
    {
        if (Platform is not null)
        {
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

    public int CompareTo(object? obj)
    {
        if (obj is Player player) return Statistics.BestScore.CompareTo(player.Statistics.BestScore);
        else throw new ArgumentException();
    }
}