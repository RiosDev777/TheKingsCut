using System.ComponentModel.DataAnnotations;

namespace KingsCut.Web.Data.Entities
{
    public class KingsCutRole
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El Campo {0} debetner maximo {1} caracteres.")]
        [Required(ErrorMessage = "El Campo {0} es requerido.")]
        public string Name { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } 
    }
}
