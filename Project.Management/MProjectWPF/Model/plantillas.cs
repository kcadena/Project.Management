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
    
    public partial class plantillas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public plantillas()
        {
            this.plantillas_meta_datos = new HashSet<plantillas_meta_datos>();
        }
    
        public string keym { get; set; }
        public long idx_plantilla { get; set; }
        public long id_plantilla { get; set; }
        public long id_usuario { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public System.DateTime fecha_ultima_modificacion { get; set; }
    
        public virtual usuarios_meta_datos usuarios_meta_datos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<plantillas_meta_datos> plantillas_meta_datos { get; set; }
    }
}
