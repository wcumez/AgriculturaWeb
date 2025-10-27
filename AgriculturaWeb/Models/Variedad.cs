namespace AgriculturaWeb.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Variedad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Variedad()
        {
            this.Siembras = new HashSet<Siembra>();
        }

        public int IdVariedad { get; set; }
        public int IdCultivo { get; set; }
        public string Nombre { get; set; }

        public virtual Cultivo Cultivo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siembra> Siembras { get; set; }
    }
}
