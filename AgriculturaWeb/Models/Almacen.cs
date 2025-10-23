namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Almacen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Almacen()
        {
            this.MovimientoInventarios = new HashSet<MovimientoInventario>();
        }

        public int IdAlmacen { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> IdFinca { get; set; }

        public virtual Finca Finca { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MovimientoInventario> MovimientoInventarios { get; set; }
    }
}
