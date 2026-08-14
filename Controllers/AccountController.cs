using ManoloLimitada.Data;
using ManoloLimitada.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManoloLimitada.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Administrador> _passwordHasher;

        private const int MaxIntentos = 5;
        private const int MinutosBloqueo = 5;

        public AccountController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Administrador>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya existe una sesión, no mostramos nuevamente el login.
            if (HttpContext.Session.GetString("AdministradorCorreo") != null)
            {
                return RedirectToAction("Index", "Contactos");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string password)
        {
            // Comprobar si existe un bloqueo temporal
            var bloqueadoHasta = HttpContext.Session.GetString("BloqueadoHasta");

            if (DateTime.TryParse(bloqueadoHasta, out DateTime fechaBloqueo))
            {
                if (DateTime.Now < fechaBloqueo)
                {
                    var minutosRestantes =
                        Math.Ceiling((fechaBloqueo - DateTime.Now).TotalMinutes);

                    ViewBag.Error =
                        $"Demasiados intentos fallidos. " +
                        $"Intenta nuevamente en {minutosRestantes} minuto(s).";

                    return View();
                }

                // El bloqueo ya terminó
                HttpContext.Session.Remove("BloqueadoHasta");
                HttpContext.Session.Remove("LoginIntentos");
            }

            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "El correo y la contraseña son obligatorios.";
                return View();
            }

            // Normalizar el correo
            correo = correo.Trim().ToLowerInvariant();

            var administrador = await _context.Administradores
                .FirstOrDefaultAsync(a =>
                    a.Correo.ToLower() == correo);

            // Si no existe el administrador, contamos el intento
            if (administrador == null)
            {
                return RegistrarIntentoFallido();
            }

            var resultado = _passwordHasher.VerifyHashedPassword(
                administrador,
                administrador.Password,
                password
            );

            if (resultado == PasswordVerificationResult.Failed)
            {
                return RegistrarIntentoFallido();
            }

            // Login correcto: limpiar intentos anteriores
            HttpContext.Session.Remove("LoginIntentos");
            HttpContext.Session.Remove("BloqueadoHasta");

            // Crear la sesión autenticada
            HttpContext.Session.SetString(
                "AdministradorCorreo",
                administrador.Correo
            );

            return RedirectToAction("Index", "Contactos");
        }

        private IActionResult RegistrarIntentoFallido()
        {
            int intentos = HttpContext.Session.GetInt32("LoginIntentos") ?? 0;

            intentos++;

            HttpContext.Session.SetInt32(
                "LoginIntentos",
                intentos
            );

            if (intentos >= MaxIntentos)
            {
                var fechaBloqueo =
                    DateTime.Now.AddMinutes(MinutosBloqueo);

                HttpContext.Session.SetString(
                    "BloqueadoHasta",
                    fechaBloqueo.ToString("O")
                );

                ViewBag.Error =
                "Demasiados intentos fallidos. " +
                "El acceso ha sido bloqueado temporalmente.";

                return View("Login");
            }

            ViewBag.Error =
                "Correo o contraseña incorrectos.";

            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Account"
            );
        }
    }
}