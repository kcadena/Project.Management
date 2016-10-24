
using System.Windows;
using System.Windows.Input;
using System;
using MProjectWPF.Controller;
using ControlDB.Model;
using System.Diagnostics;

namespace MProjectWPF.UserControl.Inicio
{

    public partial class LoginWindow : Window
    {
        MProjectDeskSQLITEEntities dbMP;
        MainWindow mainW;
        bool canClose = true;

        public LoginWindow(MainWindow mainW)
        {
            InitializeComponent();
            dbMP = mainW.dbMP;
            this.mainW = mainW;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(mainW.user_Click("kelvin.cadena@gmail.com", "123"))
            {
                canClose = false;
                mainW.Visibility = Visibility.Visible;
                Close();
            }
        }

        private void button_Click2(object sender, RoutedEventArgs e)
        {
            if (mainW.user_Click("david@gmail.com", "123"))
            {
                canClose = false;
                mainW.Visibility = Visibility.Visible;
                Close();
            }
        }

        private void button_Click3(object sender, RoutedEventArgs e)
        {
            if (mainW.user_Click("karenEst@hotmail.com", "123"))
            {
                canClose = false;
                mainW.Visibility = Visibility.Visible;
                Close();
            }
        }

        private void btn_register_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void btn_forgotpassword_MouseEnter(object sender, MouseEventArgs e)
        {
            //btn_forgotpassword.TextDecorations = TextDecorations.Baseline;
        }

        private void btn_forgotpassword_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (canClose)
            {
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
