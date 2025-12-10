using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaUNAPEC.Models
{
    public class RentasReportRow
    {
        public DateTime FechaPrestamo { get; set; }
        public string Libro { get; set; } = string.Empty;
        public string TipoBibliografia { get; set; } = string.Empty;
        public string Idioma { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Empleado { get; set; } = string.Empty;
        public decimal MontoPorDia { get; set; }
        public int CantidadDias { get; set; }
        public decimal Total { get; set; }
    }

    public class RentasReportViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? FechaDesde { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaHasta { get; set; }

        // Filtros (múltiple selección)
        public List<int> TipoBibliografiaIds { get; set; } = new();
        public List<int> IdiomaIds { get; set; } = new();
        public List<int> LibroIds { get; set; } = new();
        public List<int> UsuarioIds { get; set; } = new();
        public List<int> EmpleadoIds { get; set; } = new();

        // Resultados
        public List<RentasReportRow> Results { get; set; } = new();
    }
}
