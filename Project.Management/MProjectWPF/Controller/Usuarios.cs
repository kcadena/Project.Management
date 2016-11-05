using System;
using System.Collections.Generic;
using System.Linq;
using ControlDB.Model;
using System.Windows;
using System.Data.Entity.Validation;
using System.Xml;

namespace MProjectWPF.Controller
{
    public class Usuarios
    {
        //Buscar Usuario en la base de datos Local
        public static usuarios_meta_datos getUser(string email, string pass, MProjectDeskSQLITEEntities dbMP)
        {
            try
            {
                var datos = (from x in dbMP.usuarios
                             where x.usuarios_meta_datos.e_mail == email && x.pass == pass
                             select x).Single();

                return datos.usuarios_meta_datos;
            }
            catch
            {
                return null;
            }
        }

        //Agregar Usuario de la Base de Datos Remota a Base Local
        public static usuarios_meta_datos addUser(XmlNode nodeF , MProjectDeskSQLITEEntities dbMP)
        {
            #region Agregar Usuario Meta Datos
            usuarios_meta_datos usuMod = new usuarios_meta_datos();
            usuMod.id_usuario = Convert.ToInt64(nodeF.Attributes["id_usuario"].Value);
            usuMod.e_mail = nodeF.Attributes["e_mail"].Value;
            usuMod.nombre = nodeF.Attributes["nombre"].Value;
            usuMod.apellido = nodeF.Attributes["apellido"].Value;
            usuMod.genero = nodeF.Attributes["genero"].Value;
            usuMod.cargo = nodeF.Attributes["cargo"].Value;
            usuMod.telefono = nodeF.Attributes["telefono"].Value;
            usuMod.entidad = nodeF.Attributes["entidad"].Value;

            if(nodeF.Attributes["imagen"].Value != "")
            usuMod.imagen = nodeF.Attributes["imagen"].Value;

            dbMP.usuarios_meta_datos.Add(usuMod);
            #endregion

            #region Agregar Usuario
            usuarios usuPass = new usuarios();
            usuPass.usuarios_meta_datos = usuMod;
            usuPass.pass = nodeF.Attributes["pass"].Value;
            usuPass.administrador = Convert.ToBoolean(nodeF.Attributes["administrador"].Value);

            dbMP.usuarios.Add(usuPass);
            #endregion

            #region Agregar Ruta repositorio
            repositorios_usuarios repUsu = new repositorios_usuarios();
            repUsu.usuarios_meta_datos = usuMod;
            repUsu.ruta_repositorio_local = getRepositorio(dbMP) + "user" + usuMod.id_usuario;
            repUsu.ruta_repositorio_servidor = nodeF.Attributes["ruta_repositorio"].Value;

            dbMP.repositorios_usuarios.Add(repUsu);
            #endregion

            #region Agregar table sequence
            table_sequence tabSeq = new table_sequence();
            tabSeq.usuarios_meta_datos = usuMod;
            tabSeq.actividades = Convert.ToInt64(nodeF.Attributes["actividades"].Value);
            tabSeq.archivos = Convert.ToInt64(nodeF.Attributes["archivos"].Value);
            tabSeq.caracteristicas = Convert.ToInt64(nodeF.Attributes["caracteristicas"].Value);
            tabSeq.costos = Convert.ToInt64(nodeF.Attributes["costos"].Value);
            tabSeq.proyectos = Convert.ToInt64(nodeF.Attributes["proyectos"].Value);
            tabSeq.proyectos_meta_datos = Convert.ToInt64(nodeF.Attributes["proyectos_meta_datos"].Value);
            tabSeq.recursos = Convert.ToInt64(nodeF.Attributes["recursos"].Value);
            tabSeq.presupuesto = Convert.ToInt64(nodeF.Attributes["presupuesto"].Value);

            dbMP.table_sequence.Add(tabSeq);
            #endregion
            return usuMod;
        }

        public static void updateTableSequence(XmlNode nodeF, MProjectDeskSQLITEEntities dbMP)
        {
            long id_usuario = Convert.ToInt64(nodeF.Attributes["id_usuario"].Value);

            table_sequence tabSeq = (from tabs in dbMP.table_sequence
                                     where tabs.id_usuario == id_usuario
                                     select tabs).Single();

            tabSeq.id_usuario = Convert.ToInt64(nodeF.Attributes["id_usuario"].Value);
            tabSeq.actividades = Convert.ToInt64(nodeF.Attributes["actividades"].Value);
            tabSeq.archivos = Convert.ToInt64(nodeF.Attributes["archivos"].Value);
            tabSeq.caracteristicas = Convert.ToInt64(nodeF.Attributes["caracteristicas"].Value);
            tabSeq.costos = Convert.ToInt64(nodeF.Attributes["costos"].Value);
            tabSeq.proyectos = Convert.ToInt64(nodeF.Attributes["proyectos"].Value);
            tabSeq.proyectos_meta_datos = Convert.ToInt64(nodeF.Attributes["proyectos_meta_datos"].Value);
            tabSeq.recursos = Convert.ToInt64(nodeF.Attributes["recursos"].Value);
            tabSeq.presupuesto = Convert.ToInt64(nodeF.Attributes["presupuesto"].Value);

            dbMP.SaveChanges();
        }

        public static string getRepositorio(MProjectDeskSQLITEEntities dbMP)
        {
            var repo = (from rep in dbMP.mproject_key select rep).First();
            return repo.repositorio;
        }

        private static bool saveChanges(MProjectDeskSQLITEEntities dbMP)
        {
            try
            {
                try
                {
                    dbMP.SaveChanges();
                }
                catch (Exception err)
                {
                   MessageBox.Show(err.InnerException.InnerException.Message);
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                MessageBox.Show(e.Message);

                foreach (var eve in e.EntityValidationErrors)
                {
                    MessageBox.Show(eve.Entry.Entity.GetType().Name + " " + eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        MessageBox.Show(ve.PropertyName + " " + ve.ErrorMessage);
                    }
                }
                return false;
            }
        }

        //public void removeUser(int id)
        //{
        //    usuarios_meta_datos usu = (from u in dbMP.usuarios_meta_datos
        //                    where u.id_usuario == id
        //                    select u).First();

        //    dbMP.usuarios_meta_datos.Remove(usu);
        //    saveChanges();
        //}

        

        //public List<usuarios_meta_datos> listUsers()
        //{
        //    return (from x in dbMP.usuarios_meta_datos select x).ToList();
        //}
    }
}
