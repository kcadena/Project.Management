using MProjectWPF.UserControl.Registro;
using System.Windows;
using System.Windows.Input;
using System;

namespace MProjectWPF.UserControl.Inicio
{

    public partial class LoginWindow : Window
    {
        //DbLitecontroller dbMP;
        
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

        private void btn_forgotpassword_MouseEnter(object sender, MouseEventArgs e)
        {
            //btn_forgotpassword.TextDecorations = TextDecorations.Baseline;
        }

        private void btn_forgotpassword_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
               //           
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
                    
            
        }
    }
}
