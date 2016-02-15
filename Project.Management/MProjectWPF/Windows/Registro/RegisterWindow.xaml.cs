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
using MProjectWPF.Controller;
using MProjectWPF.UsersControlls.Clases;

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
            DbLitecontroller db = new DbLitecontroller();             
            string res=db.agregarUsuario(txt_email.Text, txt_name.Text, txt_lastName.Text, txt_Password.Text);
            if (res.Equals("ok"))
            {
                Application.Current.MainWindow.Visibility = Visibility.Visible;
                this.Close();
            }
            else
            {
                MessageBox.Show(res);
            }
        }
    }
}
