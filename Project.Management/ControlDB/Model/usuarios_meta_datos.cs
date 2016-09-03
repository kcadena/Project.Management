//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ControlDB.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class usuarios_meta_datos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usuarios_meta_datos()
        {
            this.actividades = new HashSet<actividades>();
            this.caracteristicas1 = new HashSet<caracteristicas>();
            this.caracteristicas = new HashSet<caracteristicas>();
            this.costos = new HashSet<costos>();
            this.meta_datos = new HashSet<meta_datos>();
            this.plantillas = new HashSet<plantillas>();
            this.plantillas_meta_datos = new HashSet<plantillas_meta_datos>();
            this.presupuesto = new HashSet<presupuesto>();
            this.proyectos = new HashSet<proyectos>();
            this.proyectos_meta_datos = new HashSet<proyectos_meta_datos>();
            this.recursos = new HashSet<recursos>();
            this.archivos = new HashSet<archivos>();
        }
    
        public long id_usuario { get; set; }
        public string e_mail { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string genero { get; set; }
        public string cargo { get; set; }
        public string telefono { get; set; }
        public string entidad { get; set; }
        public string imagen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<actividades> actividades { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<caracteristicas> caracteristicas1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<caracteristicas> caracteristicas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<costos> costos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<meta_datos> meta_datos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<plantillas> plantillas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<plantillas_meta_datos> plantillas_meta_datos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<presupuesto> presupuesto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyectos> proyectos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<proyectos_meta_datos> proyectos_meta_datos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<recursos> recursos { get; set; }
        public virtual repositorios_usuarios repositorios_usuarios { get; set; }
        public virtual usuarios usuarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<archivos> archivos { get; set; }
    }
}
