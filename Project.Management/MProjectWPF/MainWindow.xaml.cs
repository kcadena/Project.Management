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
            LoginWindow log= new LoginWindow();
            this.Visibility = Visibility.Hidden;
            log.Visibility = Visibility.Visible;
            
        }
    }
}
