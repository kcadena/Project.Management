using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MProjectWPF.Model;

namespace MProjectWPF.UsersControls.Clases
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

        public  string buscarUsuario(string email)
        {
            return "";
        }

    }
}
