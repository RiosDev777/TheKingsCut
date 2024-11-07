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
    public class ProductsController : Controller
    {

        private readonly IProductsService _productsService;
        private readonly INotyfService _notifyService;

        public ProductsController(IProductsService productsService, INotyfService notyfService)
        {
            _productsService = productsService;
            _notifyService = notyfService;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            Response<List<Product>> response = await _productsService.GetListAsync();
            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        

        //ARREGLAR


        public async Task<IActionResult> Details(int id)
        {
            Response<Product> response = await _productsService.GetDetailsAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            
            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }


        //ARREGLAR FIN

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,IsActive")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response<Product> response = await _productsService.CreateAsync(product);
                    if (response.IsSuccess)
                    {
                        _notifyService.Success("El producto ha sido creado satisfactoriamente");
                        return RedirectToAction(nameof(Index));
                    }
                    

                    // TODO: Mostrar mensaje de error si no se creó el producto
                    ModelState.AddModelError("", response.Message);
                }

                

                return View(product);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(product);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Product> response = await _productsService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                
                return View(response.Result);
            }

            //TODO: mensaje error

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    //TODO: mensaje de error
                    return View(product);
                }

                Response<Product> response = await _productsService.EditAsync(product);

                if (response.IsSuccess)
                {
                    _notifyService.Success("El producto se ha editado satisfactoriamente");
                    return RedirectToAction(nameof(Index));
                }

                return View(response);
            }
            catch (Exception ex)
            {

                //TODO: mensaje de error
                return View(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Product> response = await _productsService.DeleteteAsync(id);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
            }else
            {

                _notifyService.Error(response.Message);

            }

                
            return RedirectToAction(nameof(Index));

            
        }




    }
}
