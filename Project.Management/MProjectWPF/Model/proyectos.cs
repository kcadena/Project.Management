//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MProjectWPF.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class proyectos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public proyectos()
        {
            this.caracteristicas = new HashSet<caracteristicas>();
            this.caracteristicas1 = new HashSet<caracteristicas>();
            this.proyectos_meta_datos = new HashSet<proyectos_meta_datos>();
        }
    
        public long id_proyecto { get; set; }
        public long id_plantilla { get; set; }
        public long id_repositorio { get; set; }
        public bool IR_proyecto { get; set; }
        public Nullable<long> id_usuario { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<caracteristicas> caracteristicas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<caracteristicas> caracteristicas1 { get; set; }
        public virtual plantillas plantillas { get; set; }
        public virtual usuarios usuarios { get; set; }
        public virtual repositorio repositorio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyectos_meta_datos> proyectos_meta_datos { get; set; }
    }
}
