using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MvcProyectoResauranteAPI.Controllers
{
    public class ManagedController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login
            (string username, string password)
        {   
            if(username.ToLower() == "admin"
                && password.ToLower() == "admin") 
            {

                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);

                Claim claimUserName = 
                    new Claim(ClaimTypes.Name, username);
                Claim claimRole = 
                    new Claim(ClaimTypes.Role, password);
                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);

                ClaimsPrincipal userPrincipal = 
                    new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync
                    (
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.Now.AddMinutes(5)
                        }
                    );
                return RedirectToAction("Mesa", "Mesa");
                
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }

           

           
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
