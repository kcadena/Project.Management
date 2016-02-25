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

using MProjectWPF.Controller.FromModel;
using MProjectWPF.Controller.Classes;
using MProjectWPF.Controller;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Interaction logic for AddAtivity.xaml
    /// </summary>
    public partial class AddAtivity : System.Windows.Controls.UserControl
    {
        TreeViewItem tvi;
        TreeView tvPro;
        Grid mainGrid;
        public AddAtivity(TreeViewItem tvi, TreeView tvPro,Grid main)
        {
            InitializeComponent();
            this.tvi = tvi;
            this.tvPro = tvPro;
            mainGrid = main;
        }

        private void bntAddAct_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stp = (StackPanel) tvi.Header;
            string idFol = stp.Children.OfType<Label>().ElementAt(1).Content.ToString();


            Dictionary<String, string> data = new Dictionary<string, string>();
            data["nom"] = txtNom.Text;
            data["des"] = txtDes.Text;
            data["est"] = txtEst.Text;
            data["per"] = txtPer.Text;
            data["dur"] = txtDur.Text;
            data["fol"] = idFol;
            
            data["typDur"] = "Horas";
            data["fec"] = DateTime.Now.Date.ToString();
            data["id_pro"] ="";
            data["fat_cha"]= "";
            data["pro_fat"] = "1";
            data["act"] = "";
            data["opt"] = "3";

            Characteristics cha = new Characteristics();
            cha.CreateCharacteristicsActivity(data);
            

            Folders fol = new Folders();
            FolderTree treFol = new FolderTree();
            TreeViewItem tv = treFol.arrange(fol.getStructureFolders());
            tvPro.Items.Clear();
            tvPro.Items.Add(tv);


        }
    }
}
