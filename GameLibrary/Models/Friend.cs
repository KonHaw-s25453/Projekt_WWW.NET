namespace GameLibrary.Models;

// Tabela pośrednia dla relacji self-referencing M:N Player <-> Player
public class Friend
{
    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public int FriendId { get; set; }
    public Player FriendPlayer { get; set; } = null!;
}
