using Microsoft.AspNetCore.Mvc;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaUNAPEC.Controllers
{
    public class TiposBibliografiaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposBibliografiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() =>
            View(await _context.TiposBibliografia.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoBibliografia tipo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipo);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tipo = await _context.TiposBibliografia.FindAsync(id);
            if (tipo == null) return NotFound();
            return View(tipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoBibliografia tipo)
        {
            if (id != tipo.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(tipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tipo = await _context.TiposBibliografia.FindAsync(id);
            if (tipo == null) return NotFound();
            _context.TiposBibliografia.Remove(tipo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
