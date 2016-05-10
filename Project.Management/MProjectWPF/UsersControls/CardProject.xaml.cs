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
    public partial class CardProject : UserControl
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
            mainW.vp1.Children.Add(new ExplorerProject());
        }
    }
        
}
