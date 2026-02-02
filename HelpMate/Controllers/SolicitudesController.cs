using HelpMate.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpMate.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly AppDbContext _db;

        public SolicitudesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var solicitudes = await _db.Solicitudes
                .Include(s => s.Asignatura)
                .Include(s => s.Usuario)
                .OrderByDescending(s => s.Id)
                .ToListAsync();

            return View(solicitudes);
        }
    }
}
