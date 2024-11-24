using System.Data;
using AspNetCoreHero.ToastNotification.Abstractions;
using KingsCut.Web.Core;
using KingsCut.Web.Core.Attributes;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.DTOs;
using KingsCut.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheKingsCut.Web.Core.Pagination;

namespace KingsCut.Web.Controllers
{
    public class RolesController : Controller
    {

        private readonly IRolesService _rolesService;
        private readonly INotyfService _notifyService;
        public RolesController(IRolesService rolesService, INotyfService notifyService)

        {
            _rolesService = rolesService;
            _notifyService = notifyService;
        }

        [HttpGet]
        [CustomAuthorized(permission: "showRole", module: "Roles")]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {

            PaginationRequest paginationrequest = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };

            Response<PaginationResponse<KingsCutRole>> response = await _rolesService.GetListAsync(paginationrequest);
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorized(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create()
        {
            Response<IEnumerable<Permission>> permissionsresponse = await _rolesService.GetPermissionsAsync();
            if (!permissionsresponse.IsSuccess)
            {
                _notifyService.Error(permissionsresponse.Message);
                return RedirectToAction(nameof(Index));
            }

            Response<IEnumerable<Service>> Servicesresponse = await _rolesService.GetServicesAsync();
            if (!Servicesresponse.IsSuccess)
            {
                _notifyService.Error(Servicesresponse.Message);
                return RedirectToAction(nameof(Index));
            }

            KingsCutRoleDTO dto = new KingsCutRoleDTO
            {
                Permissions = permissionsresponse.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList(),

                Service = Servicesresponse.Result.Select(p => new ServiceForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    
                }).ToList()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorized(permission: "createRoles", module: "Roles")]

        public async Task<IActionResult> Create(KingsCutRoleDTO dto)
        {

            if (!ModelState.IsValid)
            {               

                _notifyService.Error("Debe ajustar los errores de validación");

                Response<IEnumerable<Permission>> permissionsResponse1 = await _rolesService.GetPermissionsAsync();
                Response<IEnumerable<Service>> servicesResponse1 = await _rolesService.GetServicesAsync();


                dto.Permissions = permissionsResponse1.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList();

                dto.Service = servicesResponse1.Result.Select(p => new ServiceForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    
                }).ToList();

                return View(dto);
            }

            Response<KingsCutRole> createResponse = await _rolesService.CreateAsync(dto);


            if (createResponse.IsSuccess)
            {
                _notifyService.Success(createResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(createResponse.Message);

            Response<IEnumerable<Permission>> permissionsResponse2 = await _rolesService.GetPermissionsAsync();
            Response<IEnumerable<Service>> servicesResponse2 = await _rolesService.GetServicesAsync();

            dto.Permissions = permissionsResponse2.Result.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,

            }).ToList();

            dto.Service = servicesResponse2.Result.Select(p => new ServiceForDTO
            {
                Id = p.Id,
                Name = p.Name,                

            }).ToList();


            return View(dto);
        }

        [HttpGet]
        [CustomAuthorized(permission: "editRoles", module: "Roles")]
        public async Task<IActionResult> Edit(int id)
        {
            Response<KingsCutRoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorized(permission: "editRoles", module: "Roles")]
        public async Task<IActionResult> Edit(KingsCutRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación.");

                Response<IEnumerable<PermissionForDTO>> permissionsByRoleResponse = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
                Response<IEnumerable<ServiceForDTO>> servicesByRoleResponse = await _rolesService.GetServicesByRoleAsync(dto.Id);

                dto.Permissions = permissionsByRoleResponse.Result.ToList();
                dto.Service = servicesByRoleResponse.Result.ToList();

                return View(dto);
            }

            Response<KingsCutRole> editResponse = await _rolesService.EditAsync(dto);

            if (editResponse.IsSuccess)
            {
                _notifyService.Success(editResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(editResponse.Message);
            Response<IEnumerable<PermissionForDTO>> permissionsByRoleResponse2 = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
            Response<IEnumerable<ServiceForDTO>> servicesByRoleResponse2 = await _rolesService.GetServicesByRoleAsync(dto.Id);

            dto.Permissions = permissionsByRoleResponse2.Result.ToList();
            dto.Service = servicesByRoleResponse2.Result.ToList();

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorized("deleteRoles", "Roles")]
        public async Task<IActionResult> Delete(int id)
        {
            Response<KingsCutRole> response = await _rolesService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
