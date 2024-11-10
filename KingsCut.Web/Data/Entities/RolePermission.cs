using System.ComponentModel.DataAnnotations;

namespace KingsCut.Web.Data.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }

        public KingsCutRole Role { get; set; }

        public int PermissionId { get; set; }

        public Permission Permission { get; set; }

    }
}
