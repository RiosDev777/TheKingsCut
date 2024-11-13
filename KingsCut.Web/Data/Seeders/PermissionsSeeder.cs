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
            List<Permission> permissions = [.. Product(), .. Service(), .. Users()];

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
                new Permission { Name = "showProduct", Description = "Ver Productos", Module = "Productos" },
                new Permission { Name = "createProduct", Description = "Crear Productos", Module = "Productos" },
                new Permission { Name = "editProduct", Description = "Editar Productos", Module = "Productos" },
                new Permission { Name = "deleteProduct", Description = "Eliminar Productos", Module = "Productos" },
            };
        }

        private List<Permission> Service()
        {
            return new List<Permission>
            {
                new Permission { Name = "showService", Description = "Ver Servicios", Module = "Servicios" },
                new Permission { Name = "createService", Description = "Crear Servicios", Module = "Servicios" },
                new Permission { Name = "editService", Description = "Editar Servicios", Module = "Servicios" },
                new Permission { Name = "deleteService", Description = "Eliminar Servicios", Module = "Servicios" },
            };
        }

        private List<Permission> Users()
        {
            return new List<Permission>
            {
                new Permission { Name = "showUser", Description = "Ver Usuarios", Module = "Usuarios" },
                new Permission { Name = "createUser", Description = "Crear Usuarios", Module = "Usuarios" },
                new Permission { Name = "editUser", Description = "Editar Usuarios", Module = "Usuarios" },
                new Permission { Name = "deleteUser", Description = "Eliminar Usuarios", Module = "Usuarios" },
            };
        }
    }
}
