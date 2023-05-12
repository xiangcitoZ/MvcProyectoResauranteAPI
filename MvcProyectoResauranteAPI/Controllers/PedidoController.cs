using Microsoft.AspNetCore.Mvc;
using MvcProyectoResauranteAPI.Services;
using NuggetRestauranteXZX.Models;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class PedidoController : Controller
    {

        private ServiceApiRestaurante service;

        public PedidoController(ServiceApiRestaurante service)
        {
            this.service = service;
        }
        public async Task<IActionResult>  Pedido()
        {
            List<Pedido> Pedidos = 
                await this.service.GetPedidoAsync();
            return View(Pedidos);
        }




        public IActionResult Create(int IdMenu, string ItemsMenu, string precio, int idmesa)
        {
            ViewData["IDMENU"] = IdMenu;
            ViewData["ITEMSMENU"] = ItemsMenu;
            ViewData["PRECIO"] = precio;
            ViewData["IDMESA"] = idmesa;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pedido pedido)
        {
            await this.service.InsertPedidoAsync
                (pedido.IdPedido, pedido.Precio, DateTime.Now,
                pedido.ItemsMenu, pedido.IdMesa, pedido.IdMenu, pedido.Cantidad);

            //await this.service.MesaOcupado(pedido.IdMesa);

            //return RedirectToAction
            //    ("Index", "Home", new { IdMesa = pedido.IdMesa, descripcion = "Arroz" });
            return RedirectToAction("Index","Home");
        }



        public async Task<IActionResult> Edit(int idpedido)
        {
            Pedido pedido =
                await this.service.FindPedidoAsync(idpedido);
            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pedido pedido)
        {
            await this.service.UpdatePedidoAsync
                (pedido.IdPedido, pedido.Precio, pedido.Fecha,
                pedido.ItemsMenu, pedido.IdMesa, pedido.IdMenu, pedido.Cantidad);
            return RedirectToAction("Index", "Home", new { IdMesa = pedido.IdMesa });
        }

        public async Task<IActionResult> Delete(int idpedido, Pedido pedido)
        {
            await this.service.DeletePedidoAsync(idpedido);
            return RedirectToAction("Index", "Home", new { IdMesa = pedido.IdMesa, descripcion = "Arroz" });
        }

    }
}
