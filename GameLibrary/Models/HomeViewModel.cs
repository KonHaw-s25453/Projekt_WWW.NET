namespace GameLibrary.Models;

public class HomeViewModel
{
    public IEnumerable<Player> Players { get; set; } = new List<Player>();
    public IEnumerable<Game> Games { get; set; } = new List<Game>();
}
