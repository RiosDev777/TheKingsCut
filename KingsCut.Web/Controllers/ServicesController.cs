using AspNetCoreHero.ToastNotification.Abstractions;
using KingsCut.Web.Core;
using KingsCut.Web.Core.Attributes;
using KingsCut.Web.Data;
using KingsCut.Web.Data.Entities;
using KingsCut.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheKingsCut.Web.Core.Pagination;
using static System.Collections.Specialized.BitVector32;

namespace KingsCut.Web.Controllers
{

    [Authorize]
    public class ServicesController : Controller
    {

        private readonly IServicesServices _servicesService;
        private readonly INotyfService _notifyService;

        public ServicesController(IServicesServices ServicesService, INotyfService notyfService)
        {
            _servicesService = ServicesService;
            _notifyService = notyfService;
        }


        [CustomAuthorized(permission: "showService", module: "Servicios")]
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

            Response<PaginationResponse<Service>> response = await _servicesService.GetListAsync(request);
            return View(response.Result);
        }

        
        [HttpGet]
        [CustomAuthorized(permission: "createService", module: "Servicios")]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [CustomAuthorized(permission: "createService", module: "Servicios")]
        public async Task<IActionResult> Create([Bind("Id,Name,ServiceType,Price,Description")] Service service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response<Service> response = await _servicesService.CreateAsync(service);
                    if (response.IsSuccess)
                    {
                        _notifyService.Success("El servicio ha sido creado satisfactoriamente");
                        return RedirectToAction(nameof(Index));
                    }
                    // TODO: Mostrar mensaje de error si no se creó el usero
                    ModelState.AddModelError("", response.Message);
                }

                return View(service);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(service);
            }
        }

        [CustomAuthorized(permission: "showService", module: "Servicios")]
        public async Task<IActionResult> Details(int id)
        {
            Response<Service> response = await _servicesService.GetDetailsAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }


            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }


        

        

        [HttpGet]
        [CustomAuthorized(permission: "editService", module: "Servicios")]

        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Service> response = await _servicesService.GetOneAsync(id);

            if (response.IsSuccess)
            {

                return View(response.Result);
            }

            //TODO: mensaje error

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [CustomAuthorized(permission: "editService", module: "Servicios")]
        public async Task<IActionResult> Edit(Service service)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    //TODO: mensaje de error
                    return View(service);
                }

                Response<Service> response = await _servicesService.EditAsync(service);

                if (response.IsSuccess)
                {
                    _notifyService.Success("El servicio se ha editado satisfactoriamente");
                    return RedirectToAction(nameof(Index));
                }

                return View(response);
            }
            catch (Exception ex)
            {

                //TODO: mensaje de error
                return View(service);
            }
        }

        [HttpPost]
        [CustomAuthorized(permission: "deleteService", module: "Servicios")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Service> response = await _servicesService.DeleteteAsync(id);

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
