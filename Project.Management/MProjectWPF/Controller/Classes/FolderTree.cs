using System;
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
        public TreeViewItem arrange(List<Model.folder> fol)
        {
            //parent lista de todos los padres sin repetirse
            List<int> parent = new List<int>();
            List<string> parentName = new List<string>();
            try
            {
                parent.Add(Convert.ToInt16(fol.First().Parent_id_folder.ToString()));
                //metodo para agregar los padres a parent
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

                //organizar lista de folders segun padres
                List<List<Model.folder>> org = new List<List<Model.folder>>();
                foreach (var x in parent)
                {
                    List<Model.folder> it_fol = new List<Model.folder>();
                    foreach (var y in fol)
                        if (x == y.Parent_id_folder) it_fol.Add(y);
                    org.Add(it_fol);
                }


                /////////////////////////
                //Lista de actividaes Actividades (folders)  ordenadas por pos
                Activities actFol = new Activities();
                List<List<Model.actividade>> act = actFol.getActivitiesFolders();
                //MessageBox.Show("" + act.Count);
                /////////////////////////
                //int lo = 0;
                //foreach (var x in act)
                //{
                //   // MessageBox.Show("pos     " + ++lo);
                //    foreach (var y in x)
                //    {
                //        MessageBox.Show(y.id_actividad + "          " + y.nombre+"         "+y.id_folder);
                //    }
                //}
                List<TreeViewItem> lstTree = new List<TreeViewItem>();
                foreach (var x in org)
                {
                    TreeViewItem par = new TreeViewItem();
                    if (x.First().Parent_id_folder == 0)
                        par.Header = itemsTree(
                            "My project",
                            "0",
                            "P",
                            1);
                    else {
                        Model.folder aux = getFolderParent((int)x.First().Parent_id_folder, fol);
                        par.Header = itemsTree(
                            aux.nombre,
                            aux.id_folder.ToString(),
                            aux.Parent_id_folder.ToString(),
                            1);
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
                                1);
                            par.Items.Add(child);
                        }
                    }
                    lstTree.Add(par);

                }

                ///agregar actividades
                ///
               
                foreach (var x in lstTree)
                {
                    try
                    {  
                        TreeViewItem par = (TreeViewItem)x;
                        StackPanel pan = (StackPanel)par.Header;
                        string id_par = pan.Children.OfType<Label>().ElementAt(1).Content.ToString();


                        foreach (var ac in act)
                        {
                            foreach (var a in ac)
                            {
                                if (id_par.Equals(a.id_folder.ToString()))
                                {
                                    TreeViewItem child = new TreeViewItem();
                                    child.Header = itemsTree(
                                    a.nombre,
                                    a.id_folder.ToString(),
                                    "-1",
                                    2);
                                    par.Items.Add(child);

                                }
                            }
                        }
                        foreach(var y in x.Items)
                        {
                            TreeViewItem pa = (TreeViewItem)y;
                            StackPanel p = (StackPanel)pa.Header;
                            string id = p.Children.OfType<Label>().ElementAt(1).Content.ToString();
                            foreach (var ac in act)
                            {
                                foreach (var a in ac)
                                {
                                    if (id.Equals(a.id_folder.ToString()))
                                    {
                                        TreeViewItem child = new TreeViewItem();
                                        child.Header = itemsTree(
                                        a.nombre,
                                        a.id_folder.ToString(),
                                        "-1",
                                        2);
                                        pa.Items.Add(child);

                                    }
                                }
                            }
                        }

                    }
                    catch { }

                }


              
                //Organizar arbol de TreeNodes
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






                return lstTree.ElementAt(0);
            }
            catch (Exception err) { System.Windows.MessageBox.Show(err.ToString()); }
            return null;
        }

        private StackPanel itemsTree(string cad, string id_fol, string parent, int op)
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
