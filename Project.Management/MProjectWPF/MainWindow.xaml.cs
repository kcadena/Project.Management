using System.Windows;
using MProjectWPF.UserControl.Inicio;
using MProjectWPF.UsersControls;
using System;
using ControlDB.Model;
using MProjectWPF.Controller;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using MProjectWPF.UsersControls.UserControls;
using MProjectWPF.MProjectWCF;
using System.Diagnostics;
using System.ServiceModel;
using System.Collections.Generic;
using ControlDB.ChatServiceDuplex;
using ControlDB.ChatService;
using mc = MProjectChat;
using System.IO;

namespace MProjectWPF
{   
    public partial class MainWindow : Window 
    {
        #region Variables Modelo Datos
        public MProjectDeskSQLITEEntities dbMP;
        public usuarios_meta_datos usuMod;
        public Usuarios usuCon;
        #endregion

        #region Controles Usuario
        public UserDetails usuDet;
        public UsersPanel  usuPan;
        UserSessionPanel usp = null;
        #endregion

        #region Variables Sistema
        public bool userSessionIsActive= false;
        public bool internet, serv;
        public Dictionary<string, mc.MainWindow> lstWinChat;
        #endregion

        #region Variables Servicio ChatDuplex 
        ReceiveChat receiveChat;
        InstanceContext instanceContext;
        public SendChatServiceClient chatDuplex;
        public ControlDB.ChatServiceDuplex.User userDuplex;
        #endregion

        #region VariablesServiciosMproject
        MProjectServiceClient ServiceClient;
        #endregion

        #region Variables Servicio Chat
        public ChatServiceClient chat;
        public ControlDB.ChatService.User user;     
        #endregion

        //CONSTRUTOR DE LA INTERFAZ PRINCIPAl MPROJECT
        public MainWindow()
        {   
            dbMP = new MProjectDeskSQLITEEntities();
            InitializeComponent();
            Visibility = Visibility.Hidden;
            usuPan = new UsersPanel(this);
            LoginWindow logWin = new LoginWindow(this);
            logWin.Show();
        }

        public bool user_Click(string email, string pass)
        {
            usuCon = new Usuarios(dbMP);
            usuMod = usuCon.getUser(email, pass);
            
            if (usuMod != null)
            {
                usuPan.loadActUser(usuMod);
                nameConection.Content = usuMod.nombre + " " + usuMod.apellido;
                usuDet = new UserDetails(usuMod);
                lstWinChat = new Dictionary<string, mc.MainWindow>();
                addLabels();

                #region Conexion Chat Duplex
                //msc = new MProjectServiceClient();
                //rc = new ReceiveChat();
                //inst = new InstanceContext(rc);
                //chat = new SendChatServiceClient(inst);

                //rc.mainW = this;

                //user = new User();
                //user.UsuDic = new Dictionary<string, string>();
                //user.UsuDic["id_usuario"] = "" + usuMod.id_usuario;
                //user.UsuDic["nombre"] = "" + usuMod.nombre + " " + usuMod.apellido;
                //user.UsuDic["e_mail"] = "" + usuMod.e_mail;
                //user.UsuDic["cargo"] = "" + usuMod.cargo;
                //user.UsuDic["entidad"] = "" + usuMod.entidad;
                //user.UsuDic["telefono"] = "" + usuMod.telefono;
                //user.UsuDic["genero"] = "" + usuMod.genero;
                //user.AvatarID = null;

                ////chat.ClientCredentials.UserName.Password = "NJ@udn2011";
                ////chat.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, "WCFClient");

                //chat.Start(user);
                #endregion
                #region Conexion Chat
                chat = new ChatServiceClient();
                #endregion

                Thread oThread = new Thread(() =>
                {
                    InternetAccess();
                });
                oThread.Start();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void addLabels()
        {
            lal.Children.Clear();
            lstmen.Items.Clear();
            lstrec.Items.Clear();

            spViewA.Children.Clear();
            spViewA.Children.Add(usuDet);

            spViewB.Children.Clear();
            spViewB.Children.Add(usuPan);

            lstmen.Items.Add(new LabelMenu("Nuevo Proyecto", this, 1));
            lstmen.Items.Add(new LabelMenu("Abrir Proyecto", this, 2));
            lstmen.Items.Add(new LabelMenu("Importar Proyecto", this, 3));            
            lstmen.Items.Add(new LabelMenu("Nueva Plantilla", this, 4));

            lal.Children.Add(new ListProject(this, usuMod));

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
                    //chat.DoConnect(getUser().UsuDic, getUser().AvatarID);
                    chat.DoConnect(getUser());
                    usuPan.loadUsers(chat.listUsers());
                    indicator.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        indicator.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/ind_Green.png"));
                    }));
                    internet = true;
                }
                catch(Exception err)
                {
                    MessageBox.Show(err.ToString());
                    internet = false;                    
                    indicator.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        serv = false;
                        usuPan.deleteUsers();
                        indicator.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/ind_Red.png"));
                    }));                   
                }
                Thread.Sleep(100);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {   
            if (userSessionIsActive) { 
                usp = new UserSessionPanel(usuMod);            
                usp.Margin = new Thickness(0,53,5,0);
                grid_main_window.Children.Add(usp);
                userSessionIsActive = false;
                arrowPerf.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/triangle-up.png"));
            }
            else
            {   
                grid_main_window.Children.Remove(usp);
                userSessionIsActive = true;
                arrowPerf.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Icons/16px/triangle-down.png"));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                chat.DoDisConnect(user);
            }
            catch { }
            Process.GetCurrentProcess().Kill();
        }

        private void uploadproject_Click(object sender, RoutedEventArgs e)
        {   
            LogXml logxml = new LogXml(usuMod.repositorios_usuarios.ruta_repositorio_local + "/Log/log.xml");
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
                    usuCon.addUser(ServiceClient.getUser(log1[1]));
                    spViewB.Children.Clear();
                    //spViewB.Children.Add(new UsersPanel(this, usuControl));
                }
                else
                {
                    usuCon.removeUser(id);
                    spViewB.Children.Clear();
                    //spViewB.Children.Add(new UsersPanel(this, usuControl));
                }
            }
        }

        private void downloadproject_Click(object sender, RoutedEventArgs e)
        {
            ServerController.getProyect(""+usuMod.id_usuario,dbMP);
            addLabels();
            MessageBox.Show("Accion Descarga Terminada");
        }

        //OBTIENE INFORMACION DEL USUARIO LOGGEADO
        private ControlDB.ChatService.User getUser()
        {
            user = new ControlDB.ChatService.User();
            user.UsuDic = new Dictionary<string, string>();
            user.UsuDic["id_usuario"] = "" + usuMod.id_usuario;
            user.UsuDic["nombre"] = "" + usuMod.nombre + " " + usuMod.apellido;
            user.UsuDic["e_mail"] = "" + usuMod.e_mail;
            user.UsuDic["cargo"] = "" + usuMod.cargo;
            user.UsuDic["entidad"] = "" + usuMod.entidad;
            user.UsuDic["telefono"] = "" + usuMod.telefono;
            user.UsuDic["genero"] = "" + usuMod.genero;

            //FileStream filestream = null;
            //user.AvatarID = filestream;
            //if (usuMod.imagen != "")
            //{
            //    string sourceImage = usuMod.repositorios_usuarios.ruta_repositorio_local + "\\" + usuMod.imagen;
            //    filestream = File.OpenRead(sourceImage);

            //    user.AvatarID = filestream;
            //}           
            return user;
        }
    }
}

//private void uploadproject_Click(object sender, RoutedEventArgs e)
//{
//    MProjectServiceClient sc = new MProjectServiceClient();
//    try
//    {
//        Dictionary<string, string> u = new Dictionary<string, string>();
//        u.Add("e_mail", usuMod.e_mail);
//        u.Add("id_usuario", "" + usuMod.id_usuario);
//        u.Add("nombre", usuMod.nombre);
//        u.Add("apellido", usuMod.apellido);
//        u.Add("genero", usuMod.genero);
//        u.Add("pass", usuMod.usuarios.pass);
//        u.Add("cargo", usuMod.cargo);
//        u.Add("telefono", usuMod.telefono);
//        u.Add("entidad", usuMod.entidad);
//        u.Add("imagen", usuMod.imagen);
//        u.Add("administrador", "" + usuMod.usuarios.administrador);

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