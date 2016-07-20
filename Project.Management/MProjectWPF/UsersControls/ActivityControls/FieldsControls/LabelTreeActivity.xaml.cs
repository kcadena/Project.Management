using MProjectWPF.Controller;
using MProjectWPF.Model;
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
        public long father;
        public caracteristicas car;
        public LabelTreeActivity lab_father;
        MProjectDeskSQLITEEntities dbMP;
        ExplorerProject expP;

        public LabelTreeActivity()
        {
            InitializeComponent();
        }

        public LabelTreeActivity(actividades act, ExplorerProject ep, LabelTreeActivity l_father)
        {
            InitializeComponent();
            expP = ep;
            lblName.Text = act.nombre;
            car = act.caracteristicas;
            lab_father = l_father;           
            child = new List<LabelTreeActivity>();

            if (car.id_caracteristica_padre != null)
            {
                father = (long)car.id_caracteristica_padre;
            }
            else
            {
                ua.Visibility = Visibility.Hidden;
                da.Visibility = Visibility.Hidden;
                father = 0;
            }

            if (act.folder == 0)
              folder.Source = new BitmapImage(new Uri("/Resources/activity.ico", UriKind.Relative));
        }
       
        public LabelTreeActivity(long opc, LabelTreeActivity l_father, MProjectDeskSQLITEEntities db, ExplorerProject ep)
        {
            expP = ep;
            dbMP = db;
            InitializeComponent();
            lab_father = l_father;
            child = new List<LabelTreeActivity>();
            if (opc == 1)
                folder.Source = new BitmapImage(new Uri("/Resources/activity.ico", UriKind.Relative));
        }        

        public void addFather(LabelTreeActivity lta)
        {
            lab_father = lta;
        }

        public void addchilds(LabelTreeActivity lta)
        {
            child.Add(lta);
            show_arrow();
            tvi.Items.Add(lta);
        }

        private void ua_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LabelTreeActivity lup;
            int pos = (int)car.actividades.First().pos - 2;


            lup = lab_father.child.ElementAt(pos);

            lab_father.child.Remove(lup);
            lab_father.child.Remove(this);

            lab_father.child.Insert(pos, this);
            lab_father.child.Insert(pos + 1, lup);

            changeChildPosition(lab_father);
        }

        private void da_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LabelTreeActivity lup;
            int pos = (int)car.actividades.First().pos;

            //lup = lab_father.child.ElementAt(pos);

            //lab_father.child.Remove(lup);
            //lab_father.child.Remove(this);

            //lab_father.child.Insert(pos - 1, lup);
            //lab_father.child.Insert(pos, this);

            changeChildPosition(lab_father);
        }

        private void btn_add_folder_Click(object sender, RoutedEventArgs e)
        {
            LabelTreeActivity nuevo = new LabelTreeActivity(0, this, dbMP, expP);
            child.Add(nuevo);
            show_arrow();
            nuevo.lblName.Visibility = Visibility.Hidden;
            nuevo.textName.Visibility = Visibility.Visible;
            tvi.Items.Add(nuevo);
            tvi.IsExpanded = true;
        }

        private void tbcb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Actividades act = new Actividades(dbMP);
                car = act.createActivity(lab_father.car, lab_father.child, textName.Text);
                lblName.Text = textName.Text;
                textName.Visibility = Visibility.Hidden;
                lblName.Visibility = Visibility.Visible;
            }
        }

        private void show_arrow()
        {
            if (child.Count > 1)
            {
                for (int i = 0; i < child.Count; i++)
                {
                    LabelTreeActivity c = child.ElementAt(i);
                    if (i == 0) { c.da.Visibility = Visibility.Visible; }
                    else if (i == child.Count - 1) { c.ua.Visibility = Visibility.Visible; }
                    else {
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
                    changeChildPosition(lab_father);
                }
            }
            else
            {
                new Caracteristicas(dbMP).removeCharacteristics(car);
                lab_father.child.Remove(this);
                lab_father.tvi.Items.Remove(this);
                changeChildPosition(lab_father);
            }

            dbMP.SaveChanges();
        }

        private void btn_add_activity_Click(object sender, RoutedEventArgs e)
        {
            LabelTreeActivity nuevo = new LabelTreeActivity(1, this, dbMP, expP);
            child.Add(nuevo);
            nuevo.lblName.Visibility = Visibility.Hidden;
            nuevo.textName.Visibility = Visibility.Visible;
            tvi.Items.Add(nuevo);
            tvi.IsExpanded = true;
        }

        private void changeChildPosition(LabelTreeActivity lab_father)
        {
            lab_father.tvi.Items.Clear();
            for (int i = 0; i < lab_father.child.Count; i++)
            {
                LabelTreeActivity c = lab_father.child.ElementAt(i);

                c.car.actividades.First().pos = i + 1;

                if (i == 0) { c.da.Visibility = Visibility.Visible; c.ua.Visibility = Visibility.Hidden; }
                else if (i == lab_father.child.Count - 1) { c.ua.Visibility = Visibility.Visible; c.da.Visibility = Visibility.Hidden; }
                else {
                    c.ua.Visibility = Visibility.Visible;
                    c.da.Visibility = Visibility.Visible;
                }
                lab_father.tvi.Items.Add(c);
            }
            //dbMP.SaveChanges();
        }

        private void deleteChild(List<LabelTreeActivity> ch)
        {
            foreach (var x in ch)
            {
                deleteChild(x.child);
                new Caracteristicas(dbMP).removeCharacteristics(x.car);
            }
        }

        private void tbc_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (expP.lastSelectlta != null) expP.lastSelectlta.gridSh.Visibility = Visibility.Hidden;
            expP.lastSelectlta = this;
            gridSh.Visibility = Visibility.Visible;
            ActivityPanel ap = new ActivityPanel(car);
            expP.workplaceGrid.Children.Clear();
            expP.workplaceGrid.Children.Add(ap);
        }

        private void tvi_Expanded(object sender, RoutedEventArgs e)
        {
            folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/open-folder.png", UriKind.Relative));
        }

        private void tvi_Collapsed(object sender, RoutedEventArgs e)
        {
            folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/closed-folder.png", UriKind.Relative));
        }
        
        private void UserControl_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {   
            expP.btnBackItem.Visibility = Visibility.Visible;
            expP.lta = lab_father;
            expP.titlePro.Text = lblName.Text.ToUpper();
            expP.tvPro.Items.Clear();
            expP.car.getActivitiesCharacteristics(car,expP.tvPro,this,expP);
        }
    }
}
/*
folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/open-folder.png", UriKind.Relative));
folder.Source = new BitmapImage(new Uri("/Resources/Icons/16px/closed-folder.png", UriKind.Relative));
*/
