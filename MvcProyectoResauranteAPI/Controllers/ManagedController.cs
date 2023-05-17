using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcProyectoResauranteAPI.Services;

using System.Security.Claims;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class ManagedController : Controller
    {

        private ServiceApiRestaurante service;
        private ServiceStorageBlobs serviceblobs;

        public ManagedController(ServiceApiRestaurante service, ServiceStorageBlobs serviceblobs)
        {
            this.service = service;
            this.serviceblobs = serviceblobs;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            string token = await this.service.GetTokenAsync(username, password);

            if (token == null)
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
            }
            else
            {
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity =
                    new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                identity.AddClaim(new Claim(ClaimTypes.Name, username));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, password));
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal, new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                        });
                return RedirectToAction("Mesa", "Mesa");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("TOKEN");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string nombre, string email, string password, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceblobs.UploadBlobAsync
                ("imagenesrestaurante", blobName, stream);

            }

            await this.service.GetRegisterUserAsync(nombre, email, password, blobName);

            return RedirectToAction("Login");
        }


        //public IActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Login
        //    (string username, string password)
        //{   
        //    if(username.ToLower() == "admin"
        //        && password.ToLower() == "admin") 
        //    {

        //        ClaimsIdentity identity =
        //            new ClaimsIdentity(
        //                CookieAuthenticationDefaults.AuthenticationScheme,
        //                ClaimTypes.Name, ClaimTypes.Role);

        //        Claim claimUserName = 
        //            new Claim(ClaimTypes.Name, username);
        //        Claim claimRole = 
        //            new Claim(ClaimTypes.Role, password);
        //        identity.AddClaim(claimUserName);
        //        identity.AddClaim(claimRole);

        //        ClaimsPrincipal userPrincipal = 
        //            new ClaimsPrincipal(identity);

        //        await HttpContext.SignInAsync
        //            (
        //                CookieAuthenticationDefaults.AuthenticationScheme,
        //                userPrincipal,
        //                new AuthenticationProperties
        //                {
        //                    ExpiresUtc = DateTime.Now.AddMinutes(5)
        //                }
        //            );
        //        return RedirectToAction("Mesa", "Mesa");

        //    }
        //    else
        //    {
        //        ViewData["MENSAJE"] = "Credenciales incorrectas";
        //        return View();
        //    }




        //}

        //public async Task<IActionResult> LogOut()
        //{
        //    await HttpContext.SignOutAsync
        //        (CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
