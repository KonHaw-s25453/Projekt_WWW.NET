using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Controllers;

public class GameController : Controller
{
    private readonly AppDbContext _context;

    public GameController(AppDbContext context)
    {
        _context = context;
    }

    // GET /Game — lista wszystkich gier
    public async Task<IActionResult> Index()
    {
        var games = await _context.Games
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
            .ToListAsync();
        return View(games);
    }

    // GET /Game/Details/5 — szczegóły gry (gracze + tagi)
    public async Task<IActionResult> Details(int id)
    {
        var game = await _context.Games
            .Include(g => g.PlayerGames)
                .ThenInclude(pg => pg.Player)
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Tag)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null) return NotFound();
        return View(game);
    }

    // GET /Game/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST /Game/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ReleaseYear")] Game game)
    {
        if (!ModelState.IsValid) return View(game);
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Game/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null) return NotFound();
        return View(game);
    }

    // POST /Game/Edit/5
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseYear")] Game game)
    {
        if (id != game.Id) return NotFound();
        if (!ModelState.IsValid) return View(game);

        _context.Update(game);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Game/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null) return NotFound();
        return View(game);
    }

    // POST /Game/Delete/5
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
