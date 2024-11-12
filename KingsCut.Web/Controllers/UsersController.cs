using AspNetCoreHero.ToastNotification.Abstractions;
using KingsCut.Web.Core;
using KingsCut.Web.Data;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace KingsCut.Web.Controllers
{
    public class UsersController : Controller
    {
<<<<<<< Updated upstream

        private readonly IUsersServices _usersService;
        private readonly INotyfService _notifyService;

        public UsersController(IUsersServices UsersService, INotyfService notyfService)
        {
            _usersService = UsersService;
            _notifyService = notyfService;
=======
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _notifyService;
        private readonly IUsersService _usersService;
        private readonly IConverterHelper _converterHelper;

        public UsersController(INotyfService notifyService, IUsersService usersService, ICombosHelper combosHelper, IConverterHelper converterHelper)
        {
            _notifyService = notifyService;
            _usersService = usersService;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
>>>>>>> Stashed changes
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<User>> response = await _usersService.GetListAsync();
            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        


        public async Task<IActionResult> Details(int id)
        {
            Response<User> response = await _usersService.GetDetailsAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }


            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }


        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Contact,Description,IsActive")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response<User> response = await _usersService.CreateAsync(user);
                    if (response.IsSuccess)
                    {
                        _notifyService.Success("El usuario ha sido creado satisfactoriamente");
                        return RedirectToAction(nameof(Index));
                    }
                    // TODO: Mostrar mensaje de error si no se creó el usero
                    ModelState.AddModelError("", response.Message);
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(user);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<User> response = await _usersService.GetOneAsync(id);

            if (response.IsSuccess)
            {

                return View(response.Result);
            }

            //TODO: mensaje error

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    //TODO: mensaje de error
                    return View(user);
                }

                Response<User> response = await _usersService.EditAsync(user);

                if (response.IsSuccess)
                {
                    _notifyService.Success("El usuario se ha editado satisfactoriamente");
                    return RedirectToAction(nameof(Index));
                }

                return View(response);
            }
            catch (Exception ex)
            {

                //TODO: mensaje de error
                return View(user);
            }
        }

<<<<<<< Updated upstream
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<User> response = await _usersService.DeleteteAsync(id);
=======
        [HttpGet]
        public async Task<IActionResult>Edit(Guid id)
        {
            if (Guid.Empty.Equals(id))
            {
                return NotFound();
            }

            User user = await _usersService.GetUserAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            UserDTO dto = await _converterHelper.ToUserDTOAsync(user, false);
            
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.KingsCutRoles = await _combosHelper.GetComboKingsCutRolesAsync();
                return View(dto);
            }

            Response<User> response = await _usersService.UpdateUserAsync(dto);
>>>>>>> Stashed changes

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
<<<<<<< Updated upstream
            }
            else
            {

                _notifyService.Error(response.Message);

            }


            return RedirectToAction(nameof(Index));


        }




=======
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(response.Message);
            dto.KingsCutRoles = await _combosHelper.GetComboKingsCutRolesAsync();
            return View(dto);
        }
>>>>>>> Stashed changes
    }
}