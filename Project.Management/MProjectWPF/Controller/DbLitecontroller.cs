using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MProjectWPF.Model;
using System.Windows.Controls;
using MProjectWPF.UsersControlls;

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

        public void buscarProyecto(ListBox lb,MainWindow mw)
        {
            var datos = from x in dbMP.proyectos_meta_datos                        
                        select x;

            foreach(var y in datos)
            {
                lb.Items.Add(new LabelProjectAll(y.valor.ToUpper(), mw, true));
            }
        }

        public void buscarPlantilla(ListBox lb, MainWindow mw)
        {
            var datos = from x in dbMP.plantillas
                        select x;

            foreach (var y in datos)
            {
                lb.Items.Add(new LabelProjectAll(y.nombre, mw, false));
            }
        }
    }
}
