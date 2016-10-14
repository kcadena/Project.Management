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
using System.Windows.Threading;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Interaction logic for ExplorerProject.xaml
    /// </summary>
    public partial class ExplorerProject : System.Windows.Controls.UserControl
    {

        public MainWindow mainW;        
        public Caracteristicas carCon;
        public LabelTreeActivity ltaFather,lastSelectlta;
        public string title;
        public proyectos proMod;
        
        public ProjectPanel proPanRoot;

        public ActivityPanel actPanBack;
        public ActivityPanel actPanCurrent;

        //CARGAR
        public ExplorerProject(MainWindow mw, proyectos proMod ,string t)
        {
            InitializeComponent();
            mainW = mw;
            title = t;
            this.proMod = proMod;
            titlePro.Text = title;
            carCon = new Caracteristicas(mainW);
            proPanRoot = new ProjectPanel(proMod, mainW, this);
            
            carCon.getActivitiesCharacteristics(proMod.caracteristicas, tvPro, null, this);
            workplaceGrid.Children.Add(proPanRoot);           
        }

        //ACTUALIZAR
        public ExplorerProject(MainWindow mw, proyectos proMod, List<BoxField> bf, List<BoxField> tbf, Dictionary<string, string> dic)
        {
            InitializeComponent();
            mainW = mw;
            title = dic["pName"].ToUpper();
            this.proMod = proMod;
            titlePro.Text = title;

            proPanRoot = new ProjectPanel(mainW, this, proMod, bf, tbf, dic);
            workplaceGrid.Children.Add(proPanRoot);
            
            carCon = new Caracteristicas(mainW);
            carCon.getActivitiesCharacteristics(proMod.caracteristicas, tvPro, null, this);
        }
        
        //CREAR
        public ExplorerProject(MainWindow mw, List<BoxField> bf, List<BoxField> tbf, Dictionary<string,string> dic)
        {
            InitializeComponent();
            
            mainW = mw;
            title = dic["pName"].ToUpper();            
            titlePro.Text = title;
            proPanRoot = new ProjectPanel(mainW, this, bf, tbf, dic);
            workplaceGrid.Children.Add(proPanRoot);
            carCon = new Caracteristicas(mainW);
        }

        //PERMITE SALIR DEA INTERFAZ
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            proPanRoot.wc.unlockDocument();
            mainW.addLabels();
            mainW.viewPlan.Children.Remove(this);
            mainW.vp1.Visibility = Visibility.Visible;
        }
               
        //EFECTOS TITULO/////
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
        //FINISH////////////

        //CLICK TITULO
        private void titlePro_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(lastSelectlta != null)
            {
                lastSelectlta.gridSh.Visibility = Visibility.Hidden;
            }
            if(actPanCurrent != null)
            {
                workplaceGrid.Children.Clear();
                workplaceGrid.Children.Add(actPanCurrent);
            }
            else
            {
                workplaceGrid.Children.Clear();
                workplaceGrid.Children.Add(proPanRoot);
            }
           
        }

        //REGRESAR ESTRUCTURA ANTERIOR ACTIVIDAD
        private void btnBackItem_Click(object sender, RoutedEventArgs e)
        {            
            if (ltaFather != null)
            {              
                titlePro.Text = ltaFather.lblName.Text.ToUpper();
                tvPro.Items.Clear();
                carCon.getActivitiesCharacteristics(ltaFather.car,tvPro, ltaFather.lab_father,this);
                actPanCurrent = new ActivityPanel(ltaFather.car.actividades.First(), ltaFather, this);
                ltaFather = ltaFather.lab_father;                
            }
            else
            {
                btnBackItem.Visibility = Visibility.Collapsed;
                btnHome.Visibility = Visibility.Collapsed;
                titlePro.Text = title.ToUpper();
                tvPro.Items.Clear();
                carCon.getActivitiesCharacteristics(proMod.caracteristicas, tvPro,null, this);
                actPanCurrent = null;
            }
        }
        
        public  void removeItem(LabelTreeActivity lta)
        {
            tvPro.Items.Remove(lta);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            btnBackItem.Visibility = Visibility.Collapsed;
            btnHome.Visibility = Visibility.Collapsed;
            titlePro.Text = title.ToUpper();
            tvPro.Items.Clear();
            workplaceGrid.Children.Clear();
            workplaceGrid.Children.Add(proPanRoot);
            tvPro.Items.Clear();
            carCon.getActivitiesCharacteristics(proPanRoot.proMod.caracteristicas, tvPro, null, this);
            actPanCurrent = null;
        }

        public  int childsCount()
        {
            return tvPro.Items.Count;
        }
    }
}