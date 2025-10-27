using System;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Collections.Generic;
using AgriculturaWeb.Models;
using AgriculturaWeb.Filters; // ✅ para los filtros personalizados

namespace AgriculturaWeb.Controllers
{
    [SesionActiva] // 🔒 Verifica que el usuario haya iniciado sesión
    [AuthorizeRole("Administrador", "Empleado")] // 👥 Permite acceso a ambos roles
    public class FincasController : Controller
    {
        public ActionResult Index()
        {
            using (var cn = Db.Conn())
            {
                var fincas = cn.Query<Finca>(
                    "dbo.sp_Finca_Listar",
                    commandType: System.Data.CommandType.StoredProcedure
                ).ToList();

                return View(fincas);
            }
        }

        public ActionResult Create()
        {
            // Solo el administrador puede crear nuevas fincas
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Finca vm)
        {
            if (!ModelState.IsValid) return View(vm);

            try
            {
                using (var cn = Db.Conn())
                {
                    var id = cn.ExecuteScalar<int>(
                        "dbo.sp_Finca_Insertar",
                        new { vm.Nombre, vm.AreaHa, vm.Ubicacion },
                        commandType: System.Data.CommandType.StoredProcedure
                    );

                    TempData["Mensaje"] = "✅ Finca creada correctamente (ID: " + id + ")";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "❌ Error: " + ex.Message;
                return View(vm);
            }
        }

        public ActionResult Edit(int id)
        {
            using (var cn = Db.Conn())
            {
                var finca = cn.QuerySingleOrDefault<Finca>(
                    "dbo.sp_Finca_Obtener",
                    new { IdFinca = id },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (finca == null)
                    return HttpNotFound();

                return View(finca);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Finca vm)
        {
            if (!ModelState.IsValid) return View(vm);

            using (var cn = Db.Conn())
            {
                cn.Execute(
                    "dbo.sp_Finca_Actualizar",
                    new
                    {
                        vm.IdFinca,
                        vm.Nombre,
                        vm.AreaHa,
                        vm.Ubicacion
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );
            }

            TempData["Mensaje"] = "✅ Finca actualizada correctamente";
            return RedirectToAction("Index");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("Index");

            try
            {
                using (var cn = Db.Conn())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@IdFinca", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    parametros.Add("@Mensaje", dbType: System.Data.DbType.String, size: 200, direction: System.Data.ParameterDirection.Output);

                    cn.Execute("dbo.sp_Finca_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);

                    string mensaje = parametros.Get<string>("@Mensaje");

                    // Guardamos el mensaje devuelto para mostrar en SweetAlert
                    TempData["Mensaje"] = string.IsNullOrEmpty(mensaje)
                        ? "❌ No se pudo procesar la solicitud."
                        : mensaje;
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "❌ Error inesperado: " + ex.Message;
            }

            return RedirectToAction("Index");
        }






    }
}
