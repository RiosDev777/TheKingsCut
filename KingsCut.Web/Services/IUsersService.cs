using KingsCut.Web.Core;
using KingsCut.Web.Data;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.DTOs;
using KingsCut.Web.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using TheKingsCut.Web.Core.Pagination;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

namespace KingsCut.Web.Services
{
    public interface IUsersService
    {
        public Task<IdentityResult> AddUserAsync(User user, string password);
        public Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        public Task<Response<User>> CreateAsync(UserDTO dto);
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<string> GenerateEmailConfirmationTokenAsync(User user);
        public Task<Response<PaginationResponse<User>>> GetListAsync(PaginationRequest request);
        public Task<User> GetUserAsync(string email);
        public Task<User> GetUserAsync(Guid id);
        public Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        public Task<IdentityResult> UpdateUserAsync(User user);
        public Task<Response<User>> UpdateUserAsync(UserDTO dto);


    }
    public class UsersService : IUsersService
    {
        private readonly DataContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConverterHelper _converterHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(DataContext context, SignInManager<User> signInManager, UserManager<User> userManager, 
            IConverterHelper converterHelper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _converterHelper = converterHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<Response<User>> CreateAsync(UserDTO dto)
        {
            try
            {
                User user = _converterHelper.ToUser(dto);
                Guid id = Guid.NewGuid();
                user.Id = id.ToString();

                IdentityResult result = await AddUserAsync(user, dto.Document);

                string token = await GenerateEmailConfirmationTokenAsync(user);
                await ConfirmEmailAsync(user, token);

                return ResponseHelper<User>.MakeResponseSuccess(user, "Usuario creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }
        }

        public async Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module)
        {

            ClaimsUser? claimUser = _httpContextAccessor.HttpContext?.User;

            if (claimUser is null)
            {

                return false;

            }

            string? userName = claimUser.Identity.Name;

            User? user = await GetUserAsync(userName);

            if(user is null)
            {
                return false;
            }

            if(user.kingsCutRole.Name == Env.SUPER_ADMIN_ROLE_NAME)
            {

                return true;
            }

            return await _context.Permissions.Include(p => p.RolePermissions)
                                 .AnyAsync(p => (p.Module == module && p.Name == permission)
                                                && p.RolePermissions.Any(rp => rp.RoleId == user.KingsCutRoleId));

                                                    
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<Response<PaginationResponse<User>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<User> query = _context.Users.AsQueryable()
                                                       .Include(u => u.kingsCutRole);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.FirstName.ToLower().Contains(request.Filter.ToLower())
                                            || s.LastName.ToLower().Contains(request.Filter.ToLower())
                                            || s.Document.ToLower().Contains(request.Filter.ToLower())
                                            || s.Email.ToLower().Contains(request.Filter.ToLower())
                                            || s.PhoneNumber.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<User> list = await PagedList<User>.ToPagedListAsync(query, request);

                PaginationResponse<User> result = new PaginationResponse<User>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<User>>.MakeResponseSuccess(result, "Usuarios obtenidos con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<User>>.MakeResponseFail(ex);
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            User? user = await _context.Users.Include(u => u.kingsCutRole)
                                             .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _context.Users.Include(u => u.kingsCutRole)
                                       .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<Response<User>> UpdateUserAsync(UserDTO dto)
        {
            try
            {
                User user = await GetUserAsync(dto.Id);
                user.PhoneNumber = dto.PhoneNumber;
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.KingsCutRoleId = dto.KingsCutRoleId;

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return ResponseHelper<User>.MakeResponseSuccess(user, "Usuario actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }
        }
    }
}
