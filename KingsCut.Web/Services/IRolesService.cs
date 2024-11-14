using TheKingsCut.Web.Core.Pagination;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.Helper;
using Microsoft.EntityFrameworkCore;
using KingsCut.Web.Data;
using KingsCut.Web.Core;
using KingsCut.Web.DTOs;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace KingsCut.Web.Services
{
    public interface IRolesService
    {

        public Task<Response<KingsCutRole>> CreateAsync(KingsCutRoleDTO dto);
        public Task<Response<PaginationResponse<KingsCutRole>>> GetListAsync(PaginationRequest request);
        public Task<Response<KingsCutRoleDTO>> GetOneAsync(int id);
        Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();
    }

    public class RolesService : IRolesService
    {

        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public RolesService(DataContext context, IConverterHelper converterHelper)
        {

            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<KingsCutRole>> CreateAsync(KingsCutRoleDTO dto)
        {

            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {

                    KingsCutRole role = _converterHelper.ToRole(dto);
                    await _context.KingsCutRoles.AddAsync(role);

                    await _context.SaveChangesAsync();

                    int roleId = role.Id;

                    List<int> permissionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                    {

                        permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);

                    }

                    foreach (int permissionId in permissionIds) 
                    {

                        RolePermission rolePermission = new RolePermission()

                        {

                            RoleId = roleId,
                            PermissionId = permissionId

                        };

                        await _context.RolePermissions.AddAsync(rolePermission);

                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return ResponseHelper<KingsCutRole>.MakeResponseSuccess(role, "Rol Creado con éxito");

                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    return ResponseHelper<KingsCutRole>.MakeResponseFail(ex);
                }

            }

        }

        public async Task<Response<PaginationResponse<KingsCutRole>>> GetListAsync(PaginationRequest request)
        {
            try
            {

                IQueryable<KingsCutRole> query = _context.KingsCutRoles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {

                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<KingsCutRole> list = await PagedList<KingsCutRole>.ToPagedListAsync(query, request);

                PaginationResponse<KingsCutRole> result = new PaginationResponse<KingsCutRole>
                {

                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter

                };

                return ResponseHelper<PaginationResponse<KingsCutRole>>.MakeResponseSuccess(result, "Roles obtenidos con éxito");

            }
            catch (Exception ex)
            {

                return ResponseHelper<PaginationResponse<KingsCutRole>>.MakeResponseFail(ex);

            }
        }

        public async Task<Response<KingsCutRoleDTO>> GetOneAsync(int id)
        {
            try
            {
                KingsCutRole? role = await _context.KingsCutRoles.FirstOrDefaultAsync(s => s.Id == id);

                if (role is null)
                {
                    return ResponseHelper<KingsCutRoleDTO>.MakeResponseFail("El Role con el id indicado no existe");
                }

                return ResponseHelper<KingsCutRoleDTO>.MakeResponseSuccess(await _converterHelper.ToRoleDTOAsync(role));
            }
            catch (Exception ex)
            {
                return ResponseHelper<KingsCutRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            
            try
            {

                IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync();
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseSuccess(permissions);

            }
            catch(Exception ex)
            {

                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);


            }
        }
    }
}
