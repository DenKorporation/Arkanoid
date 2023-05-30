namespace Arkanoid;

public class Players
{
    public List<Player> PlayersRating = new();

    public void AddPlayer(Player player)
    {
        PlayersRating.Add(player);
        PlayersRating.Sort();
    }
    
    public bool RemovePlayer(Player player)
    {
        return PlayersRating.Remove(player);
    }
}