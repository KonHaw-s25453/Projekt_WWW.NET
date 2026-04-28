namespace GameLibrary.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Relacja M:N z Game (przez tabelę pośrednią GameTag)
    public ICollection<GameTag> GameTags { get; set; } = new List<GameTag>();
}
