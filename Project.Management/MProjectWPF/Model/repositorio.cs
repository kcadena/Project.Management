//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MProjectWPF.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class repositorio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public repositorio()
        {
            this.proyectos = new HashSet<proyecto>();
        }
    
        public long id_repositorio { get; set; }
        public string descripcion { get; set; }
        public string ruta_proyecto { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyecto> proyectos { get; set; }
    }
}
