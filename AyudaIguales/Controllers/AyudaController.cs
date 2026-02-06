using Microsoft.AspNetCore.Mvc;

namespace AyudaIguales.Controllers
{
    public class AyudaController : Controller
    {
        public IActionResult AyudaHome()
        {
            return View();
        }
    }
}
