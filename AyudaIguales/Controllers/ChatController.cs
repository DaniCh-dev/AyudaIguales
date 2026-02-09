using Microsoft.AspNetCore.Mvc;
using AyudaIguales.Models;
using AyudaIguales.Services;

namespace AyudaIguales.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // GET: Mostrar lista de chats
        [HttpGet]
        public async Task<IActionResult> Index()
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

            int id_usuario = int.Parse(userIdString);
            int id_centro = int.Parse(centroIdString);

            // Obtener chats del usuario
            var resultado = await _chatService.ObtenerChatsAsync(id_usuario, id_centro);

            if (!resultado.ok)
            {
                TempData["Error"] = resultado.msg;
                return View(new List<Chat>());
            }

            return View(resultado.chats);
        }

        

        // POST: Crear nuevo chat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(int id_usuario2)
        {
            // Verificar si hay sesion iniciada
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "User");
            }

            int id_usuario1 = int.Parse(userIdString);

            // Crear o obtener chat
            var resultado = await _chatService.CrearChatAsync(id_usuario1, id_usuario2);

            if (resultado.ok)
            {
                TempData["Success"] = resultado.nuevo ? "Chat creado correctamente" : "Chat abierto";
                return RedirectToAction("Conversacion", "Chat", new { id = resultado.id_chat });
            }
            else
            {
                TempData["Error"] = resultado.msg;
                return RedirectToAction("Index");
            }
        }
        // GET: Mostrar modal de nuevo chat
        [HttpGet]
        public async Task<IActionResult> NuevoChat()
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
                return RedirectToAction("Index");
            }

            int id_usuario = int.Parse(userIdString);
            int id_centro = int.Parse(centroIdString);

            // Obtener usuarios disponibles
            var resultado = await _chatService.ObtenerUsuariosParaChatAsync(id_usuario, id_centro);

            if (!resultado.ok)
            {
                TempData["Error"] = resultado.msg;
                return RedirectToAction("Index");
            }

            // Pasar todo el objeto ObtenerUsuariosResponse
            return View(resultado);
        }
    }
}