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
    
    public partial class actividades
    {
        public long id_actividad { get; set; }
        public long idx_caracteristica { get; set; }
        public string keym { get; set; }
        public long id_usuario { get; set; }
        public string id_caracteristica { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public long pos { get; set; }
        public long folder { get; set; }
        public System.DateTime fecha_ultima_modificacion { get; set; }
    
        public virtual usuarios_meta_datos usuarios_meta_datos { get; set; }
        public virtual caracteristicas caracteristicas { get; set; }
    }
}
