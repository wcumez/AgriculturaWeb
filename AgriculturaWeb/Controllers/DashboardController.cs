using System.Linq;
using System.Web.Mvc;
using Dapper;
using AgriculturaWeb.Models;

namespace AgriculturaWeb.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            using (var cn = Db.Conn())
            {
                // Totales
                var totales = cn.QueryFirstOrDefault<dynamic>(
                    "sp_Dashboard_Totales",
                    commandType: System.Data.CommandType.StoredProcedure);

                ViewBag.TotalFincas = totales.TotalFincas;
                ViewBag.TotalEmpleados = totales.TotalEmpleados;
                ViewBag.TotalProductos = totales.TotalProductos;
                ViewBag.TotalGastos = totales.TotalGastos;

                // Gráficos
                ViewBag.GastosMes = cn.Query("sp_Dashboard_GastosPorMes",
                    commandType: System.Data.CommandType.StoredProcedure).ToList();

                ViewBag.Cultivos = cn.Query("sp_Dashboard_DistribucionCultivos",
                    commandType: System.Data.CommandType.StoredProcedure).ToList();

                ViewBag.Stock = cn.Query("sp_Dashboard_StockProductos",
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
            }

            return View();
        }
    }
}
