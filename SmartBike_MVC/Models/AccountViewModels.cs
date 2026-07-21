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
        [CedulaEcuatoriana]
        [Display(Name = "Cédula")]
  
        public string Cedula { get; set; } = null!;
        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü ]{2,80}$",
      ErrorMessage = "Los nombres solo pueden contener letras (mínimo 2 caracteres)")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü ]{2,80}$",
          ErrorMessage = "Los apellidos solo pueden contener letras (mínimo 2 caracteres)")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El correo institucional es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@utn\.edu\.ec$",
                ErrorMessage = "Debe usar su correo institucional @utn.edu.ec")]
        [Display(Name = "Correo institucional")]
        public string CorreoInstitucional { get; set; } = null!;

        [Required(ErrorMessage = "Seleccione su carrera")]
        [Display(Name = "Carrera")]
        public int? CarreraId { get; set; }

        // Lista para llenar el combo (no se envía en el POST)
        public List<Modelos.Carrera> Carreras { get; set; } = new();
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