using Microsoft.AspNetCore.Mvc;

namespace BibliotecaUNAPEC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AcercaDe()
        {
            return View();
        }
    }
}
