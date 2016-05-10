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
    /// Interaction logic for AssignResponsibility.xaml
    /// </summary>
    public partial class AssignResponsibility : System.Windows.Controls.UserControl
    {
        Dictionary<string, string> inf;
        Grid main_grid;
        public AssignResponsibility(Dictionary<string, string> inf,Grid main_grid)
        {
            InitializeComponent();
            this.inf = inf;
            this.main_grid = main_grid;
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            GetUsers.Service1Client user = new GetUsers.Service1Client();
            String cad = txt_findUser.Text.ToLower();
            cad=cad.Replace(" ", "%");

            var dat=user.GetUsers(cad);

            listView_usersFound.Items.Clear();
            //x["usu"]

            if (dat.Count() != 0)
            {
                foreach (var x in dat)
                {
                    StackPanel pan = new StackPanel();
                    
                    CheckBox ch = new CheckBox();
                    ch.Content = x["usu"] + "\n" + x["mail"];
                    Label id = new Label();
                    id.Visibility = Visibility.Hidden;
                    id.Content = x["id"].ToString();


                    pan.Children.Add(ch);
                    pan.Children.Add(id);

                    listView_usersFound.Items.Add(pan);
                    //listView_usersFound.Items.
                }
            }
            else MessageBox.Show("No se encontro al usuario.");
        }

        private void btn_accept_Click(object sender, RoutedEventArgs e)
        {
            List<Dictionary<string, string>> dat = new List<Dictionary<string, string>>();

            foreach(var x in listView_usersFound.Items)
            {
                StackPanel pan = (StackPanel)x;
                CheckBox ch = pan.Children.OfType<CheckBox>().ElementAt(0) ;
                if ((bool)ch.IsChecked)
                {
                    Dictionary<string, string> aux = new Dictionary<string, string>();
                    aux["car"] = inf["car"];
                    aux["usu"] = pan.Children.OfType<Label>().ElementAt(0).Content.ToString();
                    
                    MessageBox.Show(aux["usu"]+"   "+aux["car"]);
                }
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.main_grid.Children.Remove(this);
        }
    }
}
