using AspNetCoreHero.ToastNotification.Abstractions;
using KingsCut.Web.Core;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.DTOs;
using KingsCut.Web.Helper;
using KingsCut.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheKingsCut.Web.Core.Pagination;

namespace KingsCut.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _notifyService;
        private readonly IUsersService _usersService;

        public UsersController(INotyfService notifyService, IUsersService usersService, ICombosHelper combosHelper)
        {
            _notifyService = notifyService;
            _usersService = usersService;
            _combosHelper = combosHelper;
        }


        [HttpGet]
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

            Response<PaginationResponse<User>> response = await _usersService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserDTO dto = new UserDTO
            {
                KingsCutRoles = await _combosHelper.GetComboKingsCutRolesAsync(),
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    dto.KingsCutRoles = await _combosHelper.GetComboKingsCutRolesAsync();
                    return View(dto);
                }

                Response<User> response = await _usersService.CreateAsync(dto);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                dto.KingsCutRoles = await _combosHelper.GetComboKingsCutRolesAsync();
                return View(dto);
            }
            catch (Exception ex)
            {
                dto.KingsCutRoles = await _combosHelper.GetComboKingsCutRolesAsync();
                return View(dto);
            }
        }
    }
}
