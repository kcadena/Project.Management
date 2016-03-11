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
    /// Lógica de interacción para CardProject.xaml
    /// </summary>
    public partial class CardProject : UserControl
    {
        public CardProject()
        {
            InitializeComponent();
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
    }
}
