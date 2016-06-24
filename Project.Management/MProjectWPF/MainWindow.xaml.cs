using System.Windows;
using MProjectWPF.UserControl.Inicio;
using System.Collections.Generic;
using MProjectWPF.UsersControls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System;
using System.Windows.Interop;
using System.Media;
using System.Windows.Controls;
using MProjectWPF.Model;
using MProjectWPF.Controller;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using MProjectWPF.UsersControls.UserControls;

namespace MProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Usuarios usu;
        public usuarios usua;
        UserSessionPanel usp = null;
        public MProjectDeskSQLITEEntities dbMP;
        bool perAct = true;
        public bool internet;

        public MainWindow()
        {
            dbMP = new MProjectDeskSQLITEEntities();
            InitializeComponent();
            usu = new Usuarios(dbMP);
            usua = usu.getUser("kelvin.cadena@gmail.com", "123");
            addLabels();
            nameConection.Content = usua.nombre + " " + usua.apellido; 
            Thread oThread = new Thread(() =>
            {
                InternetAccess();
            });
            //oThread.Start();
        }

        private void user1_Click(object sender, RoutedEventArgs e)
        {
            usua = usu.getUser("kelvin.cadena@gmail.com", "123");
            addLabels();
            nameConection.Content = usua.nombre + " " + usua.apellido;
        }

        private void user2_Click(object sender, RoutedEventArgs e)
        {
            usua = usu.getUser("david@gmail.com", "123");
            addLabels();
            nameConection.Content = usua.nombre + " " + usua.apellido;
        }

        private void user3_Click(object sender, RoutedEventArgs e)
        {
            usua = usu.getUser("karenEst@hotmail.com", "123");
            addLabels();
            nameConection.Content = usua.nombre + " " + usua.apellido;
        }

        public void addLabels()
        {
            lal.Children.Clear();
            lstmen.Items.Clear();
            lstrec.Items.Clear();

            spViewA.Children.Clear();
            spViewA.Children.Add(new UserDetails(usua));

            spViewB.Children.Clear();
            spViewB.Children.Add(new UsersPanel(this, usu));
            
            lstmen.Items.Add(new LabelProject("Nuevo Proyecto", this, 1));
            lstmen.Items.Add(new LabelProject("Abrir Proyecto", this, 2));
            lstmen.Items.Add(new LabelProject("Importar Proyecto", this, 3));
            long i = 0;
            foreach (var tu in usua.tipos_usuarios)
            {
                i = tu.id_tipo_usu;
            }
            if (i == 1)
            {
                lstmen.Items.Add(new LabelProject("Nueva Plantilla", this, 4));
            }
            lal.Children.Add(new ListProject(this, usua, dbMP));

            lstrec.Items.Add(new LabelProject("Proyectos de Ingenieria", this, 0));
            lstrec.Items.Add(new LabelProject("Desarrollo Investic ", this, 0));
            lstrec.Items.Add(new LabelProject("Acreditacion Sistemas", this, 0));
            lstrec.Items.Add(new LabelProject("Desarrollo MProject", this, 0));
            lstrec.Items.Add(new LabelProject("Aplicacion Inventario", this, 0));
        }

        private void InternetAccess()
        {
            while (true)
            {   
                try
                {
                    System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry("www.google.com");   
                    internet = true;
                }
                catch
                {
                    internet = false;
                }
                indicator.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    if (internet)
                    {
                        indicator.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/ind_Green.png"));
                    }
                    else
                    {
                        indicator.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/ind_Red.png"));
                    }
                }));
                Thread.Sleep(100);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {   
            if (perAct) { 
                usp = new UserSessionPanel(usua);            
                usp.Margin = new Thickness(0,53,5,0);
                grid_main_window.Children.Add(usp);                
                perAct = false;
                arrowPerf.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/triangle-up.png"));
            }
            else
            {   
                grid_main_window.Children.Remove(usp);                               
                perAct = true;
                arrowPerf.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/triangle-down.png"));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    } 
}

