using ManoloLimitada.Data;
using ManoloLimitada.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ManoloLimitada.Controllers
{
    public class ContactosController : Controller
    {
        private readonly AppDbContext _context;

        public ContactosController(AppDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(
            Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var administrador = HttpContext.Session.GetString("AdministradorCorreo");

            if (string.IsNullOrEmpty(administrador))
            {
                context.Result = RedirectToAction(
                    "Login",
                    "Account"
                );

                return;
            }

            base.OnActionExecuting(context);
        }

        // GET: Contactos
        public async Task<IActionResult> Index()
        {
            var contactos = await _context.Contactos
                .OrderBy(c => c.Apellidos)
                .ThenBy(c => c.Nombre)
                .ToListAsync();

            return View(contactos);
        }

        // POST: Contactos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contacto contacto)
        {
            // Comprobar si la cédula ya existe
            bool cedulaExiste = await _context.Contactos
                .AnyAsync(c => c.Cedula == contacto.Cedula);

            if (cedulaExiste)
            {
                TempData["Error"] =
                    "Ya existe un contacto registrado con esta cédula.";

                return RedirectToAction(nameof(Index));
            }

            // Comprobar fecha futura
            if (contacto.FechaNacimiento.HasValue &&
                contacto.FechaNacimiento.Value.Date > DateTime.Today)
            {
                TempData["Error"] =
                    "La fecha de nacimiento no puede ser futura.";

                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] =
                    "Por favor, complete correctamente todos los campos.";

                return RedirectToAction(nameof(Index));
            }

            _context.Contactos.Add(contacto);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Contacto registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // POST: Contactos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Contacto contacto)
        {
            // Comprobar fecha futura
            if (contacto.FechaNacimiento.HasValue &&
                contacto.FechaNacimiento.Value.Date > DateTime.Today)
            {
                TempData["Error"] =
                    "La fecha de nacimiento no puede ser futura.";

                return RedirectToAction(nameof(Index));
            }

            // Comprobar cédula duplicada excluyendo el contacto actual
            bool cedulaExiste = await _context.Contactos
                .AnyAsync(c =>
                    c.Cedula == contacto.Cedula &&
                    c.Id != contacto.Id);

            if (cedulaExiste)
            {
                TempData["Error"] =
                    "Ya existe otro contacto registrado con esta cédula.";

                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] =
                    "Por favor, complete correctamente todos los campos.";

                return RedirectToAction(nameof(Index));
            }

            var contactoExistente = await _context.Contactos
                .FindAsync(contacto.Id);

            if (contactoExistente == null)
            {
                TempData["Error"] = "El contacto no fue encontrado.";

                return RedirectToAction(nameof(Index));
            }

            contactoExistente.Cedula = contacto.Cedula;
            contactoExistente.Nombre = contacto.Nombre;
            contactoExistente.Apellidos = contacto.Apellidos;
            contactoExistente.FechaNacimiento = contacto.FechaNacimiento;
            contactoExistente.Telefono = contacto.Telefono;
            contactoExistente.Direccion = contacto.Direccion;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Contacto actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // POST: Contactos/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var contacto = await _context.Contactos
                .FindAsync(id);

            if (contacto == null)
            {
                TempData["Error"] = "El contacto no fue encontrado.";

                return RedirectToAction(nameof(Index));
            }

            _context.Contactos.Remove(contacto);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Contacto eliminado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}