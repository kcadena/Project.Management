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
    
    public partial class caracteristicas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public caracteristicas()
        {
            this.archivos = new HashSet<archivos>();
            this.caracteristicas1 = new HashSet<caracteristicas>();
        }
    
        public long id_caracteristica { get; set; }
        public string estado { get; set; }
        public Nullable<long> porcentaje_asignado { get; set; }
        public Nullable<long> porcentaje_cumplimido { get; set; }
        public Nullable<long> duracion { get; set; }
        public string tipo_duracion { get; set; }
        public Nullable<System.DateTime> fecha_inicio { get; set; }
        public long id_actividad { get; set; }
        public long id_proyecto { get; set; }
        public Nullable<long> padre_caracteristica { get; set; }
        public Nullable<long> proyecto_padre { get; set; }
    
        public virtual actividades actividades { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<archivos> archivos { get; set; }
        public virtual proyectos proyectos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<caracteristicas> caracteristicas1 { get; set; }
        public virtual caracteristicas caracteristicas2 { get; set; }
        public virtual proyectos proyectos1 { get; set; }
    }
}