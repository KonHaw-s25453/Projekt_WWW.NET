namespace GameLibrary.Models;

// Tabela pośrednia dla relacji M:N Player <-> Game
public class PlayerGame
{
    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    // Dodatkowe pole w tabeli pośredniej
    public int HoursPlayed { get; set; }
}
