using MProjectWPF.UsersControls.TemplatesControls;
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
    public partial class LabelProject : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        string text;
        int type;

        public LabelProject(string txt, MainWindow mw, int ty)
        {
            InitializeComponent();
            lbl.Text = "   "+txt;
            text = txt;
            mainW = mw;
            type = ty;            
        }        
        
        private void lbl_MouseEnter(object sender, MouseEventArgs e)
        {
            lbl.Text = "♦:" + text;
        }

        private void lbl_MouseLeave(object sender, MouseEventArgs e)
        {
            lbl.Text = "   "+text;
        }

        private void lbl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (type == 1)
            {
                mainW.vp1.Visibility = Visibility.Hidden;
                mainW.viewPlan.Children.Add(new newProjectPanel(mainW));                            
            }
            else if (type == 2)
            {
                mainW.vp1.Visibility = Visibility.Visible;
                MessageBox.Show(text);
            }
            else if(type == 3)
            {   
                mainW.vp1.Visibility = Visibility.Visible;
                MessageBox.Show(text);
            }
            else if (type == 4)
            {
                bool isAccepted = false;
                ControlXml cxml = new ControlXml();
                if (cxml.isSaved)
                {
                    if (MessageBox.Show("Existe un diseño de Plantilla sin guardar. \n Desea recuperar el diseño?", "Eliminar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        isAccepted = true;
                }

                mainW.vp1.Visibility = Visibility.Hidden;
                mainW.viewPlan.Children.Add(new NewTemplatePanel(mainW,cxml,isAccepted));
            }
            else
            {
                mainW.vp1.Visibility = Visibility.Visible;
            }
        }
    }
}
