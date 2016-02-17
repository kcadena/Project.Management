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

namespace MProjectWPF.UsersControlls
{
    /// <summary>
    /// Lógica de interacción para ListProject.xaml
    /// </summary>
    public partial class ListProject : UserControl
    {
        public ListProject(MainWindow mw,string header)
        {
            InitializeComponent();
            groupBox.Header = header;            
            for(int i = 1;i<11;i++)  lst_prj.Items.Add(new LabelProject(i+"",mw));
        }
    }
}
