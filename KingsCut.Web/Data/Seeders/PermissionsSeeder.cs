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
            List<Permission> permissions = [.. Product(), .. Service(), .. Users(), ..Roles()];

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
                new Permission { Name = "showServices", Description = "Ver Servicios", Module = "Servicios" },
                new Permission { Name = "createServices", Description = "Crear Servicios", Module = "Servicios" },
                new Permission { Name = "editServices", Description = "Editar Servicios", Module = "Servicios" },
                new Permission { Name = "deleteServices", Description = "Eliminar Servicios", Module = "Servicios" },
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

        private List<Permission> Roles()
        {
            return new List<Permission>
            {
                new Permission { Name = "showRoles", Description = "Ver Roles", Module = "Roles" },
                new Permission { Name = "createRoles", Description = "Crear Roles", Module = "Roles" },
                new Permission { Name = "editRoles", Description = "Editar Roles", Module = "Roles" },
                new Permission { Name = "deleteRoles", Description = "Eliminar Roles", Module = "Roles" },
            };
        }
    }
}
