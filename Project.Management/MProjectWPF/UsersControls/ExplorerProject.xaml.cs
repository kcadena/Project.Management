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

using MProjectWPF.Controller.Classes;
using System.Threading;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Interaction logic for ExplorerProject.xaml
    /// </summary>
    public partial class ExplorerProject : System.Windows.Controls.UserControl
    {
        Grid main_grid;
        Dictionary<string, long> dat;
        MProjectWPF.Controller.Classes.FolderTree treeFol;
        TextBox tx;
        Label lb;
        TreeViewItem t;
        bool xb = false;
        int ope;
        /*
        1-> Crear folders  
        2-> Renombrar  folders
        3-> renombrar Actividades 
        */

        public ExplorerProject(Grid grd, Dictionary<string, long> dat)
        {
            InitializeComponent();
            MProjectWPF.Controller.FromModel.Folders fol = new MProjectWPF.Controller.FromModel.Folders();

            ope = 0;
            
            tx = new TextBox();
            lb = new Label();

            this.dat = dat;
            treeFol = new FolderTree(dat);
            TreeViewItem tv = treeFol.arrange(fol.getStructureFolders(dat["id"]));
            tvPro.Items.Add(tv);
            main_grid = grd;

            tvPro.SelectedItemChanged += TvPro_SelectedItemChanged_CreateFolder;
        }

        private void tvPro_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(this.tvPro.SelectedItem.ToString());
            ContextMenu conMen = new ContextMenu();
            MenuItem mitNA = new MenuItem();
            MenuItem mitDA = new MenuItem();
            MenuItem mitNF = new MenuItem();
            MenuItem mitDF = new MenuItem();
            MenuItem mitRF = new MenuItem();
            MenuItem mitAR = new MenuItem();
            MenuItem mitUpAct = new MenuItem();
            MenuItem mitDownAct = new MenuItem();

            mitNA.Header = "Nueva actividad";
            mitNA.Click += MitNewAct_Click;
            mitDA.Header = "Eliminar actividad";
            mitDA.Click += MitDelAact_Click;
            mitNF.Header = "Nueva Carpeta";
            mitNF.Click += MitNewFol_Click;
            mitDF.Header = "Eliminar Carpeta";
            mitDF.Click += MitDelFol_Click;
            mitRF.Header = "Renombrar Carpeta";
            mitRF.Click += MitRenFol_Click;
            mitUpAct.Header = "Actividad Arriba";
            mitUpAct.Click += MitUpAct_Click;
            mitDownAct.Header = "Actividad Abajo";
            mitDownAct.Click += MitDownAct_Click;

            mitAR.Header = "Asignar Responsable";
            mitAR.Click += MitAssignResponsable_Click;


            Separator sp = new Separator();
            TreeViewItem tv = (TreeViewItem)tvPro.SelectedItem;
            if (tv != null)
            {
                if (!treeFol.name(tv, 2).Equals("P"))
                {
                    if (treeFol.name(tv, 3).Equals("-1"))
                    {
                        if (tv.Items.Count != 0)
                        {
                            TreeViewItem taux = (TreeViewItem)tv.Items.GetItemAt(0);
                            if (treeFol.name(taux, 3).Equals("-1"))
                                conMen.Items.Add(mitNF);
                        }
                        else conMen.Items.Add(mitNF);

                        conMen.Items.Add(mitDF);
                        conMen.Items.Add(mitRF);
                        conMen.Items.Add(sp);
                    }
                    else
                    {
                        string a = treeFol.name(tv, 1);
                        if (a.Equals(""))
                        {
                            long id = Convert.ToInt64(treeFol.name(tv, 2));
                            TreeViewItem t = treeFol.findParent((TreeViewItem)tvPro.Items.GetItemAt(0), id, 2);
                            int pos = (t.Items.Count);
                            pos = pos - 1;
                            TreeViewItem tx = (TreeViewItem)t.Items.GetItemAt(pos);
                            if (tv != (TreeViewItem)t.Items.GetItemAt(0))
                                conMen.Items.Add(mitUpAct);
                            if (tv != tx)
                                conMen.Items.Add(mitDownAct);
                        }
                        else
                        {
                            long id = Convert.ToInt64(treeFol.name(tv, 4));
                            TreeViewItem t = treeFol.findParent((TreeViewItem)tvPro.Items.GetItemAt(0), id, 1);
                            int pos = (t.Items.Count);
                            pos = pos - 1;
                            TreeViewItem tx = (TreeViewItem)t.Items.GetItemAt(pos);
                            if (tv != (TreeViewItem)t.Items.GetItemAt(0))
                                conMen.Items.Add(mitUpAct);
                            if (tv != tx)
                                conMen.Items.Add(mitDownAct);
                        }

                    }
                    
                    if (tv.Items.Count != 0)
                    {
                        TreeViewItem taux = (TreeViewItem)tv.Items.GetItemAt(0);
                        if (!treeFol.name(taux, 3).Equals("-1"))
                            conMen.Items.Add(mitNA);
                    }
                    else conMen.Items.Add(mitNA);
                    if (!treeFol.name(tv, 3).Equals("-1"))
                    {
                        conMen.Items.Add(mitDA);
                        conMen.Items.Add(mitAR);                        
                    }
                        
                    tvPro.ContextMenu = conMen;
                }
                else
                {
                    conMen.Items.Add(mitNF);
                    tvPro.ContextMenu = conMen;
                }
            }
        }

        private void MitAssignResponsable_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem tv = (TreeViewItem)tvPro.SelectedItem;
            Dictionary<string, string> dat = new Dictionary<string, string>();
            dat["car"] = treeFol.name(tv, 3);

            AssignResponsibility ar = new AssignResponsibility(dat,main_grid);
            main_grid.Children.Add(ar);
        }


        //////////////////////////////////
        ///// Option Positions with activities /////
        //////////////////////////////////

        private void MitDownAct_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem tv = (TreeViewItem)tvPro.SelectedItem;
            changePositionFunction_Activity(tv,2);
        }

        private void MitUpAct_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem tv = (TreeViewItem)tvPro.SelectedItem;
            changePositionFunction_Activity(tv,1);
        }

        private void changePositionFunction_Activity(TreeViewItem tv,int op)
        {
            long id = Convert.ToInt64(treeFol.name(tv, 2));
            // MessageBox.Show(id.ToString());
            // MessageBox.Show("  "+ treeFol.name(tv, 0) + "  -  " + treeFol.name(tv, 1) + "  -  " + treeFol.name(tv, 2) + "  -  " + treeFol.name(tv, 3) + "  -  " + treeFol.name(tv, 4));

            TreeViewItem t;
            if (id!=dat["car"])
            {
                t = treeFol.findParent((TreeViewItem)tvPro.Items.GetItemAt(0), id, 2);
            }
                
            else
            {
                id = Convert.ToInt64(treeFol.name(tv, 4));
                t = treeFol.findParent((TreeViewItem)tvPro.Items.GetItemAt(0), id, 1);
            }
            
            //MessageBox.Show("  " + treeFol.name(t, 0) + "  -  " + treeFol.name(t, 1) + "  -  " + treeFol.name(t, 2) + "  -  " + treeFol.name(t, 3) + "  -  " + treeFol.name(t, 4));
            int pos = 1;
            foreach (var x in t.Items)
            {
                if (treeFol.name((TreeViewItem)x, 3).Equals(treeFol.name(tv, 3))){
                    long fol;
                    fol = Convert.ToInt64(treeFol.name(tv, 4));
                    treeFol.changePosActivity(Convert.ToInt64(treeFol.name(tv, 2)), pos,fol, op);
                    break;
                }
                pos = pos + 1;
            }
            
            pos = pos - 1;
            StackPanel p = (StackPanel)t.Header;
            t.Items.RemoveAt(pos);
            
            if (op == 1)
            {
                pos = pos - 1;
                t.Items.Insert(pos--, tv);
            }
                
            else
            {
                pos = pos + 1;
                t.Items.Insert(pos++, tv);
            }
                
        }

        private void TvPro_SelectedItemChanged_CreateFolder(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (xb)
            {
               if(ope == 1 || ope == 2)
                {
                    tx.Visibility = Visibility.Hidden;
                    lb.Content = tx.Text;
                    tx.Text = "";
                    lb.Visibility = Visibility.Visible;
                    try
                    {
                        if(ope == 1)
                        {
                            long par = Convert.ToInt64(treeFol.name(t, 2));
                            long n = treeFol.createFolder(dat["id"],par,lb.Content.ToString());
                            if (n != -1) treeFol.changeIdFolder_TVI(n, t);
                            else MessageBox.Show("No se creo la carpeta");
                        }
                        else if (ope == 2)
                        {
                            long id = Convert.ToInt64(treeFol.name(t, 1));
                            treeFol.renameFolder(id, lb.Content.ToString());
                        }


                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.ToString());
                    }
                    tx = new TextBox();
                    lb = new Label();
                    xb = false;
                    ope = 0;
                }
                
            }
            
        }


        //////////////////////////////////
        /////Method Context menu Right Click/////
        //////////////////////////////////
        private void MitRenFol_Click(object sender, RoutedEventArgs e)
        {
            t = (TreeViewItem)tvPro.SelectedItem;
            if (treeFol.name(t, 3).Equals("-1"))
            {
                tx = treeFol.name(t);
                tx.Visibility = Visibility.Visible;
                StackPanel pan = (StackPanel)t.Header;
                lb = pan.Children.OfType<Label>().ElementAt(0);
                if (t.Focusable)
                {
                    xb = true;
                    ope = 2;

                    lb.Visibility = Visibility.Hidden;
                    tx.Text = lb.Content.ToString();                    
                }
            }
        }

        private void MitDelFol_Click(object sender, RoutedEventArgs e)
        {
            delFunction_act_fol(1);
        }

        private void MitNewFol_Click(object sender, RoutedEventArgs e)
        {
            t = (TreeViewItem)tvPro.SelectedItem;
            //t.ExpandSubtree();
            t.IsExpanded = false;  
            t.IsExpanded = true;

            TreeViewItem st = new TreeViewItem();
            st.Header = treeFol.itemsTree("","-1",treeFol.name(t,1),1,-1,-1);
            t.Items.Insert(0,st);            
            t = st;
            //t.Focus();
            t.IsSelected = true;          
            tx = treeFol.name(t);
            tx.Visibility = Visibility.Visible;
            StackPanel pan = (StackPanel)t.Header;
            lb = pan.Children.OfType<Label>().ElementAt(0);

            

            if (t.Focusable)
            {
                xb = true;
                ope = 1;                
                lb.Visibility = Visibility.Hidden;
                tx.Focus();
                //tx.Background = Brushes.MediumAquamarine;
                tx.BorderBrush = Brushes.Transparent;
                //tx.SelectionBrush = Brushes.White;
                tx.SelectedText = "New Folder";
                tx.SelectAll();  

            }


            // Finally make sure that we are
            // allowed to edit the TextBlock

        }

        private void MitDelAact_Click(object sender, RoutedEventArgs e)
        {
            delFunction_act_fol(2);
        }

        private void MitNewAct_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.MessageBox.Show(treeFol.name((TreeViewItem)this.tvPro.SelectedItem, 1));
            try
            {
                //MProjectWPF.Controller.Classes.FolderTree f = new FolderTree();
                //MessageBox.Show(f.name((TreeViewItem)tvPro.SelectedItem,3));

                AddAtivity addAct = new AddAtivity((TreeViewItem)tvPro.SelectedItem, tvPro, main_grid, dat);
                main_grid.Children.Add(addAct);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }


        }

        private void delFunction_act_fol(int op)
        {

            TreeViewItem tv = (TreeViewItem)tvPro.SelectedItem;
           
             
            if (tv.Items.Count > 0)
            {
                MessageBox.Show("No se puede eliminar esta carpeta\n" +
                    "porque existen otros elementos que dependen de esta"
                    );
            }
            else
            {
                try
                {
                    
                    if (op == 1)
                    {
                        long id = Convert.ToInt64(treeFol.name(tv, 2));
                        TreeViewItem t = treeFol.findParent((TreeViewItem)tvPro.Items.GetItemAt(0), id, op);
                        id = Convert.ToInt64(treeFol.name(tv, 1));
                        treeFol.deleteFolder(id);
                        t.Items.Remove(tv);
                    }
                    else if(op == 2)
                    {
                        TreeViewItem t;
                        long id = Convert.ToInt64(treeFol.name(tv, 2));
                        if (id != dat["car"])
                        {
                            t = treeFol.findParent((TreeViewItem)tvPro.Items.GetItemAt(0), id, 2);
                        }

                        else
                        {
                            id = Convert.ToInt64(treeFol.name(tv, 4));
                            t = treeFol.findParent((TreeViewItem)tvPro.Items.GetItemAt(0), id, 1);
                        }

                        //MessageBox.Show("" +id+"     "+ treeFol.name(t,0));
                        id = Convert.ToInt64(treeFol.name(tv, 3));
                        bool ban = treeFol.deleteActivity(id);
                        
                        if (ban) t.Items.Remove(tv);
                        else MessageBox.Show("No se puede eliminar la actividad");
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }

            }
        }

    }
}