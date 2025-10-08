using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaUNAPEC.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Descripcion { get; set; }

        public string SignaturaTopografica { get; set; }

        public string ISBN { get; set; }

        [Display(Name = "Tipo de Bibliografía")]
        public int TipoBibliografiaId { get; set; }
        public TipoBibliografia TipoBibliografia { get; set; }

        public int EditoraId { get; set; }
        public Editora Editora { get; set; }

        public int CienciaId { get; set; }
        public Ciencia Ciencia { get; set; }

        public int IdiomaId { get; set; }
        public Idioma Idioma { get; set; }

        [Display(Name = "Año de Publicación")]
        public int? AñoPublicacion { get; set; }

        public char Estado { get; set; } = 'A';

        public ICollection<LibroAutor> LibroAutores { get; set; }
    }

    public class LibroAutor
    {
        public int LibroId { get; set; }
        public Libro Libro { get; set; }

        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
