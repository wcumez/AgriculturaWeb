namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Empleado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Empleado()
        {
            this.AsignacionTareas = new HashSet<AsignacionTarea>();
            this.Contratoes = new HashSet<Contrato>();
            this.Jornadas = new HashSet<Jornada>();
            this.Rols = new HashSet<Rol>();
        }

        public int IdEmpleado { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string DPI { get; set; }
        public string Telefono { get; set; }
        public Nullable<System.DateTime> FechaIngreso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AsignacionTarea> AsignacionTareas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contrato> Contratoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Jornada> Jornadas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rol> Rols { get; set; }
    }
}
