using Microsoft.AspNetCore.Mvc;
using AyudaIguales.Models;
using AyudaIguales.Services;

namespace AyudaIguales.Controllers
{
    public class AyudaController : Controller
    {
        private readonly IAyudaService _ayudaService;
        private readonly IUserService _userService;
        private readonly IRespuestaService _respuestaService;

        // Constructor con ambos servicios inyectados
        public AyudaController(IAyudaService ayudaService, IUserService userService, IRespuestaService respuestaService)
        {
            _ayudaService = ayudaService;
            _userService = userService;
            _respuestaService = respuestaService;
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

            // Obtener id_centro de la sesión
            var centroIdString = HttpContext.Session.GetString("CentroId");
            if (string.IsNullOrEmpty(centroIdString))
            {
                TempData["Error"] = "No se pudo obtener el centro del usuario";
                return RedirectToAction("Login", "User");
            }
            int id_centro = int.Parse(centroIdString);

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

                resultado = await _ayudaService.ObtenerAyudasConFiltrosAsync(filtros, id_centro);
            }
            else
            {
                // Obtener todas las ayudas del centro
                resultado = await _ayudaService.ObtenerAyudasAsync(id_centro);
            }

            // Cargar usuarios si es admin (solo del mismo centro)
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "admin")
            {
                var usuarios = await _userService.ObtenerTodosUsuariosAsync(id_centro);
                if (usuarios.ok)
                {
                    ViewBag.Usuarios = usuarios.usuarios;
                }
            }

            // Pasar los filtros actuales a la vista
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

        // GET: Mostrar formulario para crear ayuda
        [HttpGet]
        public IActionResult CrearAyuda()
        {
            // Verificar si hay sesión iniciada
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        // POST: Crear nueva ayuda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearAyuda(CrearAyudaRequest request)
        {
            // Verificar si hay sesión iniciada
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            // Validar modelo
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos no válidos";
                return View(request);
            }

            // Asignar el ID del usuario desde la sesión
            request.id_usuario = int.Parse(userIdString);

            // Llamar al servicio para crear la ayuda
            var resultado = await _ayudaService.CrearAyudaAsync(request);

            if (resultado.ok)
            {
                TempData["Success"] = "Ayuda publicada correctamente";
                return RedirectToAction("AyudaHome");
            }
            else
            {
                TempData["Error"] = resultado.msg;
                return View(request);
            }
        }

        // GET: Mostrar detalle de una ayuda con sus respuestas
        [HttpGet]
        public async Task<IActionResult> DetalleAyuda(int id)
        {
            // Verificar si hay sesion iniciada
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            // Obtener id_centro de la sesion
            var centroIdString = HttpContext.Session.GetString("CentroId");
            if (string.IsNullOrEmpty(centroIdString))
            {
                TempData["Error"] = "No se pudo obtener el centro del usuario";
                return RedirectToAction("Login", "User");
            }
            int id_centro = int.Parse(centroIdString);
            int id_usuario_actual = int.Parse(userIdString);

            // Obtener la ayuda
            var ayuda = await _ayudaService.ObtenerAyudaPorIdAsync(id, id_centro);
            if (ayuda == null)
            {
                TempData["Error"] = "Ayuda no encontrada";
                return RedirectToAction("AyudaHome");
            }

            // Obtener las respuestas de la ayuda
            var respuestasResult = await _ayudaService.ObtenerRespuestasAsync(id, id_usuario_actual);

            // Pasar datos a la vista mediante ViewBag
            ViewBag.Respuestas = respuestasResult.ok ? respuestasResult.respuestas : new List<RespuestaDetalle>();
            ViewBag.CantidadRespuestas = respuestasResult.ok ? respuestasResult.respuestas.Count : 0;

            // DEBUG 
            System.Diagnostics.Debug.WriteLine($"OK: {respuestasResult.ok}");
            System.Diagnostics.Debug.WriteLine($"Cantidad: {respuestasResult.respuestas?.Count ?? 0}");
            return View(ayuda);
        }
    

    
        // GET: Mostrar formulario para responder a una ayuda
        [HttpGet]
        public async Task<IActionResult> Responder(int id)
        {
            // Verificar si hay sesion iniciada
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            // Obtener id_centro de la sesion
            var centroIdString = HttpContext.Session.GetString("CentroId");
            if (string.IsNullOrEmpty(centroIdString))
            {
                TempData["Error"] = "No se pudo obtener el centro del usuario";
                return RedirectToAction("Login", "User");
            }
            int id_centro = int.Parse(centroIdString);

            // Obtener la ayuda
            var ayuda = await _ayudaService.ObtenerAyudaPorIdAsync(id, id_centro);
            if (ayuda == null)
            {
                TempData["Error"] = "Ayuda no encontrada";
                return RedirectToAction("AyudaHome");
            }

            // Pasar la ayuda a la vista
            return View(ayuda);
        }

        // POST: Crear respuesta a una ayuda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Responder(CrearRespuestaRequest request)
        {
            // Verificar si hay sesion iniciada
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            // Validar modelo
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos no válidos";
                return RedirectToAction("DetalleAyuda", new { id = request.id_ayuda });
            }

            // Asignar el ID del usuario desde la sesion
            request.id_usuario = int.Parse(userIdString);

            // Llamar al servicio para crear la respuesta
            var resultado = await _respuestaService.CrearRespuestaAsync(request);

            if (resultado.ok)
            {
                TempData["Success"] = "Respuesta publicada correctamente";
            }
            else
            {
                TempData["Error"] = resultado.msg;
            }

            // Redirigir al detalle de la ayuda
            return RedirectToAction("DetalleAyuda", new { id = request.id_ayuda });
        }
   
    }
}