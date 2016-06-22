using MProjectWPF.Controller;
using MProjectWPF.Model;
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

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para CardProject.xaml
    /// </summary>
    public partial class CardProject : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        caracteristicas car;
        MProjectDeskSQLITEEntities dbMP;

        public CardProject(MainWindow mw,caracteristicas c,MProjectDeskSQLITEEntities db)
        {
            InitializeComponent();
            dbMP = db;
            Proyectos pro = new Proyectos();            
            List<proyectos_meta_datos> lpro = pro.getProjectMeta((long)c.id_proyecto);           
            titleCard.Content = lpro.First().valor;
            percentCard.Text = ""+c.porcentaje_cumplimido; 
            car = c;
            mainW = mw;
        }
        
        private void enterBtnCard_Click(object sender, RoutedEventArgs e)
        {
            mainW.vp1.Visibility = Visibility.Hidden;
            ExplorerProject exPro = new ExplorerProject(mainW,car,dbMP,""+titleCard.Content);
            mainW.viewPlan.Children.Add(exPro);            
        }
    }
        
}
