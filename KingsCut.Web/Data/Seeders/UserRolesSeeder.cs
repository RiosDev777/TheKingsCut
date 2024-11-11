using KingsCut.Web.Core;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace KingsCut.Web.Data.Seeders
{
    public class UserRolesSeeder
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public UserRolesSeeder(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await CheckRoles();
            await CheckUsers();
        }

        private async Task CheckRoles()
        {
            await AdminRoleAsync();
            await ContentManagerAsync();
            await UserManagerAsync();
        }

        private async Task UserManagerAsync()
        {
            bool exists = await _context.KingsCutRoles.AnyAsync(r => r.Name == "Gestor de usuarios");

            if (!exists)
            {
                KingsCutRole role = new KingsCutRole { Name = "Gestor de usuarios" };
                await _context.KingsCutRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ContentManagerAsync()
        {
            bool exists = await _context.KingsCutRoles.AnyAsync(r => r.Name == "Gestor de contenido");

            if (!exists)
            {
                KingsCutRole role = new KingsCutRole { Name = "Gestor de contenido" };
                await _context.KingsCutRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task AdminRoleAsync()
        {
            bool exists = await _context.KingsCutRoles.AnyAsync(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

            if (!exists)
            {
                KingsCutRole role = new KingsCutRole { Name = Env.SUPER_ADMIN_ROLE_NAME };
                await _context.KingsCutRoles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckUsers()
        {
            // Admin
            User? user = await _usersService.GetUserAsync("kings@yopmail.com");

            if (user is null)
            {
                KingsCutRole adminRole = _context.KingsCutRoles.FirstOrDefault(r => r.Name == Env.SUPER_ADMIN_ROLE_NAME);

                user = new User
                {
                    Email = "kings@yopmail.com",
                    FirstName = "king",
                    LastName = "cut",
                    PhoneNumber = "55555555",
                    UserName = "kings@yopmail.com",
                    Document = "222222",
                    kingsCutRole = adminRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Content Manager
            user = await _usersService.GetUserAsync("anad@yopmail.com");

            if (user is null)
            {
                KingsCutRole contentManagerRole = _context.KingsCutRoles.FirstOrDefault(r => r.Name == "Gestor de contenido");

                user = new User
                {
                    Email = "anad@yopmail.com",
                    FirstName = "Ana",
                    LastName = "Doe",
                    PhoneNumber = "31111111",
                    UserName = "anad@yopmail.com",
                    Document = "22222",
                    kingsCutRole = contentManagerRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }
        }
    }
}
