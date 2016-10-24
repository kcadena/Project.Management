using System.Collections.Generic;
using System.Windows.Controls;
using ControlDB.Model;
using ControlDB.ChatService;

namespace MProjectWPF.UsersControls.UserControls
{
    /// <summary>
    /// Lógica de interacción para UsersPanel.xaml
    /// </summary>
    public partial class UsersPanel : System.Windows.Controls.UserControl
    {
        MainWindow mainWin;
        LabelUser lu;
        LabelUser actlu;
        string id;

        public UsersPanel(MainWindow mw)
        {
            InitializeComponent();
            mainWin= mw;
        }

        public void loadActUser(usuarios_meta_datos usuMod)
        {
            id = "" + usuMod.id_usuario;
            actlu = new LabelUser(usuMod);
            actUser.Children.Add(actlu);
        }

        public void loadUsers(List<User> users)
        {
            lstUser.Items.Clear();
            int con = 0;
            foreach (User usua in users)
            {
                if(usua.UsuDic["id_usuario"] != id)
                {
                    lstUser.Items.Add(new LabelUser(mainWin.chat,usua));
                    con++;
                }
                if (con >= 10) break;
            }
        }

        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lu != null) lu.hideButtons();

            mainWin.spViewA.Children.Clear();
            lu =(LabelUser)lstUser.SelectedItem;
            if (lu != null)
            {
                lu.showButtons();
                mainWin.spViewA.Children.Add(new UserDetails(lu.user));
            }
        }

        private void actUser_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lu != null) lu.hideButtons();            
            lstUser.SelectedIndex = -1;
            mainWin.spViewA.Children.Clear();
            lu = actlu;
            mainWin.spViewA.Children.Add(new UserDetails(lu.usu));
        }
    }
}
