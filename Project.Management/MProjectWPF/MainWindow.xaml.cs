using System.Windows;
using MProjectWPF.Windows.Inicio;


using MProjectWPF.UsersControlls;
using System.Windows.Media.Animation;

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

            /*LoginWindow log = new LoginWindow();            
            log.Visibility = Visibility.Visible;*/

            

            viewPro.Children.Add(new ListProject(this,"LISTA PROYECTOS"));
                      
            
        }
    }
}
