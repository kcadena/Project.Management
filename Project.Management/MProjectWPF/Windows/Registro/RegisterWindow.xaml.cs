using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MProjectWPF.UserControl.Inicio;
using MProjectWPF.Model;
using MProjectWPF.Windows.Inicio;
//using MProjectWPF.Model;
using MProjectWPF.Services;
using MProjectWPF.Controller;

namespace MProjectWPF.UserControl.Registro
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btn_cancel_register_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            LoginWindow log = new LoginWindow();
            log.Visibility = Visibility.Visible;
        }

        private void btn_reg_Click(object sender, RoutedEventArgs e)
        {
            MProjectDeskSQLITEEntities dbMP = new MProjectDeskSQLITEEntities();
           
            usuario usu = new usuario();
            usu.id_usuario = Convert.ToInt32(txt_ide.Text);
            usu.e_mail = txt_email.Text;
            usu.nombre = txt_name.Text;
            usu.apellido = txt_lastName.Text;
            usu.pass = txt_Password.Text;
            try
            {
                dbMP.usuarios.Add(usu);
                dbMP.SaveChanges();
            }
            catch(Exception err) {
                MessageBox.Show(err.InnerException.ToString());
        }

            
    }
}
}
