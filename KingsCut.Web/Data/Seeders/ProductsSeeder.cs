using Microsoft.EntityFrameworkCore;
using KingsCut.Web.Data.Entities;

namespace KingsCut.Web.Data.Seeders
{
    public class ProductsSeeder
    {
        private readonly DataContext _context;

        public ProductsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Product> products = new List<Product>
        {
        new Product
        {
            Name = "Corte",
            Price = 15000,
            Description = "Realizado",
            IsActive = true

        }
    };

            foreach (Product product in products)
            {
                bool exists = await _context.Products.AnyAsync(s => s.Name == product.Name);

                if (!exists)
                {
                    await _context.Products.AddAsync(product);
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
