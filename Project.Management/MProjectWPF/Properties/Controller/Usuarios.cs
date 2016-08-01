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

namespace MProjectWPF.Controller
{
    public class Usuarios
    {
        MProjectDeskSQLITEEntities dbMP;

        public Usuarios(MProjectDeskSQLITEEntities db)
        {
            dbMP = db;
        }

        public string agregarUsuario(string email, string name, string lastname, string pass)
        {

            try
            {
                /*usu.e_mail = email;
                usu.nombre = name;
                usu.apellido = lastname;
                usu.pass = pass;                
                dbMP.usuarios.Add(usu);
                dbMP.SaveChanges();*/
                return "ok";
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public usuarios_meta_datos getUser(string email, string pass)
        {
            var datos = (from x in dbMP.usuarios
                        where x.usuarios_meta_datos.e_mail == email && x.pass == pass
                        select x).Single();

            return datos.usuarios_meta_datos;
        }

        public List<usuarios_meta_datos> listUsers()
        {
            return (from x in dbMP.usuarios_meta_datos select x).ToList();
        }
    }
}
