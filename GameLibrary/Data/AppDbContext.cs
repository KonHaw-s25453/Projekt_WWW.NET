using GameLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Każdy DbSet = jedna tabela w bazie danych
    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PlayerGame> PlayerGames { get; set; }
    public DbSet<GameTag> GameTags { get; set; }
    public DbSet<Friend> Friends { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Klucz złożony tabeli pośredniej PlayerGame (PlayerId + GameId)
        modelBuilder.Entity<PlayerGame>()
            .HasKey(pg => new { pg.PlayerId, pg.GameId });

        modelBuilder.Entity<PlayerGame>()
            .HasOne(pg => pg.Player)
            .WithMany(p => p.PlayerGames)
            .HasForeignKey(pg => pg.PlayerId);

        modelBuilder.Entity<PlayerGame>()
            .HasOne(pg => pg.Game)
            .WithMany(g => g.PlayerGames)
            .HasForeignKey(pg => pg.GameId);

        // Klucz złożony tabeli pośredniej GameTag (GameId + TagId)
        modelBuilder.Entity<GameTag>()
            .HasKey(gt => new { gt.GameId, gt.TagId });

        modelBuilder.Entity<GameTag>()
            .HasOne(gt => gt.Game)
            .WithMany(g => g.GameTags)
            .HasForeignKey(gt => gt.GameId);

        modelBuilder.Entity<GameTag>()
            .HasOne(gt => gt.Tag)
            .WithMany(t => t.GameTags)
            .HasForeignKey(gt => gt.TagId);

        // Klucz złożony tabeli Friend (PlayerId + FriendId) - self-referencing
        modelBuilder.Entity<Friend>()
            .HasKey(f => new { f.PlayerId, f.FriendId });

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.Player)
            .WithMany(p => p.Friends)
            .HasForeignKey(f => f.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Friend>()
            .HasOne(f => f.FriendPlayer)
            .WithMany()
            .HasForeignKey(f => f.FriendId)
            .OnDelete(DeleteBehavior.Restrict);

        // Dane startowe (seed)
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "RPG" },
            new Tag { Id = 2, Name = "FPS" },
            new Tag { Id = 3, Name = "Indie" },
            new Tag { Id = 4, Name = "OpenWorld" }
        );

        modelBuilder.Entity<Player>().HasData(
            new Player { Id = 1, Nick = "Jan", Email = "jan@example.com" },
            new Player { Id = 2, Nick = "Anna", Email = "anna@example.com" }
        );

        modelBuilder.Entity<Game>().HasData(
            new Game { Id = 1, Title = "Wiedźmin 3", ReleaseYear = 2015 },
            new Game { Id = 2, Title = "Minecraft", ReleaseYear = 2011 },
            new Game { Id = 3, Title = "Cyberpunk 2077", ReleaseYear = 2020 }
        );

        modelBuilder.Entity<PlayerGame>().HasData(
            new PlayerGame { PlayerId = 1, GameId = 1, HoursPlayed = 120 },
            new PlayerGame { PlayerId = 1, GameId = 2, HoursPlayed = 50 },
            new PlayerGame { PlayerId = 2, GameId = 1, HoursPlayed = 80 }
        );

        modelBuilder.Entity<GameTag>().HasData(
            new GameTag { GameId = 1, TagId = 1 },  // Wiedźmin 3 - RPG
            new GameTag { GameId = 1, TagId = 4 },  // Wiedźmin 3 - OpenWorld
            new GameTag { GameId = 2, TagId = 3 },  // Minecraft - Indie
            new GameTag { GameId = 3, TagId = 1 },  // Cyberpunk - RPG
            new GameTag { GameId = 3, TagId = 4 }   // Cyberpunk - OpenWorld
        );

        modelBuilder.Entity<Friend>().HasData(
            new Friend { PlayerId = 1, FriendId = 2 }  // Jan zna Annę
        );
    }
}
