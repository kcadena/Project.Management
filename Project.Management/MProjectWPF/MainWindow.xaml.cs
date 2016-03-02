using System.Windows;
using MProjectWPF.UserControl.Inicio;
using System.Collections.Generic;
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
            //this.Visibility = Visibility.Hidden;

            //LoginWindow log = new LoginWindow();            
            //log.Visibility = Visibility.Visible;
            Dictionary<string, long> dat= new Dictionary<string, long>();
            dat["id"] = 1;
            dat["car"] = 11;
            ExplorerProject exPro = new ExplorerProject(grid_main_window,dat);
            this.grid_main_window.Children.Add(exPro);
            
        }
    }
}
