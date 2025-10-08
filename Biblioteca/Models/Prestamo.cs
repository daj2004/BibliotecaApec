using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaUNAPEC.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; }

        public int LibroId { get; set; }
        public Libro Libro { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? FechaDevolucion { get; set; }

        [Display(Name = "Monto por Día")]
        public decimal MontoPorDia { get; set; } = 50.00m;

        [Display(Name = "Cantidad de Días")]
        public int CantidadDias { get; set; } = 1;

        public string Comentario { get; set; }

        public string Estado { get; set; } = "Activo";
    }
}
