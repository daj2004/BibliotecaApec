using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private void PopulateIdiomas(int? selectedId = null)
        {
            ViewBag.IdiomaNativoId = new SelectList(
                _context.Idiomas.OrderBy(i => i.Descripcion).ToList(),
                "Id",
                "Descripcion",
                selectedId);
        }

        public IActionResult Create()
        {
            PopulateIdiomas();
            return View(new Autor { Estado = 'A' });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Autor autor)
        {
            if (autor == null)
            {
                ModelState.AddModelError(string.Empty, "Datos de autor inválidos.");
                PopulateIdiomas();
                return View(new Autor { Estado = 'A' });
            }

            if (autor.IdiomaNativoId <= 0)
                ModelState.AddModelError(nameof(autor.IdiomaNativoId), "Seleccione un idioma nativo.");

            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateIdiomas(autor.IdiomaNativoId);
            return View(autor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null) return NotFound();
            PopulateIdiomas(autor.IdiomaNativoId);
            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Autor autor)
        {
            if (id != autor.Id) return NotFound();

            if (autor.IdiomaNativoId <= 0)
                ModelState.AddModelError(nameof(autor.IdiomaNativoId), "Seleccione un idioma nativo.");

            if (ModelState.IsValid)
            {
                _context.Update(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateIdiomas(autor.IdiomaNativoId);
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
