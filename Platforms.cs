namespace Arkanoid;

public class Platforms
{
    public List<Platform> PlatformsList = new ();

    public void AddPlatform(Platform platform)
    {
        PlatformsList.Add(platform);
    }

    public bool RemovePlatform(Platform platform)
    {
        return PlatformsList.Remove(platform);
    }

    public void Clear()
    {
        PlatformsList.Clear();
    }
}