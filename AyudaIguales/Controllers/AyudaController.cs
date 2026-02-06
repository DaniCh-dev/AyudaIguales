using Microsoft.AspNetCore.Mvc;
using AyudaIguales.Models;
using AyudaIguales.Services;

namespace AyudaIguales.Controllers
{
    public class AyudaController : Controller
    {
        private readonly IAyudaService _ayudaService;
        private readonly IUserService _userService;

        // Constructor con ambos servicios inyectados
        public AyudaController(IAyudaService ayudaService, IUserService userService)
        {
            _ayudaService = ayudaService;
            _userService = userService;
        }

        // GET: Mostrar página principal de ayudas (con o sin filtros)
        [HttpGet]
        public async Task<IActionResult> AyudaHome(string busqueda, string estado, int? id_usuario, string fecha, string respuestas)
        {
            // Verificar si hay sesión iniciada
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Login", "User");
            }

            // Si hay filtros, usarlos; si no, obtener todas las ayudas
            ObtenerAyudasResponse resultado;

            if (!string.IsNullOrEmpty(busqueda) || !string.IsNullOrEmpty(estado) ||
                id_usuario.HasValue || !string.IsNullOrEmpty(fecha) || !string.IsNullOrEmpty(respuestas))
            {
                // Aplicar filtros
                var filtros = new FiltrosAyuda
                {
                    busqueda = busqueda,
                    estado = estado,
                    id_usuario = id_usuario,
                    fecha = fecha,
                    respuestas = respuestas
                };

                resultado = await _ayudaService.ObtenerAyudasConFiltrosAsync(filtros);
            }
            else
            {
                // Obtener todas las ayudas
                resultado = await _ayudaService.ObtenerAyudasAsync();
            }

            // Cargar usuarios si es admin (para el filtro)
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "admin")
            {
                var usuarios = await _userService.ObtenerTodosUsuariosAsync();
                if (usuarios.ok)
                {
                    ViewBag.Usuarios = usuarios.usuarios;
                }
            }

            // Pasar los filtros actuales a la vista para mantenerlos seleccionados
            ViewBag.BusquedaActual = busqueda ?? "";
            ViewBag.EstadoActual = estado ?? "";
            ViewBag.UsuarioActual = id_usuario;
            ViewBag.FechaActual = fecha ?? "";
            ViewBag.RespuestasActual = respuestas ?? "";

            if (resultado.ok)
            {
                return View(resultado.ayudas);
            }
            else
            {
                TempData["Error"] = resultado.msg;
                return View(new List<Ayuda>());
            }
        }
    }
}