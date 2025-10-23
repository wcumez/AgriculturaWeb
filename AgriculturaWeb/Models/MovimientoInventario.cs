using System;

namespace AgriculturaWeb.Models
{
    public class MovimientoInventario
    {
        public int IdMovimiento { get; set; }
        public int IdAlmacen { get; set; }
        public string Almacen { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public string Tipo { get; set; }       // Entrada o Salida
        public decimal Cantidad { get; set; }



        public DateTime Fecha { get; set; }
        public string Referencia { get; set; }

        public decimal StockActual { get; set; }
    }
}

