using KingsCut.Web.Core;
using KingsCut.Web.Data;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.Helper;
using Microsoft.EntityFrameworkCore;

namespace KingsCut.Web.Services
{
    public interface IProductsService
    {
        public Task<Response<Product>> CreateAsync(Product model);
        public Task<Response<Product>> EditAsync(Product model);
        public Task<Response<List<Product>>> GetListAsync();
        public Task<Response<Product>> GetOneAsync(int id);
        public Task<Response<Product>> DeleteteAsync(int id);
        public Task<Response<Product>> GetDetailsAsync(int id);

    }

    public class ProductService : IProductsService
    {
        private readonly DataContext _context;
        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Product>> CreateAsync(Product model)
        {
            try
            {

                Product product = new Product
                {

                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    IsActive = model.IsActive
                };
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return ResponseHelper<Product>.MakeResponseSuccess(product, "Producto creado con éxito");



            }
            catch (Exception ex)
            {

                return ResponseHelper<Product>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<Product>> DeleteteAsync(int id)
        {
            try
            {
                Response<Product> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return response;
                }

                _context.Products.Remove(response.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<Product>.MakeResponseSuccess(null, "Producto eliminado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Product>> EditAsync(Product model)
        {
            try
            {
                _context.Products.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Product>.MakeResponseSuccess(model, "Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Product>> GetDetailsAsync(int id)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);

                if (product is null)
                {
                    return ResponseHelper<Product>.MakeResponseFail("El producto con el id indicado no existe");
                }

                return ResponseHelper<Product>.MakeResponseSuccess(product);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<Product>>> GetListAsync()
        {
            try
            {

                List<Product> products = await _context.Products.ToListAsync();

                return ResponseHelper<List<Product>>.MakeResponseSuccess(products);
            
            
            }
            catch(Exception ex)
            {

                return ResponseHelper<List<Product>> .MakeResponseFail(ex);

            }
        }

        public async Task<Response<Product>> GetOneAsync(int id)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(s => s.Id == id);

                if (product is null)
                {
                    return ResponseHelper<Product>.MakeResponseFail("El producto con el id indicado no existe");
                }

                return ResponseHelper<Product>.MakeResponseSuccess(product);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Product>.MakeResponseFail(ex);
            }
        }


    }
}
