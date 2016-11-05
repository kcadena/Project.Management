using ControlDB.ChatService;
using ControlDB.ChatServiceDuplex;
using ControlDB.Model;

namespace MProjectWPF.UsersControls.UserControls
{
    /// <summary>
    /// Lógica de interacción para UserDetails.xaml
    /// </summary>
    public partial class UserDetails : System.Windows.Controls.UserControl
    {
        public string id;

        public UserDetails(ControlDB.ChatService.User user)
        {
            InitializeComponent();
            nameTxt.Text = user.UsuDic["nombre"];
            emailTxt.Text = user.UsuDic["e_mail"];
            ocupationTxt.Text = user.UsuDic["cargo"];
            companyTxt.Text = user.UsuDic["entidad"];
            phoneTxt.Text = user.UsuDic["telefono"];
        }

        public UserDetails(usuarios_meta_datos usu)
        {
            InitializeComponent();
            nameTxt.Text = usu.nombre + " " + usu.apellido;
            emailTxt.Text = usu.e_mail;
            ocupationTxt.Text = usu.cargo;
            companyTxt.Text = usu.entidad;
            phoneTxt.Text = usu.telefono;
        }

        public UserDetails(ControlDB.ChatServiceDuplex.User user)
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
