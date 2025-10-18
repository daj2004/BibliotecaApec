using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class AuthController : Controller
    {
        // Página de login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UsuarioLogin model)
        {
            if (model.Usuario == "admin" && model.Contrasena == "1234")
            {
                // Guardar sesión
                HttpContext.Session.SetString("Usuario", model.Usuario);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
