using Microsoft.AspNetCore.Mvc;
using MvcProyectoResauranteAPI.Services;
using NuggetRestauranteXZX.Models;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class MesaController : Controller
    {
        private ServiceApiRestaurante service;

        public MesaController(ServiceApiRestaurante service)
        {
            this.service = service;
        }


       // [AuthorizeUsers]
        public async Task<IActionResult> Mesa()
        {
            List<Mesa> Mesas = 
                await this.service.GetMesaAsync();
            return View(Mesas);
        }

        public async Task<IActionResult> ListaMesa()
        {
            List<Mesa> Mesas = await this.service.GetMesaAsync();
            return View(Mesas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Mesa mesa)
        {
            await this.service.InsertMesaAsync
                (
                mesa.IdMesa
                ,mesa.Estado
                , mesa.Cantidad);
            return RedirectToAction("Mesa");
        }

        public async Task<IActionResult> Edit(int idmesa)
        {
            Mesa mesa = 
                await this.service.FindMesaAsync(idmesa);
            return View(mesa);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Mesa mesa)
        {
            await this.service.UpdateMesaAsync
                (mesa.IdMesa, mesa.Estado
                , mesa.Cantidad);
            return RedirectToAction("Mesa");
        }

        public async Task<IActionResult> Delete(int idmesa)
        {
            await this.service.DeleteMesaAsync(idmesa);
            return RedirectToAction("Mesa");
        }

    }
}
