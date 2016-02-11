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
using MProjectWPF.Services;


namespace MProjectWPF.UserControl.Registro
{
    
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
            try
            {
                usuario usu = new usuario();
                usu.e_mail = txt_email.Text;
                usu.nombre = txt_name.Text;
                usu.apellido = txt_lastName.Text;
                usu.pass = txt_Password.Text;
                ServicesClient semail = new ServicesClient();
                semail.sendEmail(txt_email.Text, "Registro a la Plataforma MProject", "Usted se ha registrado exitosamente en la plataforma MProject ");
                dbMP.usuarios.Add(usu);
                dbMP.SaveChanges();

                Application.Current.MainWindow.Visibility = Visibility.Visible;
                this.Close();
            }
            catch(Exception err) {
                MessageBox.Show(err.InnerException.ToString());
            }
            

            
        }
    }
}
