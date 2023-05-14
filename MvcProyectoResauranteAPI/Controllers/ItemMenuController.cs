using Microsoft.AspNetCore.Mvc;
using MvcProyectoResauranteAPI.Services;
using MvcRepasoSegundoExam.Services;
using NuggetRestauranteXZX.Models;
using MvcProyectoResauranteAPI.Helpers;
using static NuGet.Packaging.PackagingConstants;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class ItemMenuController : Controller
    {

        private ServiceApiRestaurante service;
        private ServiceStorageBlobs blob;
        private HelperPathProvider helperPath;

        public ItemMenuController(ServiceApiRestaurante service 
            , ServiceStorageBlobs blobs, HelperPathProvider helperPath)
        {
            this.service = service;
            this.blob = blobs;
            this.helperPath = helperPath;
        }
        public async Task<IActionResult> ItemMenu()
        {

            //List<BlobModel> listBlobs = await this.blob.GetBlobsAsync("imagenesrestaurante");
            
            List<ItemMenu> itemMenus = 
              await this.service.GetItemMenuAsync();
            return View(itemMenus);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemMenu menu, IFormFile fichero)
        {

            string fileName = fichero.FileName;

            string path = this.helperPath.MapPath(fileName, Helpers.Folders.Images);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }
            ViewData["MENSAJE"] = "Fichero subido a " + path;
            await this.service.InsertItemMenuAsync
                (menu.IdMenu, menu.Nombre, menu.Categoria,
                fileName, menu.Precio);
            return RedirectToAction("ItemMenu");
        }

        public async Task<IActionResult> Edit(int idmenu)
        {
            ItemMenu menu = await this.service.FindItemMenuAsync(idmenu);
            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ItemMenu menu)
        {
            await this.service.UpdateItemMenuAsync
               (menu.IdMenu, menu.Nombre, menu.Categoria,
                menu.Imagen, menu.Precio);
            return RedirectToAction("ItemMenu");
        }

        public async Task<IActionResult> Delete(int idmenu)
        {
            await this.service.DeleteItemMenuAsync(idmenu);
            return RedirectToAction("ItemMenu");
        }

    }
}
