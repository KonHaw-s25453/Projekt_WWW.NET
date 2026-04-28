using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Controllers;

public class TagController : Controller
{
    private readonly AppDbContext _context;

    public TagController(AppDbContext context)
    {
        _context = context;
    }

    // GET /Tag — lista wszystkich tagów
    public async Task<IActionResult> Index()
    {
        var tags = await _context.Tags.ToListAsync();
        return View(tags);
    }

    // GET /Tag/Details/5 — tag i gry w tej kategorii
    public async Task<IActionResult> Details(int id)
    {
        var tag = await _context.Tags
            .Include(t => t.GameTags)
                .ThenInclude(gt => gt.Game)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tag == null) return NotFound();
        return View(tag);
    }

    // GET /Tag/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST /Tag/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Tag tag)
    {
        if (!ModelState.IsValid) return View(tag);
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Tag/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null) return NotFound();
        return View(tag);
    }

    // POST /Tag/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tag tag)
    {
        if (id != tag.Id) return NotFound();
        if (!ModelState.IsValid) return View(tag);

        _context.Update(tag);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET /Tag/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag == null) return NotFound();
        return View(tag);
    }

    // POST /Tag/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        if (tag != null)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
