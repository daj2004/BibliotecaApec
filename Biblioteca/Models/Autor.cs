using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaUNAPEC.Models
{
    public class Autor
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        public string? PaisOrigen { get; set; }

        [Display(Name = "Idioma Nativo")]
        public int IdiomaNativoId { get; set; }
        public Idioma? IdiomaNativo { get; set; }

        public char Estado { get; set; } = 'A';

        public ICollection<LibroAutor>? LibroAutores { get; set; } = new List<LibroAutor>();
    }
}
