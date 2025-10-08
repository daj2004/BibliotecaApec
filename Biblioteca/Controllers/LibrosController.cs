using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;

namespace BibliotecaUNAPEC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LibrosController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var libros = _context.Libros
                .Include(l => l.TipoBibliografia)
                .Include(l => l.Editora)
                .Include(l => l.Ciencia)
                .Include(l => l.Idioma);
            return View(await libros.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Tipos = _context.TiposBibliografia.ToList();
            ViewBag.Editoras = _context.Editoras.ToList();
            ViewBag.Ciencias = _context.Ciencias.ToList();
            ViewBag.Idiomas = _context.Idiomas.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            ViewBag.Tipos = _context.TiposBibliografia.ToList();
            ViewBag.Editoras = _context.Editoras.ToList();
            ViewBag.Ciencias = _context.Ciencias.ToList();
            ViewBag.Idiomas = _context.Idiomas.ToList();
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro)
        {
            if (id != libro.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
