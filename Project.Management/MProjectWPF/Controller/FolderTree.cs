using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MProjectWPF.Controller;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MProjectWPF.Controller
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
                        parentName.Add(getNameParent((int)x.Parent_id_folder, fol));
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



                //Lista de Tree Nodes
                List<TreeViewItem> lstTree = new List<TreeViewItem>();
                foreach (var x in org)
                {
                    TreeViewItem par = new TreeViewItem();
                    if (x.First().Parent_id_folder == 0)
                        par.Header = itemsTree(getNameParent((int)x.First().Parent_id_folder,fol),
                            "P",
                            "0",
                            1);
                    else
                        par.Header = itemsTree(getNameParent((int)x.First().Parent_id_folder, fol),
                            x.First().Parent_id_folder.ToString(),
                            getIdFolder((int)x.First().Parent_id_folder, x.First().nombre, fol),
                            1);
                    foreach (var y in x)
                    {
                        TreeViewItem child = new TreeViewItem();
                        child.Header = itemsTree(y.nombre,
                            x.First().Parent_id_folder.ToString(),
                            y.id_folder.ToString(),
                            1);
                        par.Items.Add(child);
                    }
                    lstTree.Add(par);
                }
                

                //Organizar arbol de TreeNodes
                foreach (var x in lstTree)
                {
                    for (int i = 0; i < x.Items.Count; i++) 
                    {
                        TreeViewItem par = (TreeViewItem)x.Items.GetItemAt(i);
                        StackPanel pan = (StackPanel)par.Header;
                        string id = pan.Children.OfType<Label>().ElementAt(1).Content.ToString();
                        foreach (var t in parent)
                        {
                            if (id.Equals(t.ToString()))
                            {
                                //System.Windows.MessageBox.Show(id+"  i  "+i);
                                try { par.Items.RemoveAt(i); } catch { }
                                par.Items.Add(searchTVI(lstTree, id));
                            }
                        }
                    }
                }

                //System.Windows.MessageBox.Show(lstTree.Count.ToString());
                //MessageBox.Show("" + sl.Count);
                //return lstTree.ElementAt(lstTree.Count - 1);
                return lstTree.ElementAt(0);
            }
            catch (Exception err) { System.Windows.MessageBox.Show(err.ToString()); }
            return null;
        }

        private StackPanel itemsTree(string cad, string parent, string id_fol, int op)
        {
            //System.Windows.MessageBox.Show("padre  " + parent+"  id  "+id_fol+" nombre "+cad);

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

        private TreeViewItem searchTVI(List<TreeViewItem> sl, string id)
        {
            List<TreeViewItem> list = new List<TreeViewItem>();
            foreach (var x in sl)
            {
                TreeViewItem tv = (TreeViewItem)x;
                StackPanel p = (StackPanel)tv.Header;
                string id_fin = p.Children.OfType<Label>().ElementAt(2).Content.ToString();
                if (id.Equals(id_fin))
                    list.Add(x);
            }

            return list.First();
        }

        private string getNameParent(int p, List<Model.folder> fol)
        {
            foreach (var x in fol)
            {
                if (x.id_folder == p) return x.nombre;
            }
            return "0";
        }
        private string getIdFolder(int par, string nom, List<Model.folder> fol)
        {
            foreach (var x in fol)
            {
                if (x.Parent_id_folder == par && x.nombre.Equals(nom)) return x.id_folder.ToString();
            }
            return "null";
        }
    }
    public class TreeNode
    {
        public TreeViewItem tvi;
        public int father;
        public string faName;
        public TreeNode(int parent, String fn, TreeViewItem tv)
        {
            father = parent;
            tvi = tv;
            faName = fn;
        }
    }
}
