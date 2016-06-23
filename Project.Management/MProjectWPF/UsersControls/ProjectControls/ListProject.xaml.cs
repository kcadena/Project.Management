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
using MProjectWPF.Controller;
using System.Windows.Media.Animation;
using MProjectWPF.Model;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para ListProject.xaml
    /// </summary>
    public partial class ListProject : System.Windows.Controls.UserControl
    {
        MainWindow mainW;        
        CardProject cd;               

        public ListProject(MainWindow mw,usuarios usu, MProjectDeskSQLITEEntities db)
        {
            InitializeComponent();
            mainW = mw;

            List<caracteristicas> lup = usu.caracteristicas.ToList().
                Where(a => a.id_proyecto != null).ToList();

            if (lup != null)
            {
                foreach (var x in lup)
                {
                    CardProject cp = new CardProject(mainW,x,db);
                    listPry.Items.Add(cp);
                }
            }

        }

        private void image_MouseEnter(object sender, MouseEventArgs e)
        {                        
            image.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Lupa1.png"));
        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {   
            image.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Lupa.png"));
        }

        private void listPry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {    
            if(cd != null) cd.enterBtnCard.Visibility = Visibility.Hidden;
            cd = (CardProject) listPry.Items.GetItemAt(listPry.SelectedIndex);
            cd.enterBtnCard.Visibility = Visibility.Visible;
        }
    }
}