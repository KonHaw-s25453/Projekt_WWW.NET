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
    public IActionResult Create()
    {
        return View();
    }

    // POST /Game/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ReleaseYear")] Game game)
    {
        if (!ModelState.IsValid) return View(game);
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Game/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var game = await _context.Games
            .Include(g => g.GameTags)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (game == null) return NotFound();

        ViewBag.AllTags = await _context.Tags.ToListAsync();
        ViewBag.SelectedTagIds = game.GameTags.Select(gt => gt.TagId).ToList();

        return View(game);
    }

    // POST /Game/Edit/5
    // C#
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseYear")] Game game, int[] selectedTags)
    {
        if (id != game.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            ViewBag.AllTags = await _context.Tags.ToListAsync();
            ViewBag.SelectedTagIds = selectedTags;
            return View(game);
        }

        var gameToUpdate = await _context.Games
            .Include(g => g.GameTags)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gameToUpdate == null) return NotFound();

        gameToUpdate.Title = game.Title;
        gameToUpdate.ReleaseYear = game.ReleaseYear;

        // Aktualizacja tagów
        gameToUpdate.GameTags.Clear();
        foreach (var tagId in selectedTags)
        {
            gameToUpdate.GameTags.Add(new GameTag { GameId = id, TagId = tagId });
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
