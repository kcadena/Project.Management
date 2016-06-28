using MProjectWPF.UsersControls.TemplatesControls;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
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

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para FieldTemplatePanel.xaml
    /// </summary>
    public partial class FieldTemplatePanel :  System.Windows.Controls.UserControl
    {
        public FieldTemplatePanel()
        { 
            InitializeComponent();
        }

        public FieldTemplatePanel(string nameTemplate)
        {
            InitializeComponent();
            lblListFields.Content = nameTemplate;
        }

       
    }
}
