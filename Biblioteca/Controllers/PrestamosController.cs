using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BibliotecaUNAPEC.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PrestamosController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var prestamos = _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .Include(p => p.Empleado);
            return View(await prestamos.ToListAsync());
        }

        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prestamo p)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Añadir un error de modelo combinado para que aparezcan en la summary (útil para depuración)
            var errores = ModelState
                .Where(kvp => kvp.Value.Errors.Any())
                .SelectMany(kvp => kvp.Value.Errors.Select(e => $"{kvp.Key}: {e.ErrorMessage}"))
                .ToList();
            if (errores.Any())
            {
                ModelState.AddModelError(string.Empty, string.Join(" | ", errores));
            }

            // Repopular antes de volver a la vista
            PopulateDropDowns();
            return View(p);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var p = await _context.Prestamos.FindAsync(id);
            if (p == null) return NotFound();
            PopulateDropDowns();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prestamo p)
        {
            if (id != p.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopular antes de volver a la vista
            PopulateDropDowns();
            return View(p);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Prestamos.FindAsync(id);
            if (p == null) return NotFound();
            _context.Prestamos.Remove(p);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDowns()
        {
            ViewBag.Libros = new SelectList(_context.Libros.ToList(), "Id", "Descripcion");
            ViewBag.Usuarios = new SelectList(_context.Usuarios.ToList(), "Id", "Nombre");
            ViewBag.Empleados = new SelectList(_context.Empleados.ToList(), "Id", "Nombre");
        }
    }
}
