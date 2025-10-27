using System;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using AgriculturaWeb.Models;
using System.Collections.Generic;

namespace AgriculturaWeb.Controllers
{
    public class VentasController : Controller
    {
        // ===============================
        // LISTADO DE VENTAS
        // ===============================
        public ActionResult Index()
        {
            using (var cn = Db.Conn())
            {
                cn.Open(); // ✅ Asegura que la conexión esté abierta

                string sql = @"
                    SELECT 
                        v.IdVenta, 
                        c.Nombre AS NombreCliente,
                        v.Fecha,
                        v.Total,
                        p.Nombre AS NombreProducto,
                        dv.Cantidad,
                        dv.PrecioUnitario
                    FROM Venta v
                    INNER JOIN DetalleVenta dv ON v.IdVenta = dv.IdVenta
                    INNER JOIN Producto p ON dv.IdProducto = p.IdProducto
                    INNER JOIN Cliente c ON v.IdCliente = c.IdCliente
                    ORDER BY v.IdVenta DESC";

                var lista = cn.Query<Venta>(sql).ToList();
                return View(lista);
            }
        }

        // ===============================
        // NUEVA VENTA (GET)
        // ===============================
        [HttpGet]
        public ActionResult Create()
        {
            using (var cn = Db.Conn())
            {
                cn.Open(); // ✅ Abrir conexión antes de las consultas

                ViewBag.Clientes = cn.Query<Cliente>("SELECT IdCliente, Nombre FROM Cliente").ToList();
                ViewBag.Productos = cn.Query<Producto>("SELECT IdProducto, Nombre FROM Producto").ToList();
                ViewBag.Almacenes = cn.Query<Almacen>("SELECT IdAlmacen, Nombre FROM Almacen").ToList();
            }

            return View();
        }

        // ===============================
        // NUEVA VENTA (POST)
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int IdCliente, int IdProducto, int IdAlmacen, decimal Cantidad, decimal PrecioUnitario)
        {
            using (var cn = Db.Conn())
            {
                cn.Open(); // ✅ Abrimos la conexión ANTES de iniciar la transacción
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        decimal total = Cantidad * PrecioUnitario;

                        // 1️⃣ Registrar la venta
                        int idVenta = cn.ExecuteScalar<int>(@"
                            INSERT INTO Venta (IdCliente, Fecha, Total)
                            VALUES (@IdCliente, GETDATE(), @Total);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);",
                            new { IdCliente, Total = total },
                            tran
                        );

                        // 2️⃣ Registrar el detalle de venta
                        cn.Execute(@"
                            INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario)
                            VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario);",
                            new { IdVenta = idVenta, IdProducto, Cantidad, PrecioUnitario },
                            tran
                        );

                        // 3️⃣ Registrar salida en inventario automáticamente
                        // 3️⃣ Registrar salida en inventario (columna TipoMovimiento es CHAR(1))
                        cn.Execute(@"
    INSERT INTO MovimientoInventario (IdAlmacen, IdProducto, TipoMovimiento, Cantidad, Referencia, Fecha)
    VALUES (@IdAlmacen, @IdProducto, 'S', @Cantidad, CONCAT('Venta #', @IdVenta), GETDATE());",
                            new { IdAlmacen, IdProducto, Cantidad, IdVenta = idVenta },
                            tran
                        );


                        tran.Commit();
                        TempData["Mensaje"] = "✅ Venta registrada correctamente y salida en inventario aplicada.";
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        TempData["Mensaje"] = "❌ Error al registrar la venta: " + ex.Message;
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
