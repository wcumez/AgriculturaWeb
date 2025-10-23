using System.Web;
using System.Web.Mvc;

namespace AgriculturaWeb.Filters
{
    public class SesionActivaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var usuario = HttpContext.Current.Session["Usuario"];
            var controlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var accion = filterContext.ActionDescriptor.ActionName;

            // Permitir acceso libre al Login
            if (controlador == "Login")
                return;

            // Si no hay usuario en sesión, redirigir al login
            if (usuario == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
