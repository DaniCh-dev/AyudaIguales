using HelpMate.Models;

namespace HelpMate.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // 1) Usuarios
            if (!db.Usuarios.Any())
            {
                db.Usuarios.AddRange(
                    new Usuario { Nombre = "Admin", Email = "admin@helpmate.com", PasswordHash = "admin", Rol = "Administrador", Activo = true },
                    new Usuario { Nombre = "Alumno 1", Email = "alumno1@helpmate.com", PasswordHash = "1234", Rol = "Alumno", Activo = true }
                );
                db.SaveChanges();
            }

            // 2) Asignaturas
            if (!db.Asignaturas.Any())
            {
                db.Asignaturas.AddRange(
                    new Asignatura { Nombre = "Desarrollo de Interfaces" },
                    new Asignatura { Nombre = "Acceso a Datos" }
                );
                db.SaveChanges();
            }

            // 3) Solicitudes
            if (!db.Solicitudes.Any())
            {
                var alumno = db.Usuarios.First(u => u.Email == "alumno1@helpmate.com");
                var di = db.Asignaturas.First(a => a.Nombre == "Desarrollo de Interfaces");
                var ad = db.Asignaturas.First(a => a.Nombre == "Acceso a Datos");

                db.Solicitudes.AddRange(
                    new Solicitud
                    {
                        UsuarioId = alumno.Id,
                        AsignaturaId = di.Id,
                        Titulo = "Duda sobre MVC",
                        Descripcion = "No entiendo bien la diferencia entre Controller y View. ¿Me echas una mano?",
                        Estado = "Abierta"
                    },
                    new Solicitud
                    {
                        UsuarioId = alumno.Id,
                        AsignaturaId = ad.Id,
                        Titulo = "Ayuda con EF Core",
                        Descripcion = "¿Cómo se hacen relaciones 1:N y migraciones sin liarla?",
                        Estado = "Abierta"
                    }
                );

                db.SaveChanges();
            }
        }
    }
}
