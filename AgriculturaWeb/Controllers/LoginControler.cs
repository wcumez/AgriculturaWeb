using System.Linq;
using System.Web.Mvc;
using Dapper;
using AgriculturaWeb.Models;

namespace AgriculturaWeb.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]


        public ActionResult Index(string usuario, string contrasena)
        {
            using (var cn = Db.Conn())
            {
                // ✅ Consulta SQL corregida
                var sql = @"
                    SELECT 
                        u.IdUsuario,
                        u.Nombre,
                        u.Usuario AS NombreUsuario,   -- ✅ CORREGIDO
                        u.Contrasena,
                        u.IdRol,
                        r.IdRol,
                        r.Nombre
                    FROM Usuario u
                    INNER JOIN Rol r ON u.IdRol = r.IdRol
                    WHERE u.Usuario = @usuario AND u.Contrasena = @contrasena";

                var user = cn.Query<Usuario, Rol, Usuario>(
                    sql,
                    (u, r) => { u.Rol = r; return u; },
                    new { usuario, contrasena },
                    splitOn: "IdRol"
                ).FirstOrDefault();

                if (user != null)
                {
                    Session["Usuario"] = user.NombreUsuario;
                    Session["Nombre"] = user.Nombre;
                    Session["Rol"] = user.Rol.Nombre;

                    TempData["Bienvenida"] = $"Bienvenido, {user.NombreUsuario} ({user.Rol.Nombre}) 🌿";
                    return RedirectToAction("Index", "Home");


                    
                }
                else
                {
                    ViewBag.Error = "❌ Usuario o contraseña incorrectos.";
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
