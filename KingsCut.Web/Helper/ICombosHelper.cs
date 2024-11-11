using KingsCut.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KingsCut.Web.Helper
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboKingsCutRolesAsync();

    }

    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboKingsCutRolesAsync()
        {
            List<SelectListItem> list = await _context.KingsCutRoles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToListAsync();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un rol...]",
                Value = "0"
            });

            return list;
        }
    }
}
