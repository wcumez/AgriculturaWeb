namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string NombreUsuario { get; set; }   // ✅ corregido
        public string Contrasena { get; set; }
        public int IdRol { get; set; }        // ✅ FK hacia Rol
        public virtual Rol Rol { get; set; }  // ✅ relación con Rol
    }
}
