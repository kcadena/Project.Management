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
    public partial class LabelProjectAll : UserControl
    {
        MainWindow mainW;
        string text;
        bool type;        

        public LabelProjectAll(string txt, MainWindow mw, bool ty)
        {
            InitializeComponent();
            lbl.Text = txt;
            text = txt;
            mainW = mw;
            type = ty;            
        }        
        private void lbl_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (type)
            {
                //mainW.viewPlan.Visibility = Visibility.Hidden;                
                //ExplorerProject exPro = new ExplorerProject();
                //mainW.viewPlan.Children.Add(exPro);
            }
            else
            {

            }
                        
        }

        private void lbl_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void lbl_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }
    }
}
