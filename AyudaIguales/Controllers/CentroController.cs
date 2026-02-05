using AyudaIguales.Models;
using AyudaIguales.Services;
using Microsoft.AspNetCore.Mvc;

namespace AyudaIguales.Controllers
{
    public class CentroController : Controller
    {
        private readonly ICentroService _centroService;

        public CentroController(ICentroService centroService)
        {
            _centroService = centroService ?? throw new ArgumentNullException(nameof(centroService));
        }

        // GET: Muestra el formulario
        public IActionResult CreateCentro()
        {
            return View();
        }

        // POST: Procesa el formulario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCentro(Centro centro)
        {
            Console.WriteLine("=== MÉTODO POST EJECUTADO ===");
            Console.WriteLine($"Nombre: {centro.nombre}");
            Console.WriteLine($"CIF: {centro.cif}");
            Console.WriteLine($"ModelState válido: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("=== LLAMANDO AL SERVICIO ===");
                    await _centroService.CreateCentro(centro.nombre, centro.cif);
                    Console.WriteLine("=== SERVICIO COMPLETADO ===");
                    TempData["Success"] = "Centro registrado correctamente";
                    return RedirectToAction("CreateCentro");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"=== ERROR EN SERVICIO: {ex.Message} ===");
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("=== ERRORES DE VALIDACIÓN ===");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            return View(centro);
        }

        // GET: Obtener lista de centros (usado desde JavaScript)
        [HttpGet]
        public async Task<IActionResult> ObtenerCentros()
        {
            var centros = await _centroService.ObtenerCentrosAsync();
            return Json(new { ok = true, centros });
        }
    }
}