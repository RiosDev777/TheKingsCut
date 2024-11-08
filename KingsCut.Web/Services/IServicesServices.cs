using KingsCut.Web.Core;
using KingsCut.Web.Data;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.Helper;
using Microsoft.EntityFrameworkCore;
using TheKingsCut.Web.Core.Pagination;


namespace KingsCut.Web.Services

{

    public interface IServicesServices

    {
        public Task<Response<Service>> CreateAsync(Service model);
        public Task<Response<Service>> EditAsync(Service model);
        public Task<Response<PaginationResponse<Service>>> GetListAsync(PaginationRequest request);
        public Task<Response<Service>> GetOneAsync(int id);
        public Task<Response<Service>> DeleteteAsync(int id);
        public Task<Response<Service>> GetDetailsAsync(int id);

    }

    public class ServiceService : IServicesServices
    {
        private readonly DataContext _context;
        public ServiceService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Service>> CreateAsync(Service model)
        {
            try
            {

                Service service = new Service
                {

                    Name = model.Name,
                    ServiceType = model.ServiceType,
                    Price = model.Price,
                    Description = model.Description,

                };
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();

                return ResponseHelper<Service>.MakeResponseSuccess(service, "Usuario creado con éxito");



            }
            catch (Exception ex)
            {

                return ResponseHelper<Service>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<Service>> DeleteteAsync(int id)
        {
            try
            {
                Response<Service> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return response;
                }

                _context.Services.Remove(response.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<Service>.MakeResponseSuccess(null, "Servicio eliminado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Service>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Service>> EditAsync(Service model)
        {
            try
            {
                _context.Services.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Service>.MakeResponseSuccess(model, "Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Service>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Service>> GetDetailsAsync(int id)
        {
            try
            {
                Service? service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

                if (service is null)
                {
                    return ResponseHelper<Service>.MakeResponseFail("El usuario con el id indicado no existe");
                }

                return ResponseHelper<Service>.MakeResponseSuccess(service);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Service>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Service>>> GetListAsync(PaginationRequest request)
        {
            try
            {

                IQueryable<Service> query = _context.Services.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {

                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Service> list = await PagedList<Service>.ToPagedListAsync(query, request);

                PaginationResponse<Service> result = new PaginationResponse<Service>
                {

                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter

                };

                return ResponseHelper<PaginationResponse<Service>>.MakeResponseSuccess(result, "Servicio obtenido con éxito");

            }
            catch (Exception ex)
            {

                return ResponseHelper<PaginationResponse<Service>>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<Service>> GetOneAsync(int id)
        {
            try
            {
                Service? service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

                if (service is null)
                {
                    return ResponseHelper<Service>.MakeResponseFail("El Usuario con el id indicado no existe");
                }

                return ResponseHelper<Service>.MakeResponseSuccess(service);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Service>.MakeResponseFail(ex);
            }
        }


    }
}
