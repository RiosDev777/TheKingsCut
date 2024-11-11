using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KingsCut.Web.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe terner máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombres")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe terner máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe terner máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El campo {0} deve ser un Email válido")]
        public string Email { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";

        public int KingsCutRoleId { get; set; }

        public IEnumerable<SelectListItem>? KingsCutRoles { get; set; }
    }
}
