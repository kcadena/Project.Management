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
    /// Lógica de interacción para newProjectPanel.xaml
    /// </summary>
    public partial class newProjectPanel : UserControl
    {
        MainWindow mW;
        public newProjectPanel(MainWindow mw)
        {
            InitializeComponent();
            mW = mw;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            mW.vp1.Visibility = Visibility.Visible;   
        }
    }
}
