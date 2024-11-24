using KingsCut.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;


namespace KingsCut.Web.DTOs
{
    public class KingsCutRoleDTO
    {

        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El Campo {0} debetner maximo {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        public string Name { get; set; } = null!;

        public List<PermissionForDTO>? Permissions { get; set; }
        public string? PermissionIds { get; set; }
        public List<ServiceForDTO>? Service { get; set; }
        public string? ServicesIds { get; set; }
        
    }
}
