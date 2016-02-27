﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MProjectWPF.Controller.FromModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MProjectWPF.Controller.Classes
{


    class FolderTree
    {
        List<int> parent;           //parent lista (id) de todos los padres sin repetirse
        List<string> parentName;    //parent lista (Nombres) de todos los padres sin repetirse


        private List<TreeViewItem> listActivities_Model()
        {
            Activities actFol = new Activities();
            List<FromModel.ActividadesList> act = actFol.getActivitiesProject(0);
            List<long> lstParCar = actFol.getAtivitiesParents(0);

            List<TreeViewItem> lstTree = new List<TreeViewItem>();
            try
            {

                foreach (var x in act)
                {
                    TreeViewItem par = new TreeViewItem();
                    bool ban = false;
                    foreach (var l in lstParCar)
                    {
                        if (l == (long)x.id_act)
                        {
                            ban = true;
                        }
                    }
                    par.Header = itemsTree(
                           x.nombre,
                           x.fol.ToString(),
                           x.par_car.ToString(),
                           2,
                           (long)x.id_act,
                           (long)x.fol);
                    if (ban == true)
                    {
                        foreach (var y in act)
                        {
                            if (x.id_act == y.par_car)
                            {
                                TreeViewItem child = new TreeViewItem();
                                child.Header = itemsTree(
                                    y.nombre,
                                    y.fol.ToString(),
                                    y.par_car.ToString(),
                                    2,
                                    (long)y.id_act,
                                    (long)y.fol);
                                par.Items.Add(child);
                            }

                        }

                    }
                    lstTree.Add(par);
                }
            }
            catch { }


            //Organizar TVI de actividaes
            foreach (var x in lstTree)
            {
                TreeViewItem car = (TreeViewItem)x;
                StackPanel panCar = (StackPanel)car.Header;
                string id_carAct = panCar.Children.OfType<Label>().ElementAt(3).Content.ToString();

                foreach (var y in lstTree)
                {
                    TreeViewItem par = (TreeViewItem)y;
                    StackPanel pan = (StackPanel)par.Header;
                    string id_parCar = pan.Children.OfType<Label>().ElementAt(2).Content.ToString();

                    string nom = pan.Children.OfType<Label>().ElementAt(0).Content.ToString();
                    string id = pan.Children.OfType<Label>().ElementAt(3).Content.ToString();


                    if (id_parCar.Equals(id_carAct))
                    {
                        for (int i = 0; i < car.Items.Count; i++)
                        {
                            TreeViewItem q = (TreeViewItem)car.Items.GetItemAt(i);
                            StackPanel f = (StackPanel)q.Header;
                            string n = f.Children.OfType<Label>().ElementAt(0).Content.ToString();
                            string di = f.Children.OfType<Label>().ElementAt(3).Content.ToString();

                            if (nom.Equals(n) && id.Equals(di))
                            {
                                car.Items.RemoveAt(i);
                                car.Items.Insert(i, par);

                            }
                        }
                    }
                }

                //MessageBox.Show(id_par);
            }
            return lstTree;

        }

        public TreeViewItem arrange(List<Model.folder> fol)
        {
            try
            {
                //metodo de listas de ID y Nombres de los padres (folders) sin repetirse
                listsParents(fol);
                //organizar lista de folders segun padres
                List<List<Model.folder>> org = arrangeParents_folders(fol);

                ///////////////////////////////////////////////////////////////////////////
                //Lista de actividaes Actividades (folders)  ordenadas por pos
                //Activities actFol = new Activities();
                //List<List<FromModel.ActividadesList>> act = actFol.getActivitiesProject(0);
                List<TreeViewItem> act = listActivities_Model();


                //Lista de TreVieItems organizadas segun su padre
                List<TreeViewItem> lstTree = listTVI_parents(fol, org);

                ///agregar actividades
                addActivites_TVI(lstTree, act);

                //Organizar arbol de TreeNodes
                arrangeTreeNode(lstTree);

                //Retorno de TVI que contiene todo
                return lstTree.ElementAt(0);
            }
            catch (Exception err)
            {
                System.Windows.MessageBox.Show("OJO " + err.ToString());
                return new TreeViewItem();
            }
        }

        private void arrangeTreeNode(List<TreeViewItem> lstTree)
        {
            try
            {
                foreach (var x in lstTree)
                {
                    TreeViewItem par = (TreeViewItem)x;
                    StackPanel pan = (StackPanel)par.Header;
                    string id_par = pan.Children.OfType<Label>().ElementAt(2).Content.ToString();
                    foreach (var y in lstTree)
                    {
                        TreeViewItem chi = (TreeViewItem)y;
                        StackPanel ch = (StackPanel)chi.Header;
                        string id_chid = ch.Children.OfType<Label>().ElementAt(1).Content.ToString();
                        if (id_par.Equals(id_chid))
                        {
                            y.Items.Add(par);
                        }
                    }
                }
            }
            catch { }
        }

        private void listsParents(List<Model.folder> fol)
        {
            this.parent = new List<int>();
            this.parentName = new List<string>();
            try
            {
                parent.Add(Convert.ToInt16(fol.First().Parent_id_folder.ToString()));
                //agregar los padres a parent
                foreach (var x in fol)
                {

                    //false => unico  true=ya existe padre
                    bool ban = false;
                    foreach (var y in parent)
                        if (x.Parent_id_folder == y) ban = true;
                    if (ban == false)
                    {
                        parent.Add(Convert.ToUInt16(x.Parent_id_folder.ToString()));
                        parentName.Add(getFolderParent((int)x.Parent_id_folder, fol).nombre);
                    }
                }
            }
            catch { }


        }

        private List<List<Model.folder>> arrangeParents_folders(List<Model.folder> fol)
        {
            List<List<Model.folder>> org = new List<List<Model.folder>>();
            try
            {

                foreach (var x in parent)
                {
                    List<Model.folder> it_fol = new List<Model.folder>();
                    foreach (var y in fol)
                        if (x == y.Parent_id_folder) it_fol.Add(y);
                    org.Add(it_fol);
                }
            }
            catch { }
            return org;
        }

        private List<TreeViewItem> listTVI_parents(List<Model.folder> fol, List<List<Model.folder>> org)
        {
            List<TreeViewItem> lstTree = new List<TreeViewItem>();
            try
            {
                foreach (var x in org)
                {
                    TreeViewItem par = new TreeViewItem();
                    if (x.First().Parent_id_folder == 0)
                        par.Header = itemsTree(
                            "My project",
                            "0",
                            "P",
                            1,
                            -1,
                            -1);
                    else {
                        Model.folder aux = getFolderParent((int)x.First().Parent_id_folder, fol);
                        par.Header = itemsTree(
                            aux.nombre,
                            aux.id_folder.ToString(),
                            aux.Parent_id_folder.ToString(),
                            1,
                            -1,
                            -1);
                    }
                    foreach (var y in x)
                    {
                        bool ban = false;
                        foreach (var l in parent)
                        {
                            if (l == y.id_folder) ban = true;
                        }
                        if (ban == false)
                        {
                            TreeViewItem child = new TreeViewItem();
                            child.Header = itemsTree(
                                y.nombre,
                                y.id_folder.ToString(),
                                 y.Parent_id_folder.ToString(),
                                1,
                            -1,
                            -1);
                            par.Items.Add(child);
                        }
                    }
                    lstTree.Add(par);

                }
            }
            catch { }
            return lstTree;
        }

        private string name(TreeViewItem x, int pos)
        {
            TreeViewItem par = (TreeViewItem)x;
            StackPanel pan = (StackPanel)par.Header;
            return pan.Children.OfType<Label>().ElementAt(pos).Content.ToString();
        }
        private void addActivites_TVI(List<TreeViewItem> lstTree, List<TreeViewItem> act)
        {
            ///agregar actividades al arbol
            try
            {
                foreach (var y in lstTree)
                {
                    TreeViewItem par_tvi = (TreeViewItem)y;
                    string nom = name(par_tvi, 1);

                    foreach (var aa in act)
                    {
                        try
                        {
                            TreeViewItem s = (TreeViewItem)aa;
                            string fol = name(s, 1);

                            if (nom.Equals(fol))
                            {
                                par_tvi.Items.Add(s);
                            }
                        }
                        catch { }

                    }

                    foreach (var x in y.Items)
                    {
                        try
                        {
                            TreeViewItem par = (TreeViewItem)x;
                            StackPanel pan = (StackPanel)par.Header;
                            string id_fol = pan.Children.OfType<Label>().ElementAt(1).Content.ToString();
                            foreach (var aa in act)
                            {
                                TreeViewItem d = (TreeViewItem)aa;
                                StackPanel e = (StackPanel)d.Header;
                                string fol = e.Children.OfType<Label>().ElementAt(4).Content.ToString();

                                if (id_fol.Equals(fol))
                                {
                                    par.Items.Add(d);
                                }
                            }

                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        private StackPanel itemsTree(string cad, string id_fol, string parent, int op, long id_car, long fold)
        {
            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            // Label
            Label nom = new Label();
            nom.Content = cad;
            Label par = new Label();
            par.Content = parent.ToString();
            par.Visibility = System.Windows.Visibility.Hidden;
            Label id = new Label();
            id.Content = id_fol.ToString();
            id.Visibility = System.Windows.Visibility.Hidden;
            Label id_act = new Label();
            id_act.Content = id_car.ToString();
            id_act.Visibility = System.Windows.Visibility.Hidden;
            Label fol = new Label();
            fol.Content = fold.ToString();
            fol.Visibility = System.Windows.Visibility.Hidden;


            //ruta del archivo
            Label path = new Label();

            // create Image
            Image image = new Image();

            if (op == 1)
            {
                image.Source = new BitmapImage
                (new Uri("pack://application:,,/Resources/Folder-icon.png"));
                image.Width = 20;
                image.Height = 20;
            }
            else if (op == 2)
            {
                image.Source = new BitmapImage
                (new Uri("pack://application:,,/Resources/activity.ico"));
                image.Width = 20;
                image.Height = 20;
            }

            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(nom);
            stack.Children.Add(id);
            stack.Children.Add(par);
            stack.Children.Add(id_act);
            stack.Children.Add(fol);
            return stack;
        }

        private Model.folder getFolderParent(int p, List<Model.folder> fol)
        {
            foreach (var x in fol)
            {
                if (x.id_folder == p)
                    return x;
            }
            return null;
        }

    }
}
/*


     //MessageBox.Show("" + act.Count);
                /////////////////////////
                //int lo = 0;
                //foreach (var x in act)
                //{
                //    try
                //    {
                //        MessageBox.Show("pos     "+ ++lo  );
                //    }
                //    catch { }
                //    foreach (var y in x)
                //    {
                //        MessageBox.Show(y.pos + "          " + y.nombre + "         " + y.fol);
                //    }
                //}


    */
