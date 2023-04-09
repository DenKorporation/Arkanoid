namespace Arkanoid;

public class Platforms
{
    private List<Platform> _platforms = new ();

    public void AddPlatform(Platform platform)
    {
        _platforms.Add(platform);
    }

    public bool RemovePlatform(Platform platform)
    {
        return _platforms.Remove(platform);
    }
}