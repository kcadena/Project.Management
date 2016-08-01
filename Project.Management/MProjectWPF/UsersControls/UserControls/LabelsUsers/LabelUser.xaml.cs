using ControlDB.Model;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para LabelUser.xaml
    /// </summary>
    public partial class LabelUser : System.Windows.Controls.UserControl
    {
        public usuarios_meta_datos usu;

        public LabelUser(usuarios_meta_datos u)
        {
            InitializeComponent();
            usu = u;
            if (usu.imagen != null)
            {
                string repositorioLocal = usu.repositorios_usuarios.ruta_repositorio_local;
                string image = usu.imagen;                
                string sourceImage = repositorioLocal + "\\perfil\\imagen\\" + usu.imagen;
                imgProfile.Source = new BitmapImage(new Uri(sourceImage));
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
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Package the data.
                DataObject data = new DataObject();

                data.SetData(DataFormats.StringFormat, usu.e_mail);
                LabelUser lu = new LabelUser(usu);
                lu.changeBackground();
                data.SetData("Object", lu);
                
                // Inititate the drag-and-drop operation.
                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
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
    }
}
