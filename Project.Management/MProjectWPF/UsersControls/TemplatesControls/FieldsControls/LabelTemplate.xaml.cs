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
using MProjectWPF.Controller;

namespace MProjectWPF.UsersControls.TemplatesControls.FieldsControls
{
    /// <summary>
    /// Lógica de interacción para LabelTemplate.xaml
    /// </summary>
    public partial class LabelTemplate : System.Windows.Controls.UserControl
    {
        public string nTemplate;
        public plantillas plant;
        MProjectDeskSQLITEEntities dbMP;
        ListTemplatePanel lstTemPan;
        NewTemplatePanel newTemp;

        public LabelTemplate(plantillas pla, MProjectDeskSQLITEEntities db,ListTemplatePanel ltp, NewTemplatePanel ntp)
        {
            InitializeComponent();
            nameTemplate.Text = pla.nombre.ToUpper();
            nTemplate = pla.nombre;
            descriptionTemplate.Text = pla.descripcion;
            plant = pla;
            dbMP = db;
            lstTemPan = ltp;
            newTemp = ntp;
        }

        public void showButtons()
        {
            btnDeleteTemplate.Visibility = Visibility.Visible;
            btnEditTemplate.Visibility = Visibility.Visible;
        }

        public void hiddeButtons()
        {
            btnDeleteTemplate.Visibility = Visibility.Hidden;
            btnEditTemplate.Visibility = Visibility.Hidden;
        }

        private void btnEditTemplate_Click(object sender, RoutedEventArgs e)
        {
            newTemp.resetPanelTemplate();
            newTemp.lt = this;
            newTemp.titlePanel.Content = "EDITAR PLANTILLA";
            newTemp.loadFields(lstTemPan.cxml);            
            newTemp.option = 1;
            btnDeleteTemplate.IsEnabled = false;
        }

        private void btnDeleteTemplate_Click(object sender, RoutedEventArgs e)
        {
            Plantillas planc = new Plantillas(dbMP);
            planc.deleteTemplate(plant);
            lstTemPan.updateListTemplates();
        }
    }
}
