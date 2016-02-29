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
        Dictionary<string, long> dat;

        public AddAtivity(TreeViewItem tvi, TreeView tvPro,Grid main,Dictionary<string,long> id)
        {
            InitializeComponent();
            this.tvi = tvi;
            this.tvPro = tvPro;
            mainGrid = main;
            this.dat = id;
        }

        private void bntAddAct_Click(object sender, RoutedEventArgs e)
        {
            Folders fol = new Folders();
            FolderTree treFol = new FolderTree(dat);
            
            string idFol = treFol.name(tvi, 1);
            string id_cha = treFol.name(tvi, 3);

            Dictionary<String, string> data = new Dictionary<string, string>();
            data["nom"] = txtNom.Text;
            data["des"] = txtDes.Text;
            data["est"] = txtEst.Text;
            data["per"] = txtPer.Text;
            data["dur"] = txtDur.Text;
            data["fol"] = idFol;
            
            data["typDur"] = "Horas";            
            data["id_pro"] ="";
            data["fat_prj"] = dat["id"].ToString();
            data["act"] = "";
            
            if (id_cha.Equals("-1"))
            {
               
                data["fat_cha"] = dat["car"].ToString();
                data["pos"] = "OK";
            }
            else {
                data["fat_cha"] = id_cha.ToString();
                data["pos"] = "NO";
            }

            Characteristics cha = new Characteristics();
            cha.CreateCharacteristicsActivity(data);
            

           
            TreeViewItem tv = treFol.arrange(fol.getStructureFolders(dat["id"]));
            tvPro.Items.Clear();
            tvPro.Items.Add(tv);

            mainGrid.Children.Remove(this);

        }
    }
}
