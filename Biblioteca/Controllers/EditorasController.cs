using Microsoft.AspNetCore.Mvc;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaUNAPEC.Controllers
{
    public class EditorasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EditorasController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index() =>
            View(await _context.Editoras.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Editora e)
        {
            if (ModelState.IsValid)
            {
                _context.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(e);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var e = await _context.Editoras.FindAsync(id);
            if (e == null) return NotFound();
            return View(e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Editora e)
        {
            if (id != e.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(e);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(e);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var e = await _context.Editoras.FindAsync(id);
            if (e == null) return NotFound();
            _context.Editoras.Remove(e);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
