﻿using System;
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

using MProjectWPF.Controller.FromModel;
using MProjectWPF.Controller.Classes;
using MProjectWPF.Controller;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Interaction logic for AddAtivity.xaml
    /// </summary>
    public partial class AddAtivity : System.Windows.Controls.UserControl
    {
        TreeViewItem tvi;
        TreeView tvPro;
        MainWindow mainW;
        Dictionary<string, long> dat;

        public AddAtivity(TreeViewItem tvi, TreeView tvPro,MainWindow mw,Dictionary<string,long> id)
        {
            
            InitializeComponent();
            this.tvi = tvi;
            this.tvPro = tvPro;
            mainW = mw;
            this.dat = id;
        }

        private void bntAddAct_Click(object sender, RoutedEventArgs e)
        {
            Folders fol = new Folders();
            FolderTree treFol = new FolderTree(dat);

            string idFol = treFol.name(tvi, 1);
            string id_cha = treFol.name(tvi, 3);

            Dictionary<String, string> data = new Dictionary<string, string>();
            data["nom"] = txtNom.Text;
            data["des"] = txtDes.Text;
            data["est"] = txtEst.Text;
            data["per"] = txtPer.Text;
            data["dur"] = txtDur.Text;
            data["fol"] = idFol;
            data["typDur"] = "Horas";
            data["id_pro"] = "";
            data["fat_prj"] = dat["id"].ToString();
            data["act"] = "";

            if (id_cha.Equals("-1"))
            {

                data["fat_cha"] = dat["car"].ToString();
                data["pos"] = "OK";
            }
            else {
                data["fat_cha"] = id_cha.ToString();
                data["pos"] = "NO";
            }

            Characteristics cha = new Characteristics();
            Model.actividadess act = cha.CreateCharacteristicsActivity(data);
            TreeViewItem tv = new TreeViewItem();
            ActividadesList xt = Controller.FromModel.Activities.activity_charac(act.id_actividad);
            long ns = 0;
            if (act.id_folder != null)  ns = (long) act.id_folder;
            tv.Header= treFol.itemsTree(xt.nombre, ns.ToString() , xt.par_car.ToString(), 2, (long)xt.id_act, ns);
            tvi.Items.Add(tv);
            tv.IsExpanded = true;
            tv.Focus();
            mainW.viewPlan.Children.Remove(this);
        }
    }
}
