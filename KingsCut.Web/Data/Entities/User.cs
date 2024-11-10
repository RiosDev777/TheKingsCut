using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace KingsCut.Web.Data.Entities
{
    public class User : IdentityUser

    {
        [Display(Name = "Documento")]
        [MaxLength(32, ErrorMessage = "El Campo {0} debetner maximo {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        public string Document { get; set; } = null;

        [Display(Name = "Nombres")]
        [MaxLength(32, ErrorMessage = "El Campo {0} debetner maximo {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        public string FirstName { get; set; } = null;

        [Display(Name = "Apellidos")]
        [MaxLength(32, ErrorMessage = "El Campo {0} debetner maximo {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        public string LastName { get; set; } = null;

        public string FullName => $"{FirstName} {LastName}";

        public int KingsCutRoleId { get; set; }
        public KingsCutRole kingsCutRole { get; set; }
    }
}
