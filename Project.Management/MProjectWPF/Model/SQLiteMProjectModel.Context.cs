﻿//------------------------------------------------------------------------------
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
    
        public virtual DbSet<actividade> actividades { get; set; }
        public virtual DbSet<archivo> archivos { get; set; }
        public virtual DbSet<caracteristica> caracteristicas { get; set; }
        public virtual DbSet<folder> folders { get; set; }
        public virtual DbSet<meta_datos> meta_datos { get; set; }
        public virtual DbSet<plantilla> plantillas { get; set; }
        public virtual DbSet<plantillas_meta_datos> plantillas_meta_datos { get; set; }
        public virtual DbSet<proyecto> proyectos { get; set; }
        public virtual DbSet<proyectos_meta_datos> proyectos_meta_datos { get; set; }
        public virtual DbSet<repositorio> repositorios { get; set; }
        public virtual DbSet<tipos_archivos> tipos_archivos { get; set; }
        public virtual DbSet<tipos_datos> tipos_datos { get; set; }
        public virtual DbSet<usuario> usuarios { get; set; }
    }
}
