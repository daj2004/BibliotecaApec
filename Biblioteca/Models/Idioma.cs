using System.ComponentModel.DataAnnotations;

namespace BibliotecaUNAPEC.Models
{
    public class Idioma
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Descripcion { get; set; }

        public char Estado { get; set; } = 'A';
    }
}
