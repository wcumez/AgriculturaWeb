using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Data;
using AgriculturaWeb.Models;

namespace AgriculturaWeb.Controllers
{
    public class VentasController : Controller
    {
        // LISTAR
        public ActionResult Index()
        {
            using (var cn = Db.Conn())
            {
                var lista = cn.Query<Venta>("sp_Venta_Listar", commandType: CommandType.StoredProcedure).ToList();
                return View(lista);
            }
        }

        // CREAR (GET)
        [HttpGet]
        public ActionResult Create()
        {
            using (var cn = Db.Conn())
            {
                // Para llenar el combo de clientes
                ViewBag.Clientes = cn.Query<Cliente>("sp_Cliente_Listar", commandType: CommandType.StoredProcedure).ToList();
            }
            return View();
        }

        // CREAR (POST)
        [HttpPost]
        public ActionResult Create(Venta venta)
        {
            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Venta_Insertar",
                    new { venta.IdCliente, venta.Fecha, venta.Total },
                    commandType: CommandType.StoredProcedure);
            }

            TempData["Mensaje"] = "Venta registrada correctamente";
            return RedirectToAction("Index");
        }

        // EDITAR (GET)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var cn = Db.Conn())
            {
                var venta = cn.QueryFirstOrDefault<Venta>(
                    "sp_Venta_Buscar", new { IdVenta = id }, commandType: CommandType.StoredProcedure);

                ViewBag.Clientes = cn.Query<Cliente>("sp_Cliente_Listar", commandType: CommandType.StoredProcedure).ToList();

                return View(venta);
            }
        }

        // EDITAR (POST)
        [HttpPost]
        public ActionResult Edit(Venta venta)
        {
            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Venta_Editar",
                    new { venta.IdVenta, venta.IdCliente, venta.Fecha, venta.Total },
                    commandType: CommandType.StoredProcedure);
            }

            TempData["Mensaje"] = "Venta actualizada correctamente";
            return RedirectToAction("Index");
        }

        // ELIMINAR
        public ActionResult Delete(int id)
        {
            using (var cn = Db.Conn())
            {
                cn.Execute("sp_Venta_Eliminar", new { IdVenta = id }, commandType: CommandType.StoredProcedure);
            }

            TempData["Mensaje"] = "Venta eliminada correctamente";
            return RedirectToAction("Index");
        }
    }
}
