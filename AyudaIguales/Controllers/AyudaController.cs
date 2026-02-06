using Microsoft.AspNetCore.Mvc;
using AyudaIguales.Models;
using AyudaIguales.Services;

namespace AyudaIguales.Controllers
{
    public class AyudaController : Controller
    {
        private readonly IAyudaService _ayudaService;
        private readonly IUserService _userService;

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

            if (resultado.ok)
            {
                // Pasar los filtros actuales a la vista para mantenerlos seleccionados
                ViewBag.BusquedaActual = busqueda;
                ViewBag.EstadoActual = estado;
                ViewBag.UsuarioActual = id_usuario;
                ViewBag.FechaActual = fecha;
                ViewBag.RespuestasActual = respuestas;

                return View(resultado.ayudas);
            }
            else
            {
                TempData["Error"] = resultado.msg;
                return View(new List<Ayuda>());
            }
        }

        // GET: Obtener usuarios para el filtro (solo admin)
        [HttpGet]
        public async Task<IActionResult> ObtenerUsuariosParaFiltro()
        {
            // Verificar si es admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin")
            {
                return Json(new { ok = false, msg = "No autorizado" });
            }

            try
            {
                // Aquí deberías tener un método en tu servicio para obtener usuarios
                // Por ahora devuelvo un ejemplo con consulta directa
                // Deberías crear un método GetAllUsersAsync en IUserService

                // Ejemplo temporal - Reemplaza esto con tu servicio real
                return Json(new
                {
                    ok = true,
                    usuarios = new[]
                    {
                        new { id = 1, nombre_usuario = "Usuario1" },
                        new { id = 2, nombre_usuario = "Usuario2" }
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = $"Error: {ex.Message}" });
            }
        }
    }
}