namespace GameLibrary.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Relacja M:N z Game z ukrytą tabelą pośrednią
    public ICollection<Game> Games { get; set; } = new List<Game>();
}
