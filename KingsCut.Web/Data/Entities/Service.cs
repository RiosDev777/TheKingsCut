using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingsCut.Web.Data.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Servicio")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Tipo de Servicio")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string? ServiceType { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        public ICollection<RoleService>? RoleServices { get; set; }

    }
}
