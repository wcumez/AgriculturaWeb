
namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Temporada
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Temporada()
        {
            this.Siembras = new HashSet<Siembra>();
        }

        public int IdTemporada { get; set; }
        public string Nombre { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siembra> Siembras { get; set; }
    }
}
