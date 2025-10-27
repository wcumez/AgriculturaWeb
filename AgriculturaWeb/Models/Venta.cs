namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Venta
    {
        public Venta()
        {
            this.DetalleVentas = new HashSet<DetalleVenta>();
            this.Pagoes = new HashSet<Pago>();
        }

        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetalleVenta> DetalleVentas { get; set; }
        public virtual ICollection<Pago> Pagoes { get; set; }

        // 👇 Propiedades extra para mostrar detalle de producto
        public string NombreProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Cantidad { get; set; }
    }
}
