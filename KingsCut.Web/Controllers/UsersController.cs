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

        private readonly IUsersServices _usersService;
        private readonly INotyfService _notifyService;

        public UsersController(IUsersServices UsersService, INotyfService notyfService)
        {
            _usersService = UsersService;
            _notifyService = notyfService;
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

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<User> response = await _usersService.DeleteteAsync(id);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
            }
            else
            {

                _notifyService.Error(response.Message);

            }


            return RedirectToAction(nameof(Index));


        }




    }
}
