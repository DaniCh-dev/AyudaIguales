using Microsoft.AspNetCore.Mvc;
using AyudaIguales.Models;
using AyudaIguales.Services;

namespace AyudaIguales.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

                TempData["Success"] = "Inicio de sesión exitoso";

                
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

        // GET: Obtener todos los usuarios (para filtros de admin)
        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            // Verificar si es admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "admin")
            {
                return Json(new { ok = false, msg = "No autorizado" });
            }

            var resultado = await _userService.ObtenerTodosUsuariosAsync();
            return Json(resultado);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}