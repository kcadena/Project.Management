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
    
    public partial class archivo
    {
        public long id_archivo { get; set; }
        public string nombre { get; set; }
        public string contenido { get; set; }
        public System.DateTime fecha_carga { get; set; }
        public long id_folder { get; set; }
        public long id_tipo_archivo { get; set; }
        public long id_caracteristica { get; set; }
    
        public virtual caracteristica caracteristica { get; set; }
        public virtual folder folder { get; set; }
        public virtual tipos_archivos tipos_archivos { get; set; }
    }
}