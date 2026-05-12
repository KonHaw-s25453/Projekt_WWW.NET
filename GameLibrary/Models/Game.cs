namespace GameLibrary.Models;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }

    // Relacja M:N z Player (przez tabelę pośrednią PlayerGame)
    public ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();

    // Relacja M:N z Tag (przez tabelę pośrednią GameTag)
    public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();
}
