using System.Windows;

namespace MProjectWPF.UsersControls.ChatControls
{
    /// <summary>
    /// Lógica de interacción para LabelMessage.xaml
    /// </summary>
    public partial class LabelMessage : System.Windows.Controls.UserControl
    {
        public LabelMessage(bool local,string msg,string usu)
        {
            InitializeComponent();
            userLbl.Content = usu;
            if (local)
            {
                localTxt.Text = msg;
                localLbl.Visibility = Visibility.Visible;
            }
            else
            {
                servTxt.Text = msg;
                servLbl.Visibility = Visibility.Visible;
            }
        }
    }
}
