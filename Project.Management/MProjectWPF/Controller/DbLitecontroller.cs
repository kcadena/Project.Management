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
        MProjectDeskSQLITEEntities dbMP = new MProjectDeskSQLITEEntities();
        usuario usu = new usuario();
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

    }
}
