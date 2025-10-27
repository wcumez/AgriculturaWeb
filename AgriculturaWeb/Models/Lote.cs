namespace AgriculturaWeb.Models
{
    public class Lote
    {
        public int IdLote { get; set; }
        public int IdFinca { get; set; }
        public string Codigo { get; set; }
        public decimal AreaHa { get; set; }
        public string Nombre { get; set; }  // ✅ columna correcta
    }
}
