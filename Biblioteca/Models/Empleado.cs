using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaUNAPEC.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string Cedula { get; set; }

        public string TandaLaboral { get; set; }

        [Range(0, 100)]
        public decimal PorcentajeComision { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        public char Estado { get; set; } = 'A';
    }
}
