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

        }
    }
}
