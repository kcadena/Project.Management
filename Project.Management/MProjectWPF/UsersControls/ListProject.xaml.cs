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
using MProjectWPF.Controller;
using System.Windows.Media.Animation;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para ListProject.xaml
    /// </summary>
    public partial class ListProject : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        DbLitecontroller dbMP;
        CardProject cd;
        //bool banpry;        

        public ListProject(MainWindow mw,string header)
        {
            InitializeComponent();           
            mainW = mw;
            dbMP = new DbLitecontroller();
            //dbMP.buscarProyecto(listPry,mainW);
            CardProject cp = new CardProject(mainW);
            CardProject cp1 = new CardProject(mainW);
            CardProject cp2 = new CardProject(mainW);
            CardProject cp3 = new CardProject(mainW);
            CardProject cp4 = new CardProject(mainW);

            listPry.Items.Add(cp);
            listPry.Items.Add(cp1);
            listPry.Items.Add(cp2);
            listPry.Items.Add(cp3);
            listPry.Items.Add(cp4);           
            
        }
        private void image_MouseEnter(object sender, MouseEventArgs e)
        {                        
            image.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Lupa1.png")); ;
        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {   
            image.Source = new BitmapImage(new Uri("pack://application:,,/Resources/Lupa.png")); ;
        }

        private void listPry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {    
            if(cd != null) cd.enterBtnCard.Visibility = Visibility.Hidden;
            cd = (CardProject) listPry.Items.GetItemAt(listPry.SelectedIndex);
            cd.enterBtnCard.Visibility = Visibility.Visible;
        }
    }
}
