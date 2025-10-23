using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using AgriculturaWeb.Models;
using AgriculturaWeb.Filters;

namespace AgriculturaWeb.Controllers
{
    [AuthorizeRole("Administrador")]
    public class UsuariosController : Controller
    {
        private string connectionString = "Data Source=.;Initial Catalog=AgriculturaDB;Integrated Security=True";

        // 🔹 Listar usuarios
        public ActionResult Index()
        {
            using (var db = new SqlConnection(connectionString))
            {
                var usuarios = db.Query<Usuario, Rol, Usuario>(
                    @"SELECT 
                        u.IdUsuario, 
                        u.Nombre, 
                        u.Usuario AS NombreUsuario,               -- ✅ ya no usamos alias NombreUsuario
                        u.Contrasena, 
                        r.IdRol, 
                        r.Nombre AS Nombre   -- ✅ alias distinto para evitar conflicto con u.Nombre
                      FROM Usuario u
                      INNER JOIN Rol r ON u.IdRol = r.IdRol",
                    (usuario, rol) => { usuario.Rol = rol; return usuario; },
                    splitOn: "IdRol"
                ).ToList();

                return View(usuarios);
            }
        }

        // 🔹 Crear usuario (GET)
        [HttpGet]
        public ActionResult Create()
        {
            using (var db = new SqlConnection(connectionString))
            {
                ViewBag.Roles = db.Query<Rol>("SELECT * FROM Rol").ToList();
            }
            return View();
        }

        // 🔹 Crear usuario (POST) — incluye validación de duplicado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            using (var db = new SqlConnection(connectionString))
            {
                // Verificar duplicado
                var existe = db.QueryFirstOrDefault<Usuario>(
                    "SELECT * FROM Usuario WHERE Usuario = @Usuario",
                    new { Usuario = usuario.NombreUsuario });

                if (existe != null)
                {
                    TempData["Mensaje"] = "⚠️ Ya existe un usuario con ese nombre.";
                    ViewBag.Roles = db.Query<Rol>("SELECT * FROM Rol").ToList();
                    return View(usuario);
                }

                // Si no existe, insertar
                string sql = @"INSERT INTO Usuario (Nombre, Usuario, Contrasena, IdRol) 
                               VALUES (@Nombre, @NombreUsuario, @Contrasena, @IdRol)";
                db.Execute(sql, usuario);
            }

            TempData["Mensaje"] = "✅ Usuario creado correctamente.";
            return RedirectToAction("Index");
        }

        // 🔹 Editar usuario (GET)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new SqlConnection(connectionString))
            {
                var usuario = db.QueryFirstOrDefault<Usuario>(
                    @"SELECT 
                 IdUsuario,
                 Nombre,
                 Usuario AS NombreUsuario,  -- ✅ mapeo desde la columna real a la propiedad del modelo
                 Contrasena,
                 IdRol
              FROM Usuario
              WHERE IdUsuario = @id",
                    new { id });

                ViewBag.Roles = db.Query<Rol>("SELECT IdRol, Nombre FROM Rol").ToList();
                return View(usuario);
            }
        }

        // 🔹 Editar usuario (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                using (var db = new SqlConnection(connectionString))
                {
                    ViewBag.Roles = db.Query<Rol>("SELECT IdRol, Nombre FROM Rol").ToList();
                }
                return View(usuario);
            }

            using (var db = new SqlConnection(connectionString))
            {
                // ✅ Verificar duplicado excluyendo el mismo Id
                var existe = db.ExecuteScalar<int>(
                    @"SELECT COUNT(1)
              FROM Usuario
              WHERE Usuario = @Usuario AND IdUsuario <> @IdUsuario",
                    new { Usuario = usuario.NombreUsuario, IdUsuario = usuario.IdUsuario });

                if (existe > 0)
                {
                    TempData["Mensaje"] = "⚠️ Ya existe otro usuario con ese nombre.";
                    ViewBag.Roles = db.Query<Rol>("SELECT IdRol, Nombre FROM Rol").ToList();
                    return View(usuario);
                }

                // ✅ Actualizar (columna SQL 'Usuario' toma el valor de la propiedad 'NombreUsuario')
                db.Execute(
                    @"UPDATE Usuario
              SET Nombre = @Nombre,
                  Usuario = @NombreUsuario,
                  Contrasena = @Contrasena,
                  IdRol = @IdRol
              WHERE IdUsuario = @IdUsuario",
                    usuario);
            }

            TempData["Mensaje"] = "✅ Usuario actualizado correctamente.";
            return RedirectToAction("Index");
        }



        // 🔹 Eliminar usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (var db = new SqlConnection(connectionString))
            {
                db.Execute("DELETE FROM Usuario WHERE IdUsuario = @id", new { id });
            }

            TempData["Mensaje"] = "⚠️ Usuario eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}
