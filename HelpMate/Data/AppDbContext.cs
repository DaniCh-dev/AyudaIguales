using HelpMate.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpMate.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Asignatura> Asignaturas => Set<Asignatura>();
        public DbSet<Solicitud> Solicitudes => Set<Solicitud>();
        public DbSet<Oferta> Ofertas => Set<Oferta>();
        public DbSet<Mensaje> Mensajes => Set<Mensaje>();
        public DbSet<Valoracion> Valoraciones => Set<Valoracion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evita duplicar ofertas del mismo tutor a la misma solicitud
            modelBuilder.Entity<Oferta>()
                .HasIndex(o => new { o.SolicitudId, o.TutorUsuarioId })
                .IsUnique();

            // 1 valoración por oferta/tutoría (como en el DBML)
            modelBuilder.Entity<Valoracion>()
                .HasIndex(v => v.OfertaId)
                .IsUnique();

            // Opcional: limitar longitudes si no pusiste DataAnnotations
            // (si ya pusiste [StringLength], puedes borrar esto sin problema)
        }
    }
}
