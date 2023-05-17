using Microsoft.AspNetCore.Mvc;
using MvcProyectoResauranteAPI.Filters;
using MvcProyectoResauranteAPI.Services;
using NuggetRestauranteXZX.Models;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class UsuariosController : Controller
    {
        
            private ServiceApiRestaurante service;
            private ServiceStorageBlobs serviceblob;

            public UsuariosController(ServiceApiRestaurante service, ServiceStorageBlobs serviceblob)
            {
                this.service = service;
                this.serviceblob = serviceblob;
            }
            [AuthorizeUsers]
            public IActionResult Index()
            {
                return View();
            }

            [AuthorizeUsers]
            public async Task<IActionResult> Perfil()
            {
                string token =
                    HttpContext.Session.GetString("TOKEN");
                Usuario usuario = await
                    this.service.GetPerfilUsuarioAsync(token);
                BlobModel blobPerfil = await this.serviceblob.FindBlobPerfil("imagenesrestaurante", usuario.ImagenPerfil, usuario.Nombre);
                ViewData["IMAGEN_PERFIL"] = blobPerfil;
                return View(usuario);
            }
        
    }
}
