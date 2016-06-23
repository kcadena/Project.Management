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
using MProjectWPF.Model;

namespace MProjectWPF.UsersControls.TemplatesControls.FieldsControls
{
    /// <summary>
    /// Lógica de interacción para LabelTemplate.xaml
    /// </summary>
    public partial class LabelTemplate : System.Windows.Controls.UserControl
    {
        public string nTemplate;
        public plantillas plant;

        public LabelTemplate(plantillas pla)
        {
            InitializeComponent();
            nameTemplate.Text = pla.nombre.ToUpper();
            nTemplate = pla.nombre;
            descriptionTemplate.Text = pla.descripcion;
            plant = pla;
        }

        private void btnEditTemplate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteTemplate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
