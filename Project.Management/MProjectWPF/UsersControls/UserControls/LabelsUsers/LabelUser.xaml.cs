using ControlDB.ChatService;
using ControlDB.Model;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using mc = MProjectChat;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para LabelUser.xaml
    /// </summary>
    public partial class LabelUser : System.Windows.Controls.UserControl
    {
        public usuarios_meta_datos usu;
        public User user;
        public SendChatServiceClient chat;
        public MainWindow mainW;
        public mc.MainWindow mainChat;

        public LabelUser(usuarios_meta_datos u)
        {
            InitializeComponent();
            usu = u;
            if (usu.imagen != null)
            {
                try
                {
                    string repositorioLocal = usu.repositorios_usuarios.ruta_repositorio_local;
                    string image = usu.imagen;
                    string sourceImage = repositorioLocal + "\\perfil\\imagen\\" + usu.imagen;
                    imgProfile.Source = new BitmapImage(new Uri(sourceImage));
                }
                catch { }
                
            }
            else
            {
                if (usu.genero == "F")
                {
                    imgProfile.Source = new BitmapImage(new Uri("pack://application:,,/Resources/womenprofile.png"));
                }
                else
                {
                    imgProfile.Source = new BitmapImage(new Uri("pack://application:,,/Resources/manprofile.png"));
                }
            }
            nameUser.Text = (usu.nombre + " " + usu.apellido).ToUpper();
            emailUser.Text = usu.e_mail;
            occupationUser.Text = usu.cargo;
        }

        public LabelUser(MainWindow mainW,User user)
        {
            InitializeComponent();
            chat = mainW.chat;
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
            mainChat = new mc.MainWindow(chat,mainW.user,user);
            mainChat.Show();
            mainW.lstWinChat.Add(user.UsuDic["id_usuario"],mainChat);
        }

        private void btnMessage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
