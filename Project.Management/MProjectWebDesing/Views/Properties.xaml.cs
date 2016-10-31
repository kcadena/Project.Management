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

namespace MProjectWebDesing.Views
{
    /// <summary>
    /// Lógica de interacción para Properties.xaml
    /// </summary>
    public partial class Properties : UserControl
    {
        public Properties()
        {
            InitializeComponent();
        }
        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
           //txt.Text = "#" + ClrPcker_Background.SelectedColor.R.ToString() + ClrPcker_Background.SelectedColor.G.ToString() + ClrPcker_Background.SelectedColor.B.ToString();
        }

        private void colorPanel_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            //MessageBox.Show("Panel");
        }
    }
}
