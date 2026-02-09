using Microsoft.AspNetCore.Mvc;
using AyudaIguales.Models;
using AyudaIguales.Services;

namespace AyudaIguales.Controllers
{
    public class ValoracionController : Controller
    {
        private readonly IValoracionService _valoracionService;

        public ValoracionController(IValoracionService valoracionService)
        {
            _valoracionService = valoracionService;
        }

        // POST: Crear una nueva valoracion desde el detalle de ayuda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(int id_respuesta, int id_ayuda, int nota)
        {
            // Verificar si hay sesion iniciada
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            int id_usuario = int.Parse(userIdString);

            // Crear el request para la valoracion
            var request = new CrearValoracionRequest
            {
                id_respuesta = id_respuesta,
                id_usuario = id_usuario,
                nota = nota
            };

            // Llamar al servicio para crear la valoracion
            var resultado = await _valoracionService.CrearValoracionAsync(request);

            if (resultado.ok)
            {
                TempData["Success"] = "Valoración enviada correctamente";
            }
            else
            {
                TempData["Error"] = resultado.msg;
            }

            // Redirigir de vuelta al detalle de la ayuda
            return RedirectToAction("DetalleAyuda", "Ayuda", new { id = id_ayuda });
        }
    }
}