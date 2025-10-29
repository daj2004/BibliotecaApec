using Microsoft.AspNetCore.Mvc;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaUNAPEC.Controllers
{
    public class CienciasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CienciasController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index() =>
            View(await _context.Ciencias.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ciencia c)
        {
            if (ModelState.IsValid)
            {
                _context.Add(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(c);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _context.Ciencias.FindAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ciencia c)
        {
            if (id != c.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(c);
        }

        // Nuevas acciones Update (GET + POST) — compatibles con enlaces que usen "Update"
        public async Task<IActionResult> Update(int id)
        {
            var c = await _context.Ciencias.FindAsync(id);
            if (c == null) return NotFound();
            return View("Update", c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Ciencia c)
        {
            if (id != c.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Update", c);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Ciencias.FindAsync(id);
            if (c == null) return NotFound();
            _context.Ciencias.Remove(c);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
