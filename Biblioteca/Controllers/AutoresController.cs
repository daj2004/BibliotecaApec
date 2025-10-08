using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;

namespace BibliotecaUNAPEC.Controllers
{
    public class AutoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AutoresController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var autores = _context.Autores.Include(a => a.IdiomaNativo);
            return View(await autores.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Idiomas = _context.Idiomas.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Idiomas = _context.Idiomas.ToList();
            return View(autor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null) return NotFound();
            ViewBag.Idiomas = _context.Idiomas.ToList();
            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Autor autor)
        {
            if (id != autor.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Idiomas = _context.Idiomas.ToList();
            return View(autor);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null) return NotFound();
            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
