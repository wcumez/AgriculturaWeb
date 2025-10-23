using System;
using System.Web;
using System.Web.Mvc;

namespace AgriculturaWeb.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        // Constructor que acepta uno o varios roles
        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Si no hay sesión activa, redirige al login
            if (HttpContext.Current.Session["Rol"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }

            string rolActual = HttpContext.Current.Session["Rol"].ToString();

            // Si el rol no está en la lista permitida
            if (Array.IndexOf(_roles, rolActual) < 0)
            {
                // Redirige a Home o a una vista de acceso denegado
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
