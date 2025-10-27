using System;

namespace AgriculturaWeb.Models
{
    public class Siembra
    {
        public int IdSiembra { get; set; }
        public int IdLote { get; set; }
        public int IdVariedad { get; set; }
        public int IdTemporada { get; set; }
        public DateTime FechaSiembra { get; set; }
        public int DensidadPlantas { get; set; }

        // 🔹 Propiedades para mostrar nombres en la vista (JOIN)
        public string NombreLote { get; set; }
        public string NombreVariedad { get; set; }
        public string NombreTemporada { get; set; }
    }
}
