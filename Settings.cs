using SFML.System;

namespace Arkanoid;

public class Settings
{
    public enum Difficulty
    {
        VeryEasy,
        Easy,
        Medium,
        Hard,
        Impossible
    }

    public Vector2u Resolution = new (1600, 900);
    public Difficulty CurrentDifficulty = Difficulty.VeryEasy;
    public string PlayerName = "";
}