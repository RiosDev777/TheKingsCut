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

            PaginationRequest request = new PaginationRequest
            {

                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter

            };

            Response<PaginationResponse<KingsCutRole>> response = await _rolesService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorized(permission: "createRole", module: "Roles")]

        public async Task<IActionResult> Create()

        {

            Response<IEnumerable<Permission>> response = await _rolesService.GetPermissionsAsync();
            if (!response.IsSuccess)

            {

                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));

            }

            KingsCutRoleDTO dto = new KingsCutRoleDTO

            {

                Permissions = response.Result.Select(p => new PermissionForDTO
                {

                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,

                }).ToList()
            };


            return View(dto);
        }

        [HttpPost]
        [CustomAuthorized(permission: "createRole", module: "Roles")]

        public async Task<IActionResult> Create(KingsCutRoleDTO dto)
        {

            if (!ModelState.IsValid)
            {

                

                _notifyService.Error("Debe ajustar los errores de validación");

                Response<IEnumerable<Permission>> response1 = await _rolesService.GetPermissionsAsync();


                dto.Permissions = response1.Result.Select(p => new PermissionForDTO
                {

                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,

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

            Response<IEnumerable<Permission>> response = await _rolesService.GetPermissionsAsync();


            dto.Permissions = response.Result.Select(p => new PermissionForDTO
            {

                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,

            }).ToList();

            return View(dto);
        }
    
    }
}
