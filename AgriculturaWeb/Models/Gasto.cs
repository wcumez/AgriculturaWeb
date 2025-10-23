
namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Gasto
    {
        public int IdGasto { get; set; }

        public string Categoria { get; set; }
        public int IdCategoriaGasto { get; set; }
        public Nullable<int> IdFinca { get; set; }

        public string Finca { get; set; }
        public Nullable<int> IdSiembra { get; set; }
        public System.DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }


    }
}
