using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;

namespace BibliotecaUNAPEC.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmpleadosController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index() =>
            View(await _context.Empleados.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado e)
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
            var e = await _context.Empleados.FindAsync(id);
            if (e == null) return NotFound();
            return View(e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado e)
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
            var e = await _context.Empleados.FindAsync(id);
            if (e == null) return NotFound();
            _context.Empleados.Remove(e);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
