using SFML.System;

namespace Arkanoid;

public class MenuItem : Button
{
    private Label _content;

    public MenuItem(Vector2f size) : base(size)
    {
    }
}