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
    /// Lógica de interacción para CardProject.xaml
    /// </summary>
    public partial class CardProject : System.Windows.Controls.UserControl
    {
        MainWindow mainW;

        public CardProject(MainWindow mw)
        {
            InitializeComponent();
            mainW = mw;
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //enterBtnCard.Visibility = Visibility.Visible;           
        }
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //enterBtnCard.Visibility = Visibility.Hidden;
        }
        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void enterBtnCard_Click(object sender, RoutedEventArgs e)
        {
            mainW.vp1.Visibility = Visibility.Hidden;
            Dictionary<string, long> dat = new Dictionary<string, long>();
            dat["id"] = 1;
            dat["car"] = 11;

            ExplorerProject exPro = new ExplorerProject(mainW, dat);
            mainW.viewPlan.Children.Add(exPro);
            
        }
    }
        
}
