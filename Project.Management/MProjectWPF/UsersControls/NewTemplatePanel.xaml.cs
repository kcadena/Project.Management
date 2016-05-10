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
    /// Lógica de interacción para NewTemplatePanel.xaml
    /// </summary>
    public partial class NewTemplatePanel : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        public NewTemplatePanel(MainWindow mw)
        {
            InitializeComponent();
            mainW = mw;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            mainW.vp1.Visibility = Visibility.Visible;
        }
    }
}
