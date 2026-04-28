namespace GameLibrary.Models;

public class Player
{
    public int Id { get; set; }
    public string Nick { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Relacja M:N z Game (przez tabelę pośrednią PlayerGame)
    public ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();

    // Relacja self-referencing M:N (znajomi)
    public ICollection<Friend> Friends { get; set; } = new List<Friend>();
}
