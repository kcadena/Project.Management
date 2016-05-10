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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para lbl.xaml
    /// </summary>
    public partial class LabelProject : UserControl
    {
        MainWindow mainW;
        string text;
        int type;        

        public LabelProject(string txt, MainWindow mw, int ty)
        {
            InitializeComponent();
            lbl.Text = txt;
            text = txt;
            mainW = mw;
            type = ty;            
        }        
        
        private void lbl_MouseEnter(object sender, MouseEventArgs e)
        {
            lbl.Text = "♦" + text;
        }

        private void lbl_MouseLeave(object sender, MouseEventArgs e)
        {
            lbl.Text = text;
        }

        private void lbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            mainW.viewPlan.ColumnDefinitions.Add(colDef1);
            mainW.viewPlan.ColumnDefinitions.Add(colDef2);
            mainW.viewPlan.ColumnDefinitions.Add(colDef3);*/

            mainW.vp1.Visibility = Visibility.Hidden;

            if (type == 1)
            {                
                mainW.viewPlan.Children.Add(new newProjectPanel(mainW));                            
            }
            else if (type == 2)
            {
                MessageBox.Show(text);
            }
            else if(type == 3)
            {
                MessageBox.Show(text);
            }
            else if (type == 4)
            {
                mainW.viewPlan.Children.Add(new NewTemplatePanel(mainW));
            }
            else
            {
                mainW.vp1.Visibility = Visibility.Visible;
            }
        }
    }
}
