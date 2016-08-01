using ControlDB.Model;
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

namespace MProjectWPF.UsersControls.ProjectControls.FieldsControls
{
    /// <summary>
    /// Lógica de interacción para LabelProject.xaml
    /// </summary>
    public partial class LabelProject : System.Windows.Controls.UserControl
    {
        public string lblTitleProject;
        public plantillas pla;

        public LabelProject(plantillas p)
        {
            InitializeComponent();
            pla = p;
            nameTemplate.Text = pla.nombre.ToUpper();
            descriptionTemplate.Text = pla.descripcion;
            lblTitleProject = pla.plantillas_meta_datos.Where(x => x.meta_datos.id_tipo_dato == 0).Single().meta_datos.descripcion + ":";
        }

        public LabelProject()
        {
            InitializeComponent();
            nameTemplate.Text = "Plantilla Update";
            descriptionTemplate.Text = "Update";
        }
    }
}
