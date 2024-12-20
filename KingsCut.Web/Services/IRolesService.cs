﻿using TheKingsCut.Web.Core.Pagination;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.Helper;
using Microsoft.EntityFrameworkCore;
using KingsCut.Web.Data;
using KingsCut.Web.Core;
using KingsCut.Web.DTOs;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using static System.Collections.Specialized.BitVector32;

namespace KingsCut.Web.Services
{
    public interface IRolesService
    {
        Task<Response<KingsCutRole>> CreateAsync(KingsCutRoleDTO dto);
        Task<Response<KingsCutRole>> DeleteAsync(int id);
        Task<Response<KingsCutRole>> EditAsync(KingsCutRoleDTO dto);
        Task<Response<PaginationResponse<KingsCutRole>>> GetListAsync(PaginationRequest request);
        Task<Response<KingsCutRoleDTO>> GetOneAsync(int id);
        Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();
        Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);
        Task<Response<IEnumerable<Service>>> GetServicesAsync();
        Task<Response<IEnumerable<ServiceForDTO>>> GetServicesByRoleAsync(int id);
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
                        RolePermission rolePermission = new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }

                    List<int> serviceIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.ServicesIds))
                    {
                        serviceIds = JsonConvert.DeserializeObject<List<int>>(dto.ServicesIds);
                    }

                    foreach (int serviceId in serviceIds)
                    {
                        RoleService rolePermission = new RoleService
                        {
                            RoleId = roleId,
                            ServiceId = serviceId
                        };

                        await _context.RoleServices.AddAsync(rolePermission);
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

        public Task<Response<KingsCutRole>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
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
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<KingsCutRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;

                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }
       
        public async Task<Response<KingsCutRole>> EditAsync(KingsCutRoleDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<KingsCutRole>.MakeResponseFail($"El Role '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado");
                }

                List<int> permissionsIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                {
                    permissionsIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                foreach (int permissionsId in permissionsIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionsId
                    };

                    await _context.RolePermissions.AddAsync(rolePermission);
                }

                List<int> serviceIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.ServicesIds))
                {
                    serviceIds = JsonConvert.DeserializeObject<List<int>>(dto.ServicesIds);
                }

                List<RoleService> oldRoleServices = await _context.RoleServices.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RoleServices.RemoveRange(oldRoleServices);

                foreach (int serviceId in serviceIds)
                {
                    RoleService roleService = new RoleService
                    {
                        RoleId = dto.Id,
                        ServiceId = serviceId
                    };

                    await _context.RoleServices.AddAsync(roleService);
                }

                KingsCutRole model = _converterHelper.ToRole(dto);
                _context.KingsCutRoles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<KingsCutRole>.MakeResponseSuccess(model, "Rol Actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<KingsCutRole>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Service>>> GetServicesAsync()
        {
            try
            {
                IEnumerable<Service> Services = await _context.Services.ToListAsync();

                return ResponseHelper<IEnumerable<Service>>.MakeResponseSuccess(Services);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Service>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<ServiceForDTO>>> GetServicesByRoleAsync(int id)
        {
            try
            {
                Response<KingsCutRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<ServiceForDTO>>.MakeResponseFail(response.Message);
                }

                List<ServiceForDTO> services = response.Result.Service;

                return ResponseHelper<IEnumerable<ServiceForDTO>>.MakeResponseSuccess(services);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<ServiceForDTO>>.MakeResponseFail(ex);
            }
        }
    }
}
