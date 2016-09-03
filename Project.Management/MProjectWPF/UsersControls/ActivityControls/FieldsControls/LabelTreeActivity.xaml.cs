using MProjectWPF.Controller;
using ControlDB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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

namespace MProjectWPF.UsersControls.ActivityControls.FieldsControls
{

    /// <summary>
    /// Lógica de interacción para LabelTreeActivity.xaml
    /// </summary>
    public partial class LabelTreeActivity : System.Windows.Controls.UserControl
    {
        public string nombre_act { get; set; }
        public long id;
        public caracteristicas car;
        public actividades actMod;
        public LabelTreeActivity lab_father;
        MProjectDeskSQLITEEntities dbMP;
        ExplorerProject expP;
        public bool isFirst=false;
        public bool isLast=false;
        public int pos = 0;

        //CONSTRUCTOR CREAR ACTIVIDAD
        public LabelTreeActivity()
        {
            InitializeComponent();            
            showActivityIcon();
        }        

        //CONSTRUCTOR CARGAR ACTIVIDAD
        public LabelTreeActivity(actividades act, ExplorerProject ep, LabelTreeActivity l_father)
        {
            InitializeComponent();
            actMod = act;
            dbMP = ep.mainW.dbMP;
            expP = ep;
            lblName.Text = act.nombre;
            car = act.caracteristicas;
            lab_father = l_father;           
            

            if (car.id_caracteristica_padre == null)
            {
                ua.Visibility = Visibility.Hidden;
                da.Visibility = Visibility.Hidden;
            }

            if (act.folder == 0)
                showActivityIcon();
        }
              
        //ADICIONA EL LA ACTIVIDAD DEL MODELO
        public void loadActivityMod(actividades act, ExplorerProject ep, LabelTreeActivity l_father)
        {   
            actMod = act;
            dbMP = ep.mainW.dbMP;
            expP = ep;
            lblName.Text = act.nombre;
            car = act.caracteristicas;
            lab_father = l_father;
        }

        public void addFather(LabelTreeActivity lta)
        {
            lab_father = lta;
        }

        public void addchilds(LabelTreeActivity lta)
        {   
            tvi.Items.Add(lta);
        }

        public void removechilds(LabelTreeActivity lta)
        {
            tvi.Items.Remove(lta);
        }

        private void ua_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {  
            if (lab_father != null)
            {
                moveItem(lab_father.tvi,pos-1);
            }
            else
            {
                moveItem(expP.tvPro, pos-1);
            }
        }

        private void da_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (lab_father != null)
            {
                moveItem(lab_father.tvi, pos + 1);
            }
            else
            {
                moveItem(expP.tvPro, pos + 1);
            }
        }

        private void btn_add_folder_Click(object sender, RoutedEventArgs e)
        {
           
        }
        
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_add_activity_Click(object sender, RoutedEventArgs e)
        {
            //LabelTreeActivity nuevo = new LabelTreeActivity(1, this, dbMP, expP);
                       
            //tvi.Items.Add(nuevo);
            //tvi.IsExpanded = true;
        }

        //MUESTRA LA INTERFAZ DE LA ACTIVIDAD SELECCIONADA
        private void tbc_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (expP.lastSelectlta != null) expP.lastSelectlta.gridSh.Visibility = Visibility.Hidden;
            expP.lastSelectlta = this;
            gridSh.Visibility = Visibility.Visible;                       

            ActivityPanel ap = new ActivityPanel(actMod,this,expP);
            expP.workplaceGrid.Children.Clear();
            expP.workplaceGrid.Children.Add(ap);
        }        
        
        private void tvi_Expanded(object sender, RoutedEventArgs e)
        {
            showOpenFolder();
        }

        private void tvi_Collapsed(object sender, RoutedEventArgs e)
        {
            showClosedFolder();
        }
        
        private void UserControl_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {   
            expP.btnBackItem.Visibility = Visibility.Visible;
            expP.lta = lab_father;
            expP.titlePro.Text = lblName.Text.ToUpper();
            expP.tvPro.Items.Clear();
            expP.carCon.getActivitiesCharacteristics(car,expP.tvPro,this,expP);
        }

        public void showOpenFolder()
        {
            folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/open-folder.png", UriKind.Relative));
        }

        public void showClosedFolder()
        {
            folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/closed-folder.png", UriKind.Relative));
        }

        public void showActivityIcon()
        {
            folder.Source = new BitmapImage(new Uri("/Resources/activity.png", UriKind.Relative)); 
        }

        public void showSelectedSh()
        {
            gridSh.Visibility = Visibility.Visible;
        }

        public void hideSelectedSh()
        {
            gridSh.Visibility = Visibility.Collapsed;
        }

        public void moveItem(TreeViewItem tvi,int p)
        {
            LabelTreeActivity lta = (LabelTreeActivity)tvi.Items.GetItemAt(p);
            
            tvi.Items.Remove(lta);            
            tvi.Items.Insert(pos, lta);

            lta.actMod.pos = pos + 1;
            actMod.pos = p + 1;

            show_arrow(lta, pos, tvi.Items.Count);
            show_arrow(this, p, tvi.Items.Count);
            

            dbMP.SaveChanges();
        }

        public void moveItem(TreeView tvi,int p)
        {
            LabelTreeActivity lta = (LabelTreeActivity)tvi.Items.GetItemAt(p);
            
            tvi.Items.Remove(lta);
            tvi.Items.Insert(pos, lta);

            lta.actMod.pos = pos + 1;
            actMod.pos = p + 1;

            show_arrow(lta, pos, tvi.Items.Count);
            show_arrow(this, p, tvi.Items.Count);

            dbMP.SaveChanges();
        }
        
        public void show_arrow(LabelTreeActivity lta, int i, int max)
        {
            lta.ua.Visibility = Visibility.Collapsed;
            lta.da.Visibility = Visibility.Collapsed;

            if (max > 1) {
                if (i == 0) { lta.da.Visibility = Visibility.Visible; lta.isFirst = true; lta.pos = i; }
                else if (i == max - 1) { lta.ua.Visibility = Visibility.Visible; lta.isLast = true; lta.pos = i; }
                else
                {
                    lta.ua.Visibility = Visibility.Visible;
                    lta.da.Visibility = Visibility.Visible;
                    lta.pos = i;
                }
            }
        }

        public int childsCount()
        {
            return tvi.Items.Count;
        }
    }
}
