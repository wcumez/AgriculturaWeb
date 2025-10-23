using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using AgriculturaWeb.Models;

namespace AgriculturaWeb.Controllers
{
    public class InventarioController : Controller
    {
        // ✅ LISTAR MOVIMIENTOS
        public ActionResult Index()
        {
            using (var cn = Db.Conn())
            {
                var lista = cn.Query<MovimientoInventario>(
                    "sp_MovimientoInventario_Listar",
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
                return View(lista);
            }
        }

        // ✅ CREAR (GET)
        [HttpGet]
        public ActionResult Create()
        {
            using (var cn = Db.Conn())
            {
                ViewBag.Almacenes = cn.Query("SELECT IdAlmacen, Nombre FROM Almacen").ToList();
                ViewBag.Productos = cn.Query("SELECT IdProducto, Nombre FROM Producto").ToList();
            }
            return View();
        }

        // ✅ CREAR (POST)
        [HttpPost]
        public ActionResult Create(MovimientoInventario mov)
        {
            if (!ModelState.IsValid)
                return View(mov);

            using (var cn = Db.Conn())
            {
                cn.Execute("sp_MovimientoInventario_Insertar",
                    new
                    {
                        mov.IdAlmacen,
                        mov.IdProducto,
                        Fecha = DateTime.Now,
                        TipoMovimiento = mov.Tipo.Substring(0, 1).ToUpper(), // "E" o "S"
                        mov.Cantidad,
                        mov.Referencia
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            TempData["Mensaje"] = "✅ Movimiento registrado correctamente.";
            return RedirectToAction("Index");
        }

        // ✅ VER STOCK ACTUAL
        public ActionResult Stock()
        {
            using (var cn = Db.Conn())
            {
                var stock = cn.Query<MovimientoInventario>(
                    "sp_Inventario_StockActual",
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
                return View(stock);
            }
        }

    }
}
