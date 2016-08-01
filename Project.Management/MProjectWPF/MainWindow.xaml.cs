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
using ControlDB.Model;
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
        public Usuarios usuControl;
        public usuarios_meta_datos usuModel;
        UserSessionPanel usp = null;
        public MProjectDeskSQLITEEntities dbMP;
        bool perAct = true;
        public bool internet;

        public MainWindow()
        {   
            dbMP = new MProjectDeskSQLITEEntities();
            InitializeComponent();
            usuControl = new Usuarios(dbMP);
            usuModel = usuControl.getUser("kelvin.cadena@gmail.com", "123");           
            addLabels();
            nameConection.Content = usuModel.nombre + " " + usuModel.apellido; 
            Thread oThread = new Thread(() =>
            {
                InternetAccess();
            });
            //oThread.Start();
            //Window1 win = new Window1();
            //win.Show();
        }

        private void user1_Click(object sender, RoutedEventArgs e)
        {
            usuModel = usuControl.getUser("kelvin.cadena@gmail.com", "123");
            addLabels();
            nameConection.Content = usuModel.nombre + " " + usuModel.apellido;
        }

        private void user2_Click(object sender, RoutedEventArgs e)
        {
            usuModel = usuControl.getUser("david@gmail.com", "123");
            addLabels();
            nameConection.Content = usuModel.nombre + " " + usuModel.apellido;
        }

        private void user3_Click(object sender, RoutedEventArgs e)
        {
            usuModel = usuControl.getUser("karenEst@hotmail.com", "123");
            addLabels();
            nameConection.Content = usuModel.nombre + " " + usuModel.apellido;
        }

        public void addLabels()
        {
            lal.Children.Clear();
            lstmen.Items.Clear();
            lstrec.Items.Clear();

            spViewA.Children.Clear();
            spViewA.Children.Add(new UserDetails(usuModel));

            spViewB.Children.Clear();
            spViewB.Children.Add(new UsersPanel(this, usuControl));
            
            lstmen.Items.Add(new LabelMenu("Nuevo Proyecto", this, 1));
            lstmen.Items.Add(new LabelMenu("Abrir Proyecto", this, 2));
            lstmen.Items.Add(new LabelMenu("Importar Proyecto", this, 3));
            
            if ((bool)usuModel.usuarios.administrador)
            {
                lstmen.Items.Add(new LabelMenu("Nueva Plantilla", this, 4));
            }
            lal.Children.Add(new ListProject(this, usuModel));

            lstrec.Items.Add(new LabelMenu("Proyectos de Ingenieria", this, 0));
            lstrec.Items.Add(new LabelMenu("Desarrollo Investic ", this, 0));
            lstrec.Items.Add(new LabelMenu("Acreditacion Sistemas", this, 0));
            lstrec.Items.Add(new LabelMenu("Desarrollo MProject", this, 0));
            lstrec.Items.Add(new LabelMenu("Aplicacion Inventario", this, 0));
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
                usp = new UserSessionPanel(usuModel);            
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

