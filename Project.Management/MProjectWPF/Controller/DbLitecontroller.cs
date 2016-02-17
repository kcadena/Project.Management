using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MProjectWPF.Model;

namespace MProjectWPF.Controller
{
    class DbLitecontroller
    {
        MProjectDeskEntities dbMP = new MProjectDeskEntities();
        usuarios usu = new usuarios();

        public DbLitecontroller() { }
        
        public string agregarUsuario(string email,string name, string lastname, string pass)
        {            
            try
            {               
                usu.e_mail = email;
                usu.nombre = name;
                usu.apellido = lastname;
                usu.pass = pass;                
                dbMP.usuarios.Add(usu);
                dbMP.SaveChanges();
                return "ok";            
            }
            catch (Exception err)
            {
                return err.Message;                   
            }            
        }

        public  string buscarUsuario(string email,string pass)
        {
            var datos = from x in dbMP.usuarios
                        where x.e_mail == email && x.pass==pass
                        select x;

            
            if (datos != null)
            {
                string nom="";
                foreach(var y in datos)
                {
                    nom = y.nombre;
                }                
                return nom;
            }
            else
            {
                return "Usuario no existe";
            }
            
        }

        public void buscarProyecto()
        {
            var datos = from x in dbMP.usuarios_ join y in dbMP.proyectos_meta_datos on x.id_usuario equals y.id_proyecto
                        where x.e_mail == email && x.pass == pass
                        select x;
        }
    }
}
