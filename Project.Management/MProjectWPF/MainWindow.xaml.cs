using System.Windows;
using MProjectWPF.UserControl.Inicio;

namespace MProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
            Window1 log= new Window1();
            this.Visibility = Visibility.Hidden;
            log.Visibility = Visibility.Visible;
            
        }
    }
}
