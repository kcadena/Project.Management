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
using MProjectWPF.MProjectWCF;

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
        MProjectServiceClient msc = new MProjectServiceClient();

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
            oThread.Start();
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
                    indicator.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        indicator.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/ind_Green.png"));
                    }));
                    internet = true;
                    //try
                    //{
                    //    if (msc.countlog() > 0)
                    //    {
                    //        ValidateUser(msc.readLog());
                    //    }
                    //}
                    //catch { }
                }
                catch
                {
                    internet = false;
                    indicator.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        indicator.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/ind_Red.png"));
                    }));                   
                }
                Thread.Sleep(10);
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
            msc.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private void uploadproject_Click(object sender, RoutedEventArgs e)
        {   
            LogXml logxml = new LogXml(usuModel.repositorios_usuarios.ruta_repositorio_local + "/Log/log.xml");
            logxml.readLogUpdate(dbMP);
            MessageBox.Show("Proyecto en la nube");
        }

        private void ValidateUser(string[][] logs)
        {   
            foreach (var log1 in logs)
            {
                string evento = log1[0];
                int id = Convert.ToInt32(log1[1]);

                
                if(evento == "add")
                {
                    usuControl.addUser(msc.getUser(log1[1]));
                    spViewB.Children.Clear();
                    spViewB.Children.Add(new UsersPanel(this, usuControl));
                }
                else
                {
                    usuControl.removeUser(id);
                    spViewB.Children.Clear();
                    spViewB.Children.Add(new UsersPanel(this, usuControl));
                }
            }
        }

        private void downloadproject_Click(object sender, RoutedEventArgs e)
        {
            ServerController.getProyect(""+usuModel.id_usuario,dbMP);
            addLabels();
            MessageBox.Show("Proyecto actualizado");
        }
    } 
}

//private void uploadproject_Click(object sender, RoutedEventArgs e)
//{
//    MProjectServiceClient sc = new MProjectServiceClient();
//    try
//    {
//        Dictionary<string, string> u = new Dictionary<string, string>();
//        u.Add("e_mail", usuModel.e_mail);
//        u.Add("id_usuario", "" + usuModel.id_usuario);
//        u.Add("nombre", usuModel.nombre);
//        u.Add("apellido", usuModel.apellido);
//        u.Add("genero", usuModel.genero);
//        u.Add("pass", usuModel.usuarios.pass);
//        u.Add("cargo", usuModel.cargo);
//        u.Add("telefono", usuModel.telefono);
//        u.Add("entidad", usuModel.entidad);
//        u.Add("imagen", usuModel.imagen);
//        u.Add("administrador", "" + usuModel.usuarios.administrador);

//        string[] lusu = sc.addUser(u);
//        if (lusu.LongLength == 0)
//        {
//            MessageBox.Show("GOOD");
//        }
//        else
//        {
//            MessageBox.Show("ERROR");
//        }
//    }
//    catch (Exception err)
//    {
//        MessageBox.Show(err.Message);
//    }
//}