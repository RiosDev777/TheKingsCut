using static System.Collections.Specialized.BitVector32;

namespace KingsCut.Web.Data.Entities
{
    public class RoleService
    {
       
        public int RoleId { get; set; }
        public KingsCutRole Role { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
