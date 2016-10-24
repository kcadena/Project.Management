using ControlDB.ChatService;
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

namespace MProjectWPF.UsersControls.UserControls
{
    /// <summary>
    /// Lógica de interacción para UserDetails.xaml
    /// </summary>
    public partial class UserDetails : System.Windows.Controls.UserControl
    {
        public UserDetails(usuarios_meta_datos usu)
        {
            InitializeComponent();
            nameTxt.Text = usu.nombre + " " + usu.apellido;
            emailTxt.Text = usu.e_mail;
            ocupationTxt.Text = usu.cargo;
            companyTxt.Text = usu.entidad;
            phoneTxt.Text = usu.telefono;
        }

        public UserDetails(User user)
        {
            InitializeComponent();
            nameTxt.Text = user.UsuDic["nombre"];
            emailTxt.Text = user.UsuDic["e_mail"];
            ocupationTxt.Text = user.UsuDic["cargo"];
            companyTxt.Text = user.UsuDic["entidad"];
            phoneTxt.Text = user.UsuDic["telefono"];
        }

    }
}
