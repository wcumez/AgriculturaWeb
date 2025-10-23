namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Venta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Venta()
        {
            this.DetalleVentas = new HashSet<DetalleVenta>();
            this.Pagoes = new HashSet<Pago>();
        }

        public int IdVenta { get; set; }
        public int IdCliente { get; set; }

        public string NombreCliente { get; set; }
        public System.DateTime Fecha { get; set; }
        public decimal Total { get; set; }



        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleVenta> DetalleVentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pago> Pagoes { get; set; }
    }
}
