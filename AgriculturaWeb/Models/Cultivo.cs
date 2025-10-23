namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Cultivo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cultivo()
        {
            this.Variedads = new HashSet<Variedad>();
        }

        public int IdCultivo { get; set; }
        public string Nombre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Variedad> Variedads { get; set; }
    }
}
