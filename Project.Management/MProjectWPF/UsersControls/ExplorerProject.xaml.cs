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

using MProjectWPF.Controller;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Interaction logic for ExplorerProject.xaml
    /// </summary>
    public partial class ExplorerProject : System.Windows.Controls.UserControl
    {
        public ExplorerProject()
        {
            InitializeComponent();
            Folders fol = new Folders();
            FolderTree treFol = new FolderTree();
            TreeViewItem tv = treFol.arrange(fol.getStructureFolders());
            tvPro.Items.Add(tv);
            
        }
    }
}
