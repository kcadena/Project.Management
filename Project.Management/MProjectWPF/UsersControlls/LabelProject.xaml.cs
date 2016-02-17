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

namespace MProjectWPF.UsersControlls
{
    /// <summary>
    /// Lógica de interacción para lbl.xaml
    /// </summary>
    public partial class LabelProject : UserControl
    {
        MainWindow mainW;
        Storyboard myStoryboard;
        

        public LabelProject(string content, MainWindow mw)
        {
            InitializeComponent();
            lbl.Content = content;
            mainW = mw;
            myStoryboard = (Storyboard)mainW.Resources["showTemplatePanel"];
        }
        
        private void lbl_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            myStoryboard.Begin(mainW);
            mainW.viewPlan.Children.Add(new ListProject(mainW, "LISTA DE PLANTILLAS"));
        }
    }
}
