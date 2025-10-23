using System;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Collections.Generic;
using Newtonsoft.Json;


using AgriculturaWeb.Models; // o el namespace donde tienes Db.Conn()

namespace AgriculturaWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var cn = Db.Conn()) // Usa tu método de conexión (puede ser DataContext.GetConnection())
            {
                // ==== Totales ====
                var totales = cn.QueryFirstOrDefault<dynamic>(
                    "sp_Dashboard_Totales",
                    commandType: System.Data.CommandType.StoredProcedure
                );

                // Verifica que existan columnas en el SP: TotalFincas, TotalEmpleados, TotalProductos, TotalGastos
                if (totales != null)
                {
                    ViewBag.TotalFincas = totales.TotalFincas;
                    ViewBag.TotalEmpleados = totales.TotalEmpleados;
                    ViewBag.TotalProductos = totales.TotalProductos;
                    ViewBag.TotalGastos = totales.TotalGastos;
                }
                else
                {
                    ViewBag.TotalFincas = 0;
                    ViewBag.TotalEmpleados = 0;
                    ViewBag.TotalProductos = 0;
                    ViewBag.TotalGastos = 0;
                }

                // ==== Gráfico de Gastos Mensuales ====
                var gastosMes = cn.Query(
                    "sp_Dashboard_GastosPorMes",
                    commandType: System.Data.CommandType.StoredProcedure
                ).ToList();
                ViewBag.GastosMes = gastosMes ?? new List<dynamic>();

                // ==== Distribución de Cultivos ====
                var cultivos = cn.Query(
                    "sp_Dashboard_DistribucionCultivos",
                    commandType: System.Data.CommandType.StoredProcedure
                ).ToList();
                ViewBag.Cultivos = cultivos ?? new List<dynamic>();

                // ==== Stock Actual ====
                var stock = cn.Query(
                    "sp_Dashboard_StockProductos",
                    commandType: System.Data.CommandType.StoredProcedure
                ).ToList();
                ViewBag.Stock = stock ?? new List<dynamic>();
            }

            return View();
        }
    }
}
