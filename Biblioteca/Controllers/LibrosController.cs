using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private void PopulateSelectLists(int? tipoId = null, int? editoraId = null, int? cienciaId = null, int? idiomaId = null)
        {
            ViewBag.TipoBibliografiaId = new SelectList(_context.TiposBibliografia.OrderBy(t => t.Descripcion).ToList(), "Id", "Descripcion", tipoId);
            ViewBag.EditoraId = new SelectList(_context.Editoras.OrderBy(e => e.Descripcion).ToList(), "Id", "Descripcion", editoraId);
            ViewBag.CienciaId = new SelectList(_context.Ciencias.OrderBy(c => c.Descripcion).ToList(), "Id", "Descripcion", cienciaId);
            ViewBag.IdiomaId = new SelectList(_context.Idiomas.OrderBy(i => i.Descripcion).ToList(), "Id", "Descripcion", idiomaId);
        }

        public IActionResult Create()
        {
            PopulateSelectLists();
            return View(new Libro { Estado = 'A' });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (libro == null)
            {
                ModelState.AddModelError(string.Empty, "Datos de libro inválidos.");
                PopulateSelectLists();
                return View(new Libro { Estado = 'A' });
            }

            // Validación server-side para los select placeholders (usamos value="0" en la vista)
            if (libro.TipoBibliografiaId <= 0)
                ModelState.AddModelError(nameof(libro.TipoBibliografiaId), "Seleccione un tipo de bibliografía.");
            if (libro.EditoraId <= 0)
                ModelState.AddModelError(nameof(libro.EditoraId), "Seleccione una editora.");
            if (libro.CienciaId <= 0)
                ModelState.AddModelError(nameof(libro.CienciaId), "Seleccione una ciencia.");
            if (libro.IdiomaId <= 0)
                ModelState.AddModelError(nameof(libro.IdiomaId), "Seleccione un idioma.");

            // Asegurar un Estado válido
            if (libro.Estado != 'A' && libro.Estado != 'I')
                libro.Estado = 'A';

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(libro);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar en la base de datos.");
                }
            }

            // Repoblar selects antes de devolver la vista
            PopulateSelectLists(libro.TipoBibliografiaId, libro.EditoraId, libro.CienciaId, libro.IdiomaId);
            return View(libro);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();

            PopulateSelectLists(libro.TipoBibliografiaId, libro.EditoraId, libro.CienciaId, libro.IdiomaId);
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

            PopulateSelectLists(libro.TipoBibliografiaId, libro.EditoraId, libro.CienciaId, libro.IdiomaId);
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
