using ControlDB.ChatServiceDuplex;
using ControlDB.Model;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using mc = MProjectChat;
using ControlDB.ChatService;
using System.Collections.Generic;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para LabelUser.xaml
    /// </summary>
    public partial class LabelUser : System.Windows.Controls.UserControl
    {
        public usuarios_meta_datos usuMod;
        public ControlDB.ChatServiceDuplex.User user;
        public SendChatServiceClient chat;
        public MainWindow mainW;
        public mc.MainWindow mainChat;
        private MainWindow mainWin;
        public ControlDB.ChatService.User usu;

        public LabelUser(usuarios_meta_datos usuMod)
        {
            InitializeComponent();
            this.usuMod = usuMod;

            if (usuMod.imagen != null)
            {
                try
                {
                    string repositorioLocal = usuMod.repositorios_usuarios.ruta_repositorio_local;
                    string sourceImage = repositorioLocal + "\\" + usuMod.imagen;
                    imgProfile.Source = new BitmapImage(new Uri(sourceImage));
                }
                catch { }
                
            }
            else
            {
                if (usuMod.genero == "F")
                {
                    imgProfile.Source = new BitmapImage(new Uri("pack://application:,,/Resources/womenprofile.png"));
                }
                else
                {
                    imgProfile.Source = new BitmapImage(new Uri("pack://application:,,/Resources/manprofile.png"));
                }
            }
            nameUser.Text = (usuMod.nombre + " " + usuMod.apellido).ToUpper();
            emailUser.Text = usuMod.e_mail;
            occupationUser.Text = usuMod.cargo;
        }

        public LabelUser(MainWindow mainW, ControlDB.ChatServiceDuplex.User user)
        {
            InitializeComponent();
            chat = mainW.chatDuplex;
            this.user = user;
            this.mainW = mainW;

            if (user.AvatarID != null)
            {

            }
            else
            {
                if (user.UsuDic["genero"] == "F")
                {
                    imgProfile.Source = new BitmapImage(new Uri("pack://application:,,/Resources/womenprofile.png"));
                }
                else
                {
                    imgProfile.Source = new BitmapImage(new Uri("pack://application:,,/Resources/manprofile.png"));
                }
            }
            nameUser.Text = user.UsuDic["nombre"].ToUpper();
            emailUser.Text = user.UsuDic["e_mail"];
            occupationUser.Text = user.UsuDic["cargo"];
            
        }

        public LabelUser(MainWindow mainWin, ControlDB.ChatService.User usu)
        {
            InitializeComponent();
            this.mainWin = mainWin;
            this.usu = usu;
            try
            {
                nameUser.Text = usu.UsuDic["nombre"].ToUpper();
                emailUser.Text = usu.UsuDic["e_mail"];
                occupationUser.Text = usu.UsuDic["cargo"];
            }
            catch { }
            
        }

        public void showButtons()
        {
            btnChat.Visibility = Visibility.Visible;
            btnMessage.Visibility = Visibility.Visible;
        }

        public void hideButtons()
        {
            btnChat.Visibility = Visibility.Hidden;
            btnMessage.Visibility = Visibility.Hidden;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (user != null)
            {
                base.OnMouseMove(e);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    // Package the data.
                    DataObject data = new DataObject();

                    data.SetData(DataFormats.StringFormat, user.UsuDic["e_mail"]);
                    LabelUser lu = new LabelUser(mainW,user);
                    lu.changeBackground();
                    data.SetData("Object", lu);

                    // Inititate the drag-and-drop operation.
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        public void changeBackground()
        {
            var bc = new BrushConverter();
            grid.Background = (Brush)bc.ConvertFrom("#FF004166");
            nameUser.Foreground = Brushes.White;
            emailUser.Foreground = Brushes.White;
            occupationUser.Foreground = Brushes.White;
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            if (!mainW.lstWinChat.ContainsKey(user.UsuDic["id_usuario"]))
            {
                mainChat = new mc.MainWindow(chat, mainW.userDuplex, user, mainW.lstWinChat);
                mainChat.Show();
                mainW.lstWinChat.Add(user.UsuDic["id_usuario"], mainChat);
            }
        }

        private void btnMessage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
