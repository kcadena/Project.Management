﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MProjectDeskSQLITEEntities : DbContext
    {
        public MProjectDeskSQLITEEntities()
            : base("name=MProjectDeskSQLITEEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<actividades> actividades { get; set; }
        public virtual DbSet<archivos> archivos { get; set; }
        public virtual DbSet<caracteristicas> caracteristicas { get; set; }
        public virtual DbSet<costos> costos { get; set; }
        public virtual DbSet<meta_datos> meta_datos { get; set; }
        public virtual DbSet<mproject_key> mproject_key { get; set; }
        public virtual DbSet<plantillas> plantillas { get; set; }
        public virtual DbSet<plantillas_meta_datos> plantillas_meta_datos { get; set; }
        public virtual DbSet<presupuesto> presupuesto { get; set; }
        public virtual DbSet<proyectos> proyectos { get; set; }
        public virtual DbSet<proyectos_meta_datos> proyectos_meta_datos { get; set; }
        public virtual DbSet<recursos> recursos { get; set; }
        public virtual DbSet<repositorios_usuarios> repositorios_usuarios { get; set; }
        public virtual DbSet<table_sequence> table_sequence { get; set; }
        public virtual DbSet<tipos_datos> tipos_datos { get; set; }
        public virtual DbSet<usuarios> usuarios { get; set; }
        public virtual DbSet<usuarios_meta_datos> usuarios_meta_datos { get; set; }
    }
}
