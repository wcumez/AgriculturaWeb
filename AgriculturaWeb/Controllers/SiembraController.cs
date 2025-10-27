using System;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using AgriculturaWeb.Models;
using System.Collections.Generic;

namespace AgriculturaWeb.Controllers
{
    public class SiembrasController : Controller
    {
        // ======================================
        // LISTADO
        // ======================================
        public ActionResult Index()
        {
            using (var cn = Db.Conn())
            {
                var sql = @"
                    SELECT 
                        s.IdSiembra,
                        s.FechaSiembra,
                        s.DensidadPlantas,
                        l.Nombre AS NombreLote,
                        v.NombreVariedad,
                        t.NombreTemporada
                    FROM Siembra s
                    INNER JOIN Lote l ON s.IdLote = l.IdLote
                    INNER JOIN Variedad v ON s.IdVariedad = v.IdVariedad
                    INNER JOIN Temporada t ON s.IdTemporada = t.IdTemporada";

                var lista = cn.Query<Siembra>(sql).ToList();
                return View(lista);
            }
        }

        // ======================================
        // CREAR
        // ======================================
        [HttpGet]
        public ActionResult Create()
        {
            using (var cn = Db.Conn())
            {
                ViewBag.Lotes = cn.Query<Lote>("SELECT IdLote, Nombre FROM Lote").ToList();
                ViewBag.Variedades = cn.Query<Variedad>("SELECT IdVariedad, NombreVariedad AS Nombre FROM Variedad").ToList();
                ViewBag.Temporadas = cn.Query<Temporada>("SELECT IdTemporada, NombreTemporada AS Nombre FROM Temporada").ToList();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Siembra model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var cn = Db.Conn())
            {
                string sql = @"
                    INSERT INTO Siembra (IdLote, IdVariedad, IdTemporada, FechaSiembra, DensidadPlantas)
                    VALUES (@IdLote, @IdVariedad, @IdTemporada, @FechaSiembra, @DensidadPlantas)";
                cn.Execute(sql, model);
            }

            TempData["Mensaje"] = "✅ Siembra registrada correctamente.";
            return RedirectToAction("Index");
        }

        // ======================================
        // EDITAR
        // ======================================

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var cn = Db.Conn())
            {
                // 🔹 Obtener la siembra actual
                var siembra = cn.QueryFirstOrDefault<Siembra>(
                    "SELECT * FROM Siembra WHERE IdSiembra = @IdSiembra",
                    new { IdSiembra = id });

                if (siembra == null)
                    return HttpNotFound();

                // 🔹 Cargar combos
                ViewBag.Lotes = cn.Query<Lote>("SELECT IdLote, Nombre FROM Lote").ToList();
                ViewBag.Variedades = cn.Query<Variedad>("SELECT IdVariedad, NombreVariedad AS Nombre FROM Variedad").ToList();
                ViewBag.Temporadas = cn.Query<Temporada>("SELECT IdTemporada, NombreTemporada AS Nombre FROM Temporada").ToList();

                return View(siembra);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Siembra model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var cn = Db.Conn())
            {
                string sql = @"
            UPDATE Siembra 
            SET IdLote = @IdLote,
                IdVariedad = @IdVariedad,
                IdTemporada = @IdTemporada,
                FechaSiembra = @FechaSiembra,
                DensidadPlantas = @DensidadPlantas
            WHERE IdSiembra = @IdSiembra";

                cn.Execute(sql, model);
            }

            TempData["Mensaje"] = "✅ Siembra actualizada correctamente.";
            return RedirectToAction("Index");
        }


        // ======================================
        // ELIMINAR
        // ======================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            // Solo los administradores pueden eliminar
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
                return RedirectToAction("Index");

            try
            {
                using (var cn = Db.Conn())
                {
                    var parametros = new DynamicParameters();
                    parametros.Add("@IdSiembra", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                    parametros.Add("@Mensaje", dbType: System.Data.DbType.String, size: 200, direction: System.Data.ParameterDirection.Output);

                    cn.Execute("dbo.sp_Siembra_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);

                    string mensaje = parametros.Get<string>("@Mensaje");
                    TempData["Mensaje"] = mensaje ?? "❌ No se pudo eliminar la siembra.";
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
