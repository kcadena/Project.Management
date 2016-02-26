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

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Interaction logic for ExplorerProject.xaml
    /// </summary>
    public partial class ExplorerProject : System.Windows.Controls.UserControl
    {
        Grid main_grid;
        public ExplorerProject(Grid grd)
        {
            InitializeComponent();
            Folders fol = new Folders();
            FolderTree treFol = new FolderTree();
            TreeViewItem tv = treFol.arrange(fol.getStructureFolders());
            tvPro.Items.Add(tv);
            main_grid = grd;

        }

        private void tvPro_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(this.tvPro.SelectedItem.ToString());
            if(this.tvPro.SelectedItem != null)
            {
                ContextMenu conMen = new ContextMenu();

                MenuItem mitNA = new MenuItem();
                MenuItem mitDA = new MenuItem();

                mitNA.Header = "Nueva actividad";
                mitNA.Click += MitNA_Click;

                mitDA.Header = "Eliminar actividad";
                mitDA.Click += MitDA_Click;

                conMen.Items.Add(mitNA);
                conMen.Items.Add(mitDA);
                tvPro.ContextMenu = conMen;
            }
            
        }

        private void MitDA_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MitNA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddAtivity addAct = new AddAtivity((TreeViewItem)tvPro.SelectedItem, tvPro, main_grid,1);
                main_grid.Children.Add(addAct);
                
            }
            catch (Exception err){
                MessageBox.Show(err.ToString());
            }

           
        }
    }
}
// MessageBox.Show("ok");