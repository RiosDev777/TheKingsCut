using System.ComponentModel.DataAnnotations;

namespace KingsCut.Web.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Contact { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "¿Está activo?")]
        public bool IsActive { get; set; }
    }
}
