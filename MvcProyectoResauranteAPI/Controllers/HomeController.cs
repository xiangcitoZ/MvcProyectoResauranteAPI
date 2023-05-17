using Microsoft.AspNetCore.Mvc;

using MvcProyectoResauranteAPI.Services;

using NuggetRestauranteXZX.Models;
using System.Diagnostics;
using ErrorViewModel = NuggetRestauranteXZX.Models.ErrorViewModel;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class HomeController : Controller
    {
        private ServiceApiRestaurante service;
        private readonly ILogger<HomeController> _logger;
        private ServiceStorageBlobs blob;

        public HomeController(ILogger<HomeController> logger, ServiceApiRestaurante service, ServiceStorageBlobs blob)
        {
            _logger = logger;
            this.service = service;
            this.blob = blob;
        }

        public async Task<IActionResult> Index(int idmesa, string descripcion, string estado)
        {

            List<BlobModel> listBlobs = await this.blob.GetBlobsAsync("imagenesrestaurante");
            ViewData["IMAGENES"] = listBlobs;

            DatosMenuPedidos datos = new DatosMenuPedidos();
            datos.Items = await this.service.GetItemMenuAsync();
            datos.Pedidos = await this.service.GetPedidoMesaAsync(idmesa);
            datos.Items = await  this.service.GetItemMenuCategoriaAsync(descripcion);
            datos.Mesas = await this.service.GetMesaAsync();

            ViewData["IDMESA"] = idmesa;
            ViewData["PEDIDO"] = datos.Pedidos;


            return View(datos);
        }

        public async Task<IActionResult> PagarPedido(int idmesa)
        {

            await this.service.GetMesaLibreAsync(idmesa);
            await this.service.PagarPedido(idmesa);
            return RedirectToAction("Index", "Home", new { idmesa = idmesa, descripcion = "Arroz" });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}