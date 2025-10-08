using Microsoft.AspNetCore.Mvc;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaUNAPEC.Controllers
{
    public class IdiomasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IdiomasController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index() =>
            View(await _context.Idiomas.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Idioma idioma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(idioma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(idioma);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var idioma = await _context.Idiomas.FindAsync(id);
            if (idioma == null) return NotFound();
            return View(idioma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Idioma idioma)
        {
            if (id != idioma.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(idioma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(idioma);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var idioma = await _context.Idiomas.FindAsync(id);
            if (idioma == null) return NotFound();
            _context.Idiomas.Remove(idioma);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
