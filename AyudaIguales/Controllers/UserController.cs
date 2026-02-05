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

        public IActionResult Index()
        {
            return View();
        }
    }
}