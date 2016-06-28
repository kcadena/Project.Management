using MProjectWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
