using System;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using AgriculturaWeb.Models;
using AgriculturaWeb.Filters;
using System.Collections.Generic;

namespace AgriculturaWeb.Controllers
{
    [AuthorizeRole("Administrador")]
    public class EmpleadosController : Controller
    {
        private string connectionString = "Data Source=.;Initial Catalog=AgriculturaDB;Integrated Security=True";

        // 📋 Listar empleados con búsqueda y paginación
        public ActionResult Index(string busqueda = "", int pagina = 1)
        {
            const int registrosPorPagina = 5; // 👈 cantidad de empleados por página

            using (var db = new SqlConnection(connectionString))
            {
                // 🔍 Buscar empleados por nombre o apellido
                string sql = @"
                    SELECT * FROM Empleado
                    WHERE (@busqueda = '' OR Nombres LIKE '%' + @busqueda + '%' OR Apellidos LIKE '%' + @busqueda + '%')
                    ORDER BY IdEmpleado DESC
                    OFFSET (@pagina - 1) * @registrosPorPagina ROWS
                    FETCH NEXT @registrosPorPagina ROWS ONLY;

                    SELECT COUNT(*) FROM Empleado
                    WHERE (@busqueda = '' OR Nombres LIKE '%' + @busqueda + '%' OR Apellidos LIKE '%' + @busqueda + '%');";

                using (var multi = db.QueryMultiple(sql, new { busqueda, pagina, registrosPorPagina }))
                {
                    var empleados = multi.Read<Empleado>().ToList();
                    int totalRegistros = multi.ReadFirst<int>();

                    // Enviar datos a la vista
                    ViewBag.Busqueda = busqueda;
                    ViewBag.PaginaActual = pagina;
                    ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

                    return View(empleados);
                }
            }
        }

        // ➕ Crear empleado
        [HttpGet]
        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado empleado)
        {
            try
            {
                using (var db = new SqlConnection(connectionString))
                {
                    string sql = @"INSERT INTO Empleado (Nombres, Apellidos, DPI, Telefono, FechaIngreso)
                                   VALUES (@Nombres, @Apellidos, @DPI, @Telefono, @FechaIngreso)";
                    db.Execute(sql, empleado);
                }

                TempData["Mensaje"] = "✅ Empleado registrado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "❌ Error: " + ex.Message;
                return View(empleado);
            }
        }

        // ✏️ Editar
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var empleado = db.QueryFirstOrDefault<Empleado>("SELECT * FROM Empleado WHERE IdEmpleado = @id", new { id });
                return View(empleado);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado empleado)
        {
            using (var db = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Empleado 
                               SET Nombres = @Nombres, Apellidos = @Apellidos,
                                   DPI = @DPI, Telefono = @Telefono, FechaIngreso = @FechaIngreso
                               WHERE IdEmpleado = @IdEmpleado";
                db.Execute(sql, empleado);
            }

            TempData["Mensaje"] = "✅ Empleado actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // 🗑️ Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (var db = new SqlConnection(connectionString))
            {
                db.Execute("DELETE FROM Empleado WHERE IdEmpleado = @id", new { id });
            }

            TempData["Mensaje"] = "⚠️ Empleado eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}
