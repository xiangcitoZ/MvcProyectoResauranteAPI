using Microsoft.AspNetCore.Mvc;
using MvcProyectoResauranteAPI.Services;
using NuggetRestauranteXZX.Models;
using static NuGet.Packaging.PackagingConstants;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class ItemMenuController : Controller
    {

        private ServiceApiRestaurante service;

        public ItemMenuController(ServiceApiRestaurante service)
        {
            this.service = service;
        }
        public async Task<IActionResult> ItemMenu()
        {
            List<ItemMenu> itemMenus = 
              await this.service.GetItemMenuAsync();
            return View(itemMenus);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemMenu menu)
        {


            

            await this.service.InsertItemMenuAsync
                (menu.IdMenu, menu.Nombre, menu.Categoria,
                menu.Imagen, menu.Precio);
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
