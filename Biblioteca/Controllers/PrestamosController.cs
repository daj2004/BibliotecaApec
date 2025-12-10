using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Data;
using BibliotecaUNAPEC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Text;

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

        // GET: Report form
        public IActionResult Report()
        {
            PopulateDropDowns();
            var vm = new RentasReportViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(RentasReportViewModel vm, string? export)
        {
            // Repoblar selects
            PopulateDropDowns();

            var q = _context.Prestamos
                .Include(p => p.Libro).ThenInclude(l => l.TipoBibliografia)
                .Include(p => p.Libro).ThenInclude(l => l.Idioma)
                .Include(p => p.Usuario)
                .Include(p => p.Empleado)
                .AsQueryable();

            if (vm.FechaDesde.HasValue) q = q.Where(p => p.FechaPrestamo.Date >= vm.FechaDesde.Value.Date);
            if (vm.FechaHasta.HasValue) q = q.Where(p => p.FechaPrestamo.Date <= vm.FechaHasta.Value.Date);

            if (vm.TipoBibliografiaIds?.Any() == true) q = q.Where(p => vm.TipoBibliografiaIds.Contains(p.Libro.TipoBibliografiaId));
            if (vm.IdiomaIds?.Any() == true) q = q.Where(p => vm.IdiomaIds.Contains(p.Libro.IdiomaId));
            if (vm.LibroIds?.Any() == true) q = q.Where(p => vm.LibroIds.Contains(p.LibroId));
            if (vm.UsuarioIds?.Any() == true) q = q.Where(p => vm.UsuarioIds.Contains(p.UsuarioId));
            if (vm.EmpleadoIds?.Any() == true) q = q.Where(p => vm.EmpleadoIds.Contains(p.EmpleadoId));

            var list = await q.Select(p => new
            {
                p.FechaPrestamo,
                LibroDescripcion = p.Libro.Descripcion,
                TipoBibliografiaDescripcion = p.Libro.TipoBibliografia.Descripcion,
                IdiomaDescripcion = p.Libro.Idioma.Descripcion,
                UsuarioNombre = p.Usuario.Nombre,
                EmpleadoNombre = p.Empleado.Nombre,
                p.MontoPorDia,
                p.CantidadDias,
                Total = p.MontoPorDia * p.CantidadDias
            }).ToListAsync();

            vm.Results = list.Select(r => new RentasReportRow
            {
                FechaPrestamo = r.FechaPrestamo,
                Libro = r.LibroDescripcion,
                TipoBibliografia = r.TipoBibliografiaDescripcion,
                Idioma = r.IdiomaDescripcion,
                Usuario = r.UsuarioNombre,
                Empleado = r.EmpleadoNombre,
                MontoPorDia = r.MontoPorDia,
                CantidadDias = r.CantidadDias,
                Total = r.Total
            }).ToList();

            if (export == "csv")
            {
                var sb = new StringBuilder();
                sb.AppendLine("FechaPrestamo,Libro,TipoBibliografia,Idioma,Usuario,Empleado,MontoPorDia,CantidadDias,Total");
                foreach (var r in vm.Results)
                {
                    sb.AppendLine($"{r.FechaPrestamo:yyyy-MM-dd},{EscapeCsv(r.Libro)},{EscapeCsv(r.TipoBibliografia)},{EscapeCsv(r.Idioma)},{EscapeCsv(r.Usuario)},{EscapeCsv(r.Empleado)},{r.MontoPorDia},{r.CantidadDias},{r.Total}");
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"rentas_{DateTime.Now:yyyyMMddHHmmss}.csv");
            }

            return View(vm);
        }

        private string EscapeCsv(string? input) => string.IsNullOrEmpty(input) ? string.Empty : "\"" + input.Replace("\"", "\"\"") + "\"";

        private void PopulateDropDowns()
        {
            ViewBag.Libros = new SelectList(_context.Libros.OrderBy(l => l.Descripcion).ToList(), "Id", "Descripcion");
            ViewBag.Usuarios = new SelectList(_context.Usuarios.OrderBy(u => u.Nombre).ToList(), "Id", "Nombre");
            ViewBag.Empleados = new SelectList(_context.Empleados.OrderBy(e => e.Nombre).ToList(), "Id", "Nombre");
            ViewBag.TiposBibliografia = new SelectList(_context.TiposBibliografia.OrderBy(t => t.Descripcion).ToList(), "Id", "Descripcion");
            ViewBag.Idiomas = new SelectList(_context.Idiomas.OrderBy(i => i.Descripcion).ToList(), "Id", "Descripcion");
        }
    }
}
