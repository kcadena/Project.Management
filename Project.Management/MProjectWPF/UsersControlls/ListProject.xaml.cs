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
using MProjectWPF.Controller;
using System.Windows.Media.Animation;

namespace MProjectWPF.UsersControlls
{
    /// <summary>
    /// Lógica de interacción para ListProject.xaml
    /// </summary>
    public partial class ListProject : UserControl
    {
        MainWindow mainW;
        DbLitecontroller dbMP;
        Storyboard myStoryboard;

        public ListProject(MainWindow mw,string header)
        {
            InitializeComponent();
            groupBox.Header = header;
            mainW = mw;
            dbMP = new DbLitecontroller();
            if (header.Equals("LISTA PROYECTOS")) {                
                dbMP.buscarProyecto(lst_prj, mw);
                btn_newPry.Content = "Nuevo Proyecto";
            }
            else
            {
                dbMP.buscarPlantilla(lst_prj, mw);
                btn_newPry.Content = "Nueva Plantilla";
            }
            
        }

        private void btn_newPry_Click(object sender, RoutedEventArgs e)
        {
            myStoryboard = (Storyboard)mainW.Resources["showTemplatePanel"];
            myStoryboard.Begin(mainW);
            mainW.viewPlan.Children.Add(new ListProject(mainW, "LISTA DE PLANTILLAS"));
        }
    }
}
