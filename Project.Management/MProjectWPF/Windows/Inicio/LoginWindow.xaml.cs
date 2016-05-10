using MProjectWPF.UserControl.Registro;
using System.Windows;
using System.Windows.Input;

namespace MProjectWPF.UserControl.Inicio
{

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void btn_register_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RegisterWindow  reg = new RegisterWindow();
            this.Close();
            reg.Visibility = Visibility.Visible;
        }
    }
}
