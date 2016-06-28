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


using System.Threading;
using MProjectWPF.Controller;
using MProjectWPF.Model;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Interaction logic for ExplorerProject.xaml
    /// </summary>
    public partial class ExplorerProject : System.Windows.Controls.UserControl
    {
        MainWindow mainW;

        /*
        1-> Crear folders  
        2-> Renombrar  folders
        3-> renombrar Actividades 
        */

        public ExplorerProject(MainWindow mw, caracteristicas id_pro,MProjectDeskSQLITEEntities db,string title)
        {
            InitializeComponent();
            titlePro.Content = title;
            Caracteristicas car = new Caracteristicas(db);
            car.getActivitiesCharacteristics(tvPro,id_pro,this);
            mainW = mw;            
        }

        private void tvPro_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainW.viewPlan.Children.Remove(this);
            mainW.vp1.Visibility = Visibility.Visible;
        }
       
    }
}