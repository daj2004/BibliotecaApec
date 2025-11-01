using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaUNAPEC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string? search, int? cienciaId, string? sort, int page = 1, int pageSize = 10)
        {
            // Mostrar login al abrir la aplicación, sin cambiar rutas globales
            var user = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Home") });
            }

            var librosQuery = _context.Libros
                .Include(l => l.Ciencia)
                .Include(l => l.Editora)
                .Include(l => l.Idioma)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                librosQuery = librosQuery.Where(l =>
                    EF.Functions.Like(l.Descripcion, term) ||
                    EF.Functions.Like(l.ISBN ?? string.Empty, term) ||
                    EF.Functions.Like(l.SignaturaTopografica ?? string.Empty, term)
                );
            }

            if (cienciaId.HasValue)
                librosQuery = librosQuery.Where(l => l.CienciaId == cienciaId.Value);

            librosQuery = sort switch
            {
                "titulo_asc" => librosQuery.OrderBy(l => l.Descripcion),
                "titulo_desc" => librosQuery.OrderByDescending(l => l.Descripcion),
                "anyo_asc" => librosQuery.OrderBy(l => l.AñoPublicacion),
                "anyo_desc" => librosQuery.OrderByDescending(l => l.AñoPublicacion),
                _ => librosQuery.OrderByDescending(l => l.Id)
            };

            var totalItems = await librosQuery.CountAsync();

            var libros = await librosQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new HomeIndexViewModel
            {
                UltimosLibros = libros,
                TotalLibros = await _context.Libros.CountAsync(),
                TotalUsuarios = await _context.Usuarios.CountAsync(),
                TotalPrestamosActivos = await _context.Prestamos.CountAsync(p => p.Estado == "Activo"),
                Ciencias = await _context.Ciencias.OrderBy(c => c.Descripcion).ToListAsync(),
                Search = search,
                CienciaId = cienciaId,
                Sort = sort,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(model);
        }

        public IActionResult AcercaDe()
        {
            return View();
        }
    }
}