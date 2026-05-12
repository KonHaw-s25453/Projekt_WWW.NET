using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Controllers;

public class PlayerController : Controller
{
    private readonly AppDbContext _context;

    // DbContext jest wstrzykiwany przez Dependency Injection
    public PlayerController(AppDbContext context)
    {
        _context = context;
    }

    // GET /Player — lista wszystkich graczy
    public async Task<IActionResult> Index()
    {
        var players = await _context.Players.ToListAsync();
        return View(players);
    }

    // GET /Player/Details/5 — profil gracza z grami i znajomymi
    public async Task<IActionResult> Details(int id)
    {
        var player = await _context.Players
            .Include(p => p.PlayerGames)
                .ThenInclude(pg => pg.Game)
            .Include(p => p.Friends)
                .ThenInclude(f => f.FriendPlayer)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player == null) return NotFound();
        return View(player);
    }

    // GET /Player/Create — formularz tworzenia
    public IActionResult Create()
    {
        return View();
    }

    // POST /Player/Create — zapis nowego gracza
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nick,Email")] Player player)
    {
        if (!ModelState.IsValid) return View(player);
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Player/Edit/5 — formularz edycji
    public async Task<IActionResult> Edit(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null) return NotFound();
        return View(player);
    }

    // POST /Player/Edit/5 — zapis zmian
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nick,Email")] Player player)
    {
        if (id != player.Id) return NotFound();
        if (!ModelState.IsValid) return View(player);

        _context.Update(player);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Player/Delete/5 — potwierdzenie usunięcia
    public async Task<IActionResult> Delete(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null) return NotFound();
        return View(player);
    }

    // POST /Player/Delete/5 — faktyczne usunięcie
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player != null)
        {
            var playerFriends = await _context.Friends
                .Where(f => f.PlayerId == id || f.FriendId == id)
                .ToListAsync();

            if (playerFriends.Any())
            {
                _context.Friends.RemoveRange(playerFriends);
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
