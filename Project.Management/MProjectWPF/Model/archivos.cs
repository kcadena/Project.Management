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
    
    public partial class archivos
    {
        public long id_archivo { get; set; }
        public string nombre { get; set; }
        public string contenido { get; set; }
        public System.DateTime fecha_carga { get; set; }
        public long id_tipo_archivo { get; set; }
        public long id_caracteristica { get; set; }
        public Nullable<long> publicacion { get; set; }
        public Nullable<long> id_usuario { get; set; }
    
        public virtual usuarios usuarios { get; set; }
        public virtual caracteristicas caracteristicas { get; set; }
        public virtual tipos_archivos tipos_archivos { get; set; }
    }
}