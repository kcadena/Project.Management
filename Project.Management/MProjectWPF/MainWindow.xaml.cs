using System.Windows;
using MProjectWPF.Windows.Inicio;

using MProjectWPF.UsersControls;

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

            //ExplorerProject exPro = new ExplorerProject();
            //this.grid_main_window.Children.Add(exPro);           
            
        }
    }
}
