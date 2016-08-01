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
        public List<LabelTreeActivity> child;
        public long id;
        //public long father;
        public caracteristicas car;
        public actividades actMod;
        public LabelTreeActivity lab_father;
        MProjectDeskSQLITEEntities dbMP;
        ExplorerProject expP;

        //CONSTRUCTOR CREAR ACTIVIDAD
        public LabelTreeActivity()
        {
            InitializeComponent();
            child = new List<LabelTreeActivity>();            
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
            child = new List<LabelTreeActivity>();

            if (car.id_caracteristica_padre == null)
            {
                ua.Visibility = Visibility.Hidden;
                da.Visibility = Visibility.Hidden;
            }

            if (act.folder == 0)
                showActivityIcon();
        }
       
        public LabelTreeActivity(long opc, LabelTreeActivity l_father, MProjectDeskSQLITEEntities db, ExplorerProject ep)
        {
            expP = ep;
            dbMP = db;
            InitializeComponent();
            lab_father = l_father;
            child = new List<LabelTreeActivity>();
            if (opc == 1)
                showClosedFolder();
        }        

        public void loadActivityMod(actividades act, ExplorerProject ep, LabelTreeActivity l_father)
        {   
            actMod = act;
            dbMP = ep.mainW.dbMP;
            expP = ep;
            lblName.Text = act.nombre;
            car = act.caracteristicas;
            lab_father = l_father;
            child = new List<LabelTreeActivity>();
        }

        public void addFather(LabelTreeActivity lta)
        {
            lab_father = lta;
        }

        public void addchilds(LabelTreeActivity lta)
        {
            child.Add(lta);
            show_arrow(child);
            tvi.Items.Add(lta);
        }

        private void ua_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LabelTreeActivity lup;
            int pos = (int)actMod.pos - 2;

            if (lab_father != null)
            {
                lup = lab_father.child.ElementAt(pos);

                lab_father.child.Remove(lup);
                lab_father.child.Remove(this);

                lab_father.child.Insert(pos, this);
                lab_father.child.Insert(pos + 1, lup);

            }
            else
            {
                lup = expP.child.ElementAt(pos);

                expP.child.Remove(lup);
                expP.child.Remove(this);

                expP.child.Insert(pos, this);
                expP.child.Insert(pos + 1, lup);
            }
            changeChildPosition();
        }

        private void da_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LabelTreeActivity lup;
            int pos = (int)actMod.pos;

            if(lab_father!=null)
            {
                lup = lab_father.child.ElementAt(pos);

                lab_father.child.Remove(lup);
                lab_father.child.Remove(this);

                lab_father.child.Insert(pos - 1, lup);
                lab_father.child.Insert(pos, this);
            }
            else
            {
                lup = expP.child.ElementAt(pos);

                expP.child.Remove(lup);
                expP.child.Remove(this);

                expP.child.Insert(pos - 1, lup);
                expP.child.Insert(pos, this);
            }            

            changeChildPosition();
        }

        private void btn_add_folder_Click(object sender, RoutedEventArgs e)
        {
            LabelTreeActivity nuevo = new LabelTreeActivity(0, this, dbMP, expP);
            child.Add(nuevo);
            show_arrow(child);            
            tvi.Items.Add(nuevo);
            tvi.IsExpanded = true;
        }

        private void tbcb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Actividades act = new Actividades(dbMP);
                //car = act.createActivity(lab_father.car, lab_father.child, textName.Text);
                //lblName.Text = textName.Text;
                //textName.Visibility = Visibility.Hidden;
                lblName.Visibility = Visibility.Visible;
            }
        }

        public void show_arrow(List<LabelTreeActivity> child)
        {
            if (child.Count > 1)
            {
                for (int i = 0; i < child.Count; i++)
                {
                    LabelTreeActivity c = child.ElementAt(i);
                    c.ua.Visibility = Visibility.Collapsed;
                    c.da.Visibility = Visibility.Collapsed;

                    if (i == 0) { c.da.Visibility = Visibility.Visible; }
                    else if (i == child.Count - 1) { c.ua.Visibility = Visibility.Visible; }
                    else
                    {
                        c.ua.Visibility = Visibility.Visible;
                        c.da.Visibility = Visibility.Visible;
                    }

                }
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (child.Count != 0)
            {
                if (MessageBox.Show("La Carpeta o actividad contiene sub-items Deseas Eliminarla?", "Eliminar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    deleteChild(child);
                    new Caracteristicas(dbMP).removeCharacteristics(car);
                    lab_father.child.Remove(this);
                    lab_father.tvi.Items.Remove(this);
                    changeChildPosition();
                }
            }
            else
            {
                new Caracteristicas(dbMP).removeCharacteristics(car);
                lab_father.child.Remove(this);
                lab_father.tvi.Items.Remove(this);
                changeChildPosition();
            }

            dbMP.SaveChanges();
        }

        private void btn_add_activity_Click(object sender, RoutedEventArgs e)
        {
            LabelTreeActivity nuevo = new LabelTreeActivity(1, this, dbMP, expP);
            child.Add(nuevo);            
            tvi.Items.Add(nuevo);
            tvi.IsExpanded = true;
        }

        private void changeChildPosition()
        {
            if (lab_father != null)
            {
                lab_father.tvi.Items.Clear();
                for (int i = 0; i < lab_father.child.Count; i++)
                {
                    LabelTreeActivity c = lab_father.child.ElementAt(i);
                    c.actMod.pos = i + 1;

                    if (i == 0) { c.da.Visibility = Visibility.Visible; c.ua.Visibility = Visibility.Hidden; }
                    else if (i == lab_father.child.Count - 1) { c.ua.Visibility = Visibility.Visible; c.da.Visibility = Visibility.Hidden; }
                    else {
                        c.ua.Visibility = Visibility.Visible;
                        c.da.Visibility = Visibility.Visible;
                    }
                    lab_father.tvi.Items.Add(c);
                }
            }
            else
            {
                expP.tvPro.Items.Clear();
                for (int i = 0; i < expP.child.Count; i++)
                {
                    LabelTreeActivity c = expP.child.ElementAt(i);
                    c.actMod.pos = i + 1;

                    if (i == 0) { c.da.Visibility = Visibility.Visible; c.ua.Visibility = Visibility.Hidden; }
                    else if (i == expP.child.Count - 1) { c.ua.Visibility = Visibility.Visible; c.da.Visibility = Visibility.Hidden; }
                    else
                    {
                        c.ua.Visibility = Visibility.Visible;
                        c.da.Visibility = Visibility.Visible;
                    }
                    expP.tvPro.Items.Add(c);
                }
            }

            try
            {
                dbMP.SaveChanges();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + " move");
            }
        }

        private void deleteChild(List<LabelTreeActivity> ch)
        {
            foreach (var x in ch)
            {
                deleteChild(x.child);
                new Caracteristicas(dbMP).removeCharacteristics(x.car);
            }
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
            expP.car.getActivitiesCharacteristics(car,expP.tvPro,this,expP);
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
    }
}
/*
folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/open-folder.png", UriKind.Relative));
folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/closed-folder.png", UriKind.Relative));
*/
