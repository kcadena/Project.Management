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
        ExplorerProject exPro;
        public ActivityPanel actPan;
        public bool isOwn = true;
        public int pos = 0;

        //CONSTRUCTOR CREAR ACTIVIDAD
        public LabelTreeActivity(LabelTreeActivity l_father)
        {
            InitializeComponent();            
            showActivityIcon();
            lab_father = l_father;
        }        

        //CONSTRUCTOR CARGAR ACTIVIDAD
        public LabelTreeActivity(actividades act, ExplorerProject ep, LabelTreeActivity l_father)
        {
            InitializeComponent();
            actMod = act;
            dbMP = ep.mainW.dbMP;
            exPro = ep;
            lblName.Text = act.nombre;
            car = act.caracteristicas;
            lab_father = l_father;

            if (ep.mainW.usuMod.id_usuario != actMod.id_usuario)
            {
                lblChangeColor();
                isOwn = false;
            }

            if (car.id_caracteristica_padre == null)
            {
                ua.Visibility = Visibility.Hidden;
                da.Visibility = Visibility.Hidden;
            }

            if (act.folder == 0)
                showActivityIcon();
        }

        //ADICIONA LA ACTIVIDAD DEL MODELO
        public void loadActivityMod(actividades act, ExplorerProject ep)
        {
            actMod = act;
            dbMP = ep.mainW.dbMP;
            exPro = ep;
            lblName.Text = act.nombre;
            car = act.caracteristicas;
        }

        public void addchilds(LabelTreeActivity lta)
        {   
            tvi.Items.Add(lta);
        }

        public void removechilds(LabelTreeActivity lta)
        {
            tvi.Items.Remove(lta);
        }

        //CLICK FECHA ARRIBA
        private void ua_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {  
            if (lab_father != null)
            {
                moveItem(lab_father.tvi,pos-1);
            }
            else
            {
                moveItem(exPro.tvPro, pos-1);
            }
        }

        //CLICK FECHA ABAJO
        private void da_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (lab_father != null)
            {
                moveItem(lab_father.tvi, pos + 1);
            }
            else
            {
                moveItem(exPro.tvPro, pos + 1);
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            Actividades act = new Actividades(exPro.mainW.dbMP);
            if (act.removeActivity(actMod))
            {
                exPro.workplaceGrid.Children.Clear();
                try { lab_father.removechilds(this); }
                catch { exPro.tvPro.Items.Remove(this); }

                MessageBox.Show("Actividad Eliminada");
            }
        }
        
        //MUESTRA LA INTERFAZ DE LA ACTIVIDAD SELECCIONADA
        private void tbc_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (exPro.lastSelectlta != null) exPro.lastSelectlta.gridSh.Visibility = Visibility.Hidden;
            exPro.lastSelectlta = this;
            gridSh.Visibility = Visibility.Visible;                       

            actPan = new ActivityPanel(actMod,this, exPro);
            exPro.workplaceGrid.Children.Clear();
            exPro.workplaceGrid.Children.Add(actPan);
        }

        //ENTRAR EN LA ACTIVIDAD
        private void UserControl_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            exPro.btnBackItem.Visibility = Visibility.Visible;
            exPro.btnHome.Visibility = Visibility.Visible;
            exPro.ltaFather = lab_father;
            exPro.titlePro.Text = lblName.Text.ToUpper();
            exPro.tvPro.Items.Clear();
            exPro.carCon.getActivitiesCharacteristics(car, exPro.tvPro, this, exPro);
            exPro.actPanCurrent = actPan;
        }

        //VERIFICA SI MUESTRA LAS ACTIVIDADES HIJAS/////
        private void tvi_Expanded(object sender, RoutedEventArgs e)
        {
            showOpenFolder();
        }

        private void tvi_Collapsed(object sender, RoutedEventArgs e)
        {
            showClosedFolder();
        }
        //FINISH///////////////////////////////////////

        public void showOpenFolder()
        {
            if (isOwn)
                folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/open-folder.png", UriKind.Relative));
            else
                folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/closed-folder-blue.png", UriKind.Relative));
        }

        public void showClosedFolder()
        {
            if(isOwn)
                folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/closed-folder.png", UriKind.Relative));
            else
                folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/closed-folder-blue.png", UriKind.Relative));
        }

        public void showActivityIcon()
        {
            if (isOwn)
                folder.Source = new BitmapImage(new Uri("/Resources/activity.png", UriKind.Relative));
            else
                folder.Source = new BitmapImage(new Uri("/Resources/activity-blue.png", UriKind.Relative));
        }

        public void showSelectedSh()
        {
            gridSh.Visibility = Visibility.Visible;
        }

        public void hideSelectedSh()
        {
            gridSh.Visibility = Visibility.Collapsed;
        }

        //MOVER ITEM DENTRO ACTIVIDAD
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

        //MOVER ITEM DENTRO PROYECTO
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

        //MUESTRA  LAS FLECHAS DE POSICION
        public void show_arrow(LabelTreeActivity lta, int i, int max)
        {
            lta.ua.Visibility = Visibility.Collapsed;
            lta.da.Visibility = Visibility.Collapsed;


            if (max > 1 && isOwn)
            {
                if (i == 0) { lta.da.Visibility = Visibility.Visible; lta.pos = i; }
                else if (i == max - 1) { lta.ua.Visibility = Visibility.Visible; lta.pos = i; }
                else
                {
                    lta.ua.Visibility = Visibility.Visible;
                    lta.da.Visibility = Visibility.Visible;
                    lta.pos = i;
                }
            }
        }

        //VERIFICA LA CANTIDAD DE HIJOS DE LA ACTIVIDAD SELECCIONADA
        public int childsCount()
        {
            return tvi.Items.Count;
        }

        private void lblChangeColor()
        {
            var bc = new BrushConverter();
            lblName.Foreground = (Brush)bc.ConvertFrom("#FF006CA5");
            folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/closed-folder-blue.png", UriKind.Relative));            
            btn_delete.Visibility = Visibility.Collapsed;
        }
    }
}
