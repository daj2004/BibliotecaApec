using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;

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
            ViewBag.Libros = _context.Libros.ToList();
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Empleados = _context.Empleados.ToList();
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
            return View(p);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var p = await _context.Prestamos.FindAsync(id);
            if (p == null) return NotFound();
            ViewBag.Libros = _context.Libros.ToList();
            ViewBag.Usuarios = _context.Usuarios.ToList();
            ViewBag.Empleados = _context.Empleados.ToList();
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
    }
}
