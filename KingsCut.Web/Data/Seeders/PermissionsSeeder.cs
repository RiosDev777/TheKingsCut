using KingsCut.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingsCut.Web.Data.Seeders
{
    public class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = [.. Product(), .. Service()];

            foreach (Permission permission in permissions)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Name == permission.Name
                                                                        && p.Module == permission.Module);

                if (!exists)
                {
                    await _context.Permissions.AddAsync(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<Permission> Product()
        {
            return new List<Permission>
            {
                new Permission { Name = "showProduct", Description = "Ver Product", Module = "Product" },
                new Permission { Name = "createProduct", Description = "Crear Product", Module = "Product" },
                new Permission { Name = "editProduct", Description = "Editar Product", Module = "Product" },
                new Permission { Name = "deleteProduct", Description = "Eliminar Product", Module = "Product" },
            };
        }

        private List<Permission> Service()
        {
            return new List<Permission>
            {
                new Permission { Name = "showService", Description = "Ver Service", Module = "Service" },
                new Permission { Name = "createService", Description = "Crear Service", Module = "Service" },
                new Permission { Name = "editService", Description = "Editar Service", Module = "Service" },
                new Permission { Name = "deleteService", Description = "Eliminar Service", Module = "Service" },
            };
        }
    }
}
