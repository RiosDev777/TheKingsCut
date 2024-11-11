using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KingsCut.Web.Data.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "El campo '{0}' debe tener máximo {1} caracteres")]
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "¿Está Activo?")]
        public bool IsActive { get; set; }
    }
}
