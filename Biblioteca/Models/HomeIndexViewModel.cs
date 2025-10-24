using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaUNAPEC.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Libro> UltimosLibros { get; set; } = Enumerable.Empty<Libro>();
        public int TotalLibros { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalPrestamosActivos { get; set; }
        public IEnumerable<Ciencia> Ciencias { get; set; } = Enumerable.Empty<Ciencia>();

        // Parámetros de filtro / orden / paginado
        public string? Search { get; set; }
        public int? CienciaId { get; set; }
        public string? Sort { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}