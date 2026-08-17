using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SIGMA_PROJECT.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReporteApagon> Reportes { get; set; }
        public DbSet<RegistroAuditoria> Auditorias { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
    }
}
