using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Data;
using AgriculturaWeb.Models;

namespace AgriculturaWeb.Controllers
{
    public class CultivosController : Controller
    {
        // LISTAR
        public ActionResult Index()
        {
            using (var cn = Db.Conn())
            {
                var lista = cn.Query<Cultivo>("sp_Cultivo_Listar", commandType: CommandType.StoredProcedure).ToList();
                return View(lista);
            }
        }

        // CREAR (GET)
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // CREAR (POST)
        [HttpPost]
        public ActionResult Create(Cultivo cultivo)
        {
            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Cultivo_Insertar",
                    new { IdCultivo = cultivo.IdCultivo, Nombre = cultivo.Nombre },
                    commandType: CommandType.StoredProcedure);
            }
            TempData["Mensaje"] = "Cultivo agregado correctamente";
            return RedirectToAction("Index");
        }

        // EDITAR (GET)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var cn = Db.Conn())
            {
                var cultivo = cn.QueryFirstOrDefault<Cultivo>(
                    "sp_Cultivo_Buscar", new { IdCultivo = id }, commandType: CommandType.StoredProcedure);
                return View(cultivo);
            }
        }

        // EDITAR (POST)
        [HttpPost]
        public ActionResult Edit(Cultivo cultivo)
        {
            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Cultivo_Editar",
                    new { IdCultivo = cultivo.IdCultivo, Nombre = cultivo.Nombre },
                    commandType: CommandType.StoredProcedure);
            }

            TempData["Mensaje"] = "Cultivo actualizado correctamente";
            return RedirectToAction("Index");
        }


        // ELIMINAR
        public ActionResult Delete(int id)
        {
            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Cultivo_Eliminar", new { IdCultivo = id }, commandType: CommandType.StoredProcedure);
            }
            TempData["Mensaje"] = "Cultivo eliminado correctamente";
            return RedirectToAction("Index");
        }
    }
}
