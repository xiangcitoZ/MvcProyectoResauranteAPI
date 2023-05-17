using Microsoft.AspNetCore.Mvc;
using MvcProyectoResauranteAPI.Services;

using NuggetRestauranteXZX.Models;
using static NuGet.Packaging.PackagingConstants;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class ItemMenuController : Controller
    {

        private ServiceApiRestaurante service;
        private ServiceStorageBlobs blob;
        

        public ItemMenuController(ServiceApiRestaurante service 
            , ServiceStorageBlobs blobs)
        {
            this.service = service;
            this.blob = blobs;
            
        }
        public async Task<IActionResult> ItemMenu()
        {

            //List<BlobModel> listBlobs = await this.blob.GetBlobsAsync("imagenesrestaurante");
            
            List<ItemMenu> itemMenus = 
              await this.service.GetItemMenuAsync();
            return View(itemMenus);
        }


        public IActionResult Create(string containerName)
        {
            ViewData["CONTAINER"] = containerName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemMenu menu, string containerName, IFormFile file)
        {

            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.blob.UploadBlobAsync
                    (containerName, blobName, stream);

            }

            await this.service.InsertItemMenuAsync
                (menu.IdMenu, menu.Nombre, menu.Categoria,
                blobName, menu.Precio);
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

        public async Task<IActionResult> Delete(int idmenu ,string containerName, string blobName)
        {
            await this.blob.DeleteBlobAsync(containerName, blobName);
            await this.service.DeleteItemMenuAsync(idmenu);
            return RedirectToAction("ItemMenu");
        }

    }
}
