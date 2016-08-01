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
using ControlDB.Model;
using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using MProjectWPF.UsersControls.ProjectControls;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
using MProjectWPF.UsersControls.TemplatesControls;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Interaction logic for ExplorerProject.xaml
    /// </summary>
    public partial class ExplorerProject : System.Windows.Controls.UserControl
    {

        public MainWindow mainW;        
        public Caracteristicas car;
        public LabelTreeActivity lta,lastSelectlta;
        string title;
        public proyectos pro;
        ProjectPanel proPan;
        public List<LabelTreeActivity> child;

        //cargar
        public ExplorerProject(MainWindow mw, proyectos p,string t)
        {
            InitializeComponent();
            mainW = mw;
            title = t;
            pro = p;
            titlePro.Text = title;

            child = new List<LabelTreeActivity>();

            proPan = new ProjectPanel(pro, mainW, this);
            workplaceGrid.Children.Add(proPan);

            car = new Caracteristicas(mainW);
            car.getActivitiesCharacteristics(pro.caracteristicas,tvPro,null,this);
        }
        //actualizar
        public ExplorerProject(MainWindow mw, string pName, proyectos p, List<BoxField> bf, List<BoxField> tbf, string fileSource, string fileName, string detail)
        {
            InitializeComponent();
            mainW = mw;
            title = pName.ToUpper();
            pro = p;
            titlePro.Text = title;

            proPan = new ProjectPanel(mainW, this, pName, p, bf, tbf, fileSource, fileName, detail);
            workplaceGrid.Children.Add(proPan);

            child = new List<LabelTreeActivity>();

            car = new Caracteristicas(mainW);
            car.getActivitiesCharacteristics(pro.caracteristicas, tvPro, null, this);
        }
        //primer
        public ExplorerProject(MainWindow mw,string pName, List<BoxField> bf, List<BoxField> tbf, string fileSource,string fileName,string detail)
        {
            InitializeComponent();
            mainW = mw;
            title = pName.ToUpper();            
            titlePro.Text = title;
            proPan = new ProjectPanel(mainW, this, pName, bf, tbf, fileSource, fileName, detail);
            workplaceGrid.Children.Add(proPan);
            car = new Caracteristicas(mainW);
            child = new List<LabelTreeActivity>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            proPan.wc.unlockDocument();
            mainW.addLabels();
            mainW.viewPlan.Children.Remove(this);
            mainW.vp1.Visibility = Visibility.Visible;
        }
               
        private void titlePro_MouseEnter(object sender, MouseEventArgs e)
        {
            titlePro.TextDecorations = TextDecorations.Underline;
            titlePro.FontSize = 15;
        }

        private void titlePro_MouseLeave(object sender, MouseEventArgs e)
        {
            titlePro.TextDecorations = null;
            titlePro.FontSize = 13.333;
        }

        private void titlePro_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {   
            workplaceGrid.Children.Clear();
            workplaceGrid.Children.Add(new ProjectPanel(pro,mainW,this));
        }

        private void btnBackItem_Click(object sender, RoutedEventArgs e)
        {            
            if (lta != null)
            {              
                titlePro.Text = lta.lblName.Text.ToUpper();
                tvPro.Items.Clear();
                car.getActivitiesCharacteristics(lta.car,tvPro,lta.lab_father,this);
                lta = lta.lab_father;
            }
            else
            {
                btnBackItem.Visibility = Visibility.Collapsed;
                titlePro.Text = title.ToUpper();
                tvPro.Items.Clear();
                car.getActivitiesCharacteristics(pro.caracteristicas, tvPro,null, this);
            }
        }
    }
}