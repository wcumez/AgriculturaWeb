using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using AgriculturaWeb.Models;

namespace AgriculturaWeb.Controllers
{
    public class GastosController : Controller
    {
        // ✅ LISTAR
        public ActionResult Index()
        {
            using (var cn = Db.Conn())
            {
                var gastos = cn.Query<Gasto>("sp_Gasto_Listar", commandType: System.Data.CommandType.StoredProcedure).ToList();
                return View(gastos);
            }
        }

        // ✅ CREAR (GET)
        [HttpGet]
        public ActionResult Create()
        {
            using (var cn = Db.Conn())
            {
                ViewBag.Categorias = cn.Query("SELECT IdCategoriaGasto, Nombre FROM CategoriaGasto").ToList();
                ViewBag.Fincas = cn.Query("SELECT IdFinca, Nombre FROM Finca").ToList();
            }
            return View();
        }

        // ✅ CREAR (POST)
        [HttpPost]
        public ActionResult Create(Gasto gasto)
        {
            if (!ModelState.IsValid)
                return View(gasto);

            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Gasto_Insertar",
                    new
                    {
                        gasto.IdCategoriaGasto,
                        gasto.IdFinca,
                        gasto.IdSiembra,
                        gasto.Fecha,
                        gasto.Monto,
                        gasto.Descripcion
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            TempData["Mensaje"] = "✅ Gasto registrado correctamente.";
            return RedirectToAction("Index");
        }

        // ✅ EDITAR (GET)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var cn = Db.Conn())
            {
                var gasto = cn.QueryFirstOrDefault<Gasto>("SELECT * FROM Gasto WHERE IdGasto = @Id", new { Id = id });
                ViewBag.Categorias = cn.Query("SELECT IdCategoriaGasto, Nombre FROM CategoriaGasto").ToList();
                ViewBag.Fincas = cn.Query("SELECT IdFinca, Nombre FROM Finca").ToList();
                return View(gasto);
            }
        }

        // ✅ EDITAR (POST)
        [HttpPost]
        public ActionResult Edit(Gasto gasto)
        {
            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Gasto_Editar",
                    new
                    {
                        gasto.IdGasto,
                        gasto.IdCategoriaGasto,
                        gasto.IdFinca,
                        gasto.IdSiembra,
                        gasto.Fecha,
                        gasto.Monto,
                        gasto.Descripcion
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            TempData["Mensaje"] = "✅ Gasto actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // ✅ ELIMINAR
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Gasto_Eliminar", new { IdGasto = id }, commandType: System.Data.CommandType.StoredProcedure);
            }

            TempData["Mensaje"] = "🗑️ Gasto eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}
