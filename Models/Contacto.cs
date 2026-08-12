using System.ComponentModel.DataAnnotations;

namespace ManoloLimitada.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(20, ErrorMessage = "La cédula no puede superar los 20 caracteres.")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(150, ErrorMessage = "Los apellidos no pueden superar los 150 caracteres.")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres.")]
        public string Direccion { get; set; } = string.Empty;

        public int Edad
        {
            get
            {
                if (!FechaNacimiento.HasValue)
                {
                    return 0;
                }

                var hoy = DateTime.Today;
                var fechaNacimiento = FechaNacimiento.Value;

                var edad = hoy.Year - fechaNacimiento.Year;

                if (fechaNacimiento.Date > hoy.AddYears(-edad))
                {
                    edad--;
                }

                return edad;
            }
        }
    }
}