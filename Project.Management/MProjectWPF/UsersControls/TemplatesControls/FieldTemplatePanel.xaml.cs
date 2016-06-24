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

        public FieldTemplatePanel(ControlXml cx, string nameTemplate)
        {
            InitializeComponent();
            loadFields(cx,nameTemplate);
        }

        private void loadFields(ControlXml cx, string nameTemplate)
        {
            lblListFields.Content = nameTemplate;            
            foreach (BoxField bf in cx.loadXmlToTemplate()) listFields.Children.Add(bf);
        }
        
    }
}
