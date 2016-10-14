using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlDB.Model;
using System.Windows.Controls;
using MProjectWPF.UsersControls;
using System.Windows;
using System.IO;
using System.Data.Entity.Validation;

namespace MProjectWPF.Controller
{
    public class Usuarios
    {
        MProjectDeskSQLITEEntities dbMP;

        public Usuarios(MProjectDeskSQLITEEntities db)
        {
            dbMP = db;
        }

        public void addUser(Dictionary<string, string> u)
        {
            usuarios_meta_datos usu = new usuarios_meta_datos();
            usu.e_mail = u["e_mail"];
            usu.id_usuario = Convert.ToInt32(u["id_usuario"]);
            usu.nombre = u["nombre"];
            usu.apellido = u["apellido"];
            usu.genero = u["genero"];
            usu.cargo = u["cargo"];
            usu.telefono = u["telefono"];
            usu.entidad = u["entidad"];
            usu.imagen = u["imagen"];

            //MessageBox.Show(usu.id_usuario + " logid2");

            dbMP.usuarios_meta_datos.Add(usu);
            saveChanges();
        }

        public void removeUser(int id)
        {
            usuarios_meta_datos usu = (from u in dbMP.usuarios_meta_datos
                            where u.id_usuario == id
                            select u).First();

            dbMP.usuarios_meta_datos.Remove(usu);
            saveChanges();
        }

        public usuarios_meta_datos getUser(string email, string pass)
        {
            try
            {
                var datos = (from x in dbMP.usuarios
                             where x.usuarios_meta_datos.e_mail == email && x.pass == pass
                             select x).Single();
                return datos.usuarios_meta_datos;

            }
            catch(Exception err)
            {
                try {
                    MessageBox.Show(err.InnerException.Message);
                }
                catch { }
            }
            

            return null;
        }

        public List<usuarios_meta_datos> listUsers()
        {
            return (from x in dbMP.usuarios_meta_datos select x).ToList();
        }

        private bool saveChanges()
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
    }
}
