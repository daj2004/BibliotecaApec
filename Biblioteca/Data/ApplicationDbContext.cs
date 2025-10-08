using Microsoft.EntityFrameworkCore;
using BibliotecaUNAPEC.Models;

namespace BibliotecaUNAPEC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TipoBibliografia> TiposBibliografia { get; set; }
        public DbSet<Editora> Editoras { get; set; }
        public DbSet<Idioma> Idiomas { get; set; }
        public DbSet<Ciencia> Ciencias { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔧 Configurar relación muchos a muchos sin eliminar en cascada
            modelBuilder.Entity<LibroAutor>()
                .HasKey(la => new { la.LibroId, la.AutorId });

            modelBuilder.Entity<LibroAutor>()
                .HasOne(la => la.Libro)
                .WithMany(l => l.LibroAutores)
                .HasForeignKey(la => la.LibroId)
                .OnDelete(DeleteBehavior.Restrict); // <--- Aquí está el fix 💡

            modelBuilder.Entity<LibroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.LibroAutores)
                .HasForeignKey(la => la.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // 💰 También podemos especificar precisión para evitar los warnings
            modelBuilder.Entity<Empleado>()
                .Property(e => e.PorcentajeComision)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prestamo>()
                .Property(p => p.MontoPorDia)
                .HasPrecision(18, 2);
        }

    }
}
