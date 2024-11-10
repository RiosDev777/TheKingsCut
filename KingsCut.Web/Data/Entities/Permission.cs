using System.ComponentModel.DataAnnotations;

namespace KingsCut.Web.Data.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Permiso")]
        [MaxLength(64, ErrorMessage = "El Campo {0} debetner maximo {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        public string Name { get; set; } = null;

        [Display(Name = "Descripcion")]
        [MaxLength(512, ErrorMessage = "El Campo {0} debetner maximo {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        public string Description { get; set; } = null;

        [Display(Name = "Modulo")]
        [MaxLength(64, ErrorMessage = "El Campo {0} debetner maximo {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        public string Module { get; set; } = null;

        public ICollection<RolePermission> RolePermissions{ get; set; }
    }
}
