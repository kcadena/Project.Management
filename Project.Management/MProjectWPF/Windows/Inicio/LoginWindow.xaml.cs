﻿using MProjectWPF.Windows.Registro;
using MProjectWPF.Controller;
using System.Windows;
using System.Windows.Input;

namespace MProjectWPF.Windows.Inicio
{

    public partial class LoginWindow : Window
    {
        DbLitecontroller dbMP;
        public LoginWindow()
        {
            InitializeComponent();
            dbMP = new DbLitecontroller();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string res=dbMP.buscarUsuario(txt_usulog.Text, txt_passlog.Password);
            if (res.Equals("")){
                lbl_errlog.Content = "Usuario o Contraseña incorrecta";
                lbl_errlog.Visibility = Visibility.Visible;
            }else
            {
                lbl_errlog.Visibility = Visibility.Hidden;
                MessageBox.Show("Bienvenido :" + res);
                Application.Current.MainWindow.Visibility = Visibility.Visible;
                this.Close();
            }

        }

        private void btn_register_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RegisterWindow  reg = new RegisterWindow();
            this.Close();
            reg.Visibility = Visibility.Visible;
        }
    }
}
