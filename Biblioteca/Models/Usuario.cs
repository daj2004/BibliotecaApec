using System.ComponentModel.DataAnnotations;

namespace BibliotecaUNAPEC.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string Cedula { get; set; }

        public string NumeroCarnet { get; set; }

        [Display(Name = "Tipo de Persona")]
        public string TipoPersona { get; set; } // Física o Jurídica

        public char Estado { get; set; } = 'A';
    }
}
