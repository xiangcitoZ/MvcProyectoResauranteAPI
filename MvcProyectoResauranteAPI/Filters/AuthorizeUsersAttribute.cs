using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MvcProyectoResauranteAPI.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute
        , IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
           

            var user = context.HttpContext.User;
            if(user.Identity.IsAuthenticated == false ) 
            {
                
                RouteValueDictionary rutaLogin =
                new RouteValueDictionary
                (
                    new { controller = "Managed", action = "Login" }
                );
            
            context.Result =
                new RedirectToRouteResult(rutaLogin);

                 

            }
        }
    }
}
