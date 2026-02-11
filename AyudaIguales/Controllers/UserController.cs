using Microsoft.AspNetCore.Mvc;
using AyudaIguales.Models;
using AyudaIguales.Services;

namespace AyudaIguales.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IEstadisticasService _estadisticasService;

        
        public UserController(IUserService userService, IEstadisticasService estadisticasService)
        {
            _userService = userService;
            _estadisticasService = estadisticasService;
        }

        // GET: Mostrar formulario de registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: Registrar nuevo usuario (formulario tradicional)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos no válidos";
                return View(request);
            }

            var resultado = await _userService.RegisterUserAsync(request);

            if (resultado.ok)
            {
                TempData["Success"] = resultado.msg;
                return RedirectToAction("Login");
            }
            else
            {
                TempData["Error"] = resultado.msg;
                return View(request);
            }
        }

        // GET: Mostrar formulario de login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Iniciar sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos no válidos";
                return View(request);
            }

            var resultado = await _userService.LoginUserAsync(request);

            if (resultado.ok)
            {
                // Guardar datos del usuario en sesión
                HttpContext.Session.SetString("UserId", resultado.usuario.id.ToString());
                HttpContext.Session.SetString("UserName", resultado.usuario.nombre_usuario);
                HttpContext.Session.SetString("UserRole", resultado.usuario.rol.ToString());
                HttpContext.Session.SetString("CentroId", resultado.usuario.id_centro.ToString());


                

                
               return RedirectToAction("AyudaHome", "Ayuda");
            }
            else
            {
                TempData["Error"] = resultado.msg;
                return View(request);
            }
        }

        // GET: Cerrar sesión
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Sesión cerrada correctamente";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            // Verificar si es admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin")
            {
                return Json(new { ok = false, msg = "No autorizado" });
            }

            // Obtener id_centro de la sesión
            var centroIdString = HttpContext.Session.GetString("CentroId");
            if (string.IsNullOrEmpty(centroIdString))
            {
                return Json(new { ok = false, msg = "No se pudo obtener el centro" });
            }

            int id_centro = int.Parse(centroIdString);
            var resultado = await _userService.ObtenerTodosUsuariosAsync(id_centro);

            return Json(resultado);
        }
        // GET: Ver perfil y estadisticas
[HttpGet]
public async Task<IActionResult> Perfil()
{
    // Verificar si hay sesion iniciada
    var userIdString = HttpContext.Session.GetString("UserId");
    if (string.IsNullOrEmpty(userIdString))
    {
        return RedirectToAction("Login");
    }

    var centroIdString = HttpContext.Session.GetString("CentroId");
    var rol = HttpContext.Session.GetString("UserRole") ?? "usuario";

    int id_usuario = int.Parse(userIdString);
    int id_centro = int.Parse(centroIdString ?? "0");

    // Obtener estadisticas
    var resultado = await _estadisticasService.ObtenerEstadisticasAsync(id_usuario, id_centro, rol);

    if (!resultado.ok)
    {
        TempData["Error"] = resultado.msg;
    }

    ViewBag.Estadisticas = resultado.estadisticas;
    ViewBag.Rol = rol;
    ViewBag.UserId = id_usuario;
    ViewBag.CentroId = id_centro;

    return View();
}
        public IActionResult Index()
        {
            return View();
        }
    }
}