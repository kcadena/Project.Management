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
            this.Visibility = Visibility.Hidden;

            LoginWindow log = new LoginWindow();            
            log.Visibility = Visibility.Visible;
            
        }
    }
}
