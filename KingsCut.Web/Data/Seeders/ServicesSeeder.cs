using Microsoft.EntityFrameworkCore;
using KingsCut.Web.Data.Entities;

namespace KingsCut.Web.Data.Seeders
{
    public class ServicesSeeder
    {
        private readonly DataContext _context;

        public ServicesSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Service> services = new List<Service>
        {
        new Service
        {
            Name = "Corte",
            ServiceType = "Sencillo",
            Price = 15000,
            Description = "Realizado"
        }
    };

            foreach (Service service in services)
            {
                bool exists = await _context.Services.AnyAsync(s => s.Name == service.Name);

                if (!exists)
                {
                    await _context.Services.AddAsync(service);
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
