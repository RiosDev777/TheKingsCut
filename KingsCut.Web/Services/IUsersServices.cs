using KingsCut.Web.Core;
using KingsCut.Web.Data;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.Helper;
using Microsoft.EntityFrameworkCore;


namespace KingsCut.Web.Services 

{ 

    public interface IUsersServices

        {
            public Task<Response<User>> CreateAsync(User model);
            public Task<Response<User>> EditAsync(User model);
            public Task<Response<List<User>>> GetListAsync();
            public Task<Response<User>> GetOneAsync(int id);
            public Task<Response<User>> DeleteteAsync(int id);
            public Task<Response<User>> GetDetailsAsync(int id);

        }

    public class UserService : IUsersServices
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<User>> CreateAsync(User model)
        {
            try
            {

                User user = new User
                {

                    Name = model.Name,
                    Email = model.Email,
                    Contact = model.Contact,
                    Description = model.Description,
                    IsActive = model.IsActive
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return ResponseHelper<User>.MakeResponseSuccess(user, "Usuario creado con éxito");



            }
            catch (Exception ex)
            {

                return ResponseHelper<User>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<User>> DeleteteAsync(int id)
        {
            try
            {
                Response<User> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return response;
                }

                _context.Users.Remove(response.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<User>.MakeResponseSuccess(null, "Usuario eliminado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<User>> EditAsync(User model)
        {
            try
            {
                _context.Users.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<User>.MakeResponseSuccess(model, "Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<User>> GetDetailsAsync(int id)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(s => s.Id == id);

                if (user is null)
                {
                    return ResponseHelper<User>.MakeResponseFail("El usuario con el id indicado no existe");
                }

                return ResponseHelper<User>.MakeResponseSuccess(user);
            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<User>>> GetListAsync()
        {
            try
            {

                List<User> users = await _context.Users.ToListAsync();

                return ResponseHelper<List<User>>.MakeResponseSuccess(users);


            }
            catch (Exception ex)
            {

                return ResponseHelper<List<User>>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<User>> GetOneAsync(int id)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(s => s.Id == id);

                if (user is null)
                {
                    return ResponseHelper<User>.MakeResponseFail("El Usuario con el id indicado no existe");
                }

                return ResponseHelper<User>.MakeResponseSuccess(user);
            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }
        }


    }
}