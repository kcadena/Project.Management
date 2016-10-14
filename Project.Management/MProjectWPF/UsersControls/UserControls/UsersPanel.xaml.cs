using MProjectWPF.Controller;
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
    /// Lógica de interacción para UsersPanel.xaml
    /// </summary>
    public partial class UsersPanel : System.Windows.Controls.UserControl
    {
        MainWindow mainWin;
        LabelUser lu;
        public UsersPanel(MainWindow mw, Usuarios usu)
        {
            InitializeComponent();
            mainWin= mw;
            foreach (usuarios_meta_datos usua in usu.listUsers())
            {
                lstUser.Items.Add(new LabelUser(usua));
            }
        }

        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lu != null) lu.hideButtons();

            mainWin.spViewA.Children.Clear();
            lu =(LabelUser)lstUser.SelectedItem;
            lu.showButtons();            
            mainWin.spViewA.Children.Add(new UserDetails(lu.usu));
        }
    }
}
