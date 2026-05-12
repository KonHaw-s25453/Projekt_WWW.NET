using GameLibrary.Data;
using GameLibrary.Models;
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
            .Include(g => g.Tags)
            .ToListAsync();
        return View(games);
    }

    // GET /Game/Details/5 — szczegóły gry (gracze + tagi)
    public async Task<IActionResult> Details(int id)
    {
        var game = await _context.Games
            .Include(g => g.PlayerGames)
                .ThenInclude(pg => pg.Player)
            .Include(g => g.Tags)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null) return NotFound();
        return View(game);
    }

    // GET /Game/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.AllTags = await _context.Tags.ToListAsync();
        ViewBag.SelectedTagIds = Array.Empty<int>();
        return View();
    }

    // POST /Game/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ReleaseYear")] Game game, int[]? selectedTags)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AllTags = await _context.Tags.ToListAsync();
            ViewBag.SelectedTagIds = selectedTags ?? Array.Empty<int>();
            return View(game);
        }

        var selectedTagIds = selectedTags?.Distinct().ToArray() ?? Array.Empty<int>();
        if (selectedTagIds.Length != 0)
        {
            game.Tags = await _context.Tags
                .Where(t => selectedTagIds.Contains(t.Id))
                .ToListAsync();
        }

        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Game/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var game = await _context.Games
            .Include(g => g.Tags)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (game == null) return NotFound();

        ViewBag.AllTags = await _context.Tags.ToListAsync();
        ViewBag.SelectedTagIds = game.Tags.Select(t => t.Id).ToList();

        return View(game);
    }

    // POST /Game/Edit/5
    // C#
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseYear")] Game game, int[]? selectedTags)
    {
        if (id != game.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            ViewBag.AllTags = await _context.Tags.ToListAsync();
            ViewBag.SelectedTagIds = selectedTags ?? Array.Empty<int>();
            return View(game);
        }

        var gameToUpdate = await _context.Games
            .Include(g => g.Tags)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gameToUpdate == null) return NotFound();

        gameToUpdate.Title = game.Title;
        gameToUpdate.ReleaseYear = game.ReleaseYear;

        var selectedTagIds = selectedTags?.Distinct().ToArray() ?? Array.Empty<int>();
        var tags = await _context.Tags
            .Where(t => selectedTagIds.Contains(t.Id))
            .ToListAsync();

        gameToUpdate.Tags.Clear();
        foreach (var tag in tags)
        {
            gameToUpdate.Tags.Add(tag);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Game/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null) return NotFound();
        return View(game);
    }

    // POST /Game/Delete/5
    [HttpPost, ActionName("Delete")]
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
