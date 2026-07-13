using System.ComponentModel.DataAnnotations;

namespace SmartBike_MVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo institucional es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        [Display(Name = "Correo institucional")]
        public string CorreoInstitucional { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; } = null!;

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(10, ErrorMessage = "La cédula debe tener máximo 10 caracteres")]
        [Display(Name = "Cédula")]
        public string Cedula { get; set; } = null!;
        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El correo institucional es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        [Display(Name = "Correo institucional")]
        public string CorreoInstitucional { get; set; } = null!;

        [Display(Name = "Carrera")]
        public string? Carrera { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Mínimo 8 caracteres")]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; } = null!;

        [Required(ErrorMessage = "Confirme la contraseña")]
        [DataType(DataType.Password)]
        [Compare(nameof(Contrasena), ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmarContrasena { get; set; } = null!;
    }
}