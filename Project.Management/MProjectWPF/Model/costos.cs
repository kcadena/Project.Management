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
    
    public partial class costos
    {
        public long idx_costo { get; set; }
        public long idx_caracteristica { get; set; }
        public long id_costo { get; set; }
        public long id_caracteristica { get; set; }
        public string nombre { get; set; }
        public long cantidad { get; set; }
        public long valor { get; set; }
        public long id_usuario { get; set; }
        public string keym { get; set; }
    
        public virtual caracteristicas caracteristicas { get; set; }
        public virtual usuarios_meta_datos usuarios_meta_datos { get; set; }
    }
}
