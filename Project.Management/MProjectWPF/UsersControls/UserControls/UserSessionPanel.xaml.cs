using ControlDB.Model;
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
    /// Lógica de interacción para UserSessionPanel.xaml
    /// </summary>
    public partial class UserSessionPanel : System.Windows.Controls.UserControl
    {
        public UserSessionPanel(usuarios_meta_datos usu)
        {
            InitializeComponent();
            nameUser.Text = usu.nombre.ToUpper() + " " + usu.apellido.ToUpper();
            emailUser.Text = usu.e_mail;
        }
    }
}
