using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;

namespace BibliotecaUNAPEC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UsuariosController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index() =>
            View(await _context.Usuarios.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario u)
        {
            if (id != u.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(u);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(u);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return NotFound();
            _context.Usuarios.Remove(u);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
