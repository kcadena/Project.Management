using System.Windows;
using MProjectWPF.Windows.Inicio;
using MProjectWPF.UsersControlls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System;
using System.Windows.Interop;

namespace MProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool admin = false;
        public MainWindow()
        {
            InitializeComponent();
            /*this.Visibility = Visibility.Hidden;

            LoginWindow log = new LoginWindow();            
            log.Visibility = Visibility.Visible;*/
            WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            IntPtr manejadorVentana = interopHelper.Handle;

            lstmen.Items.Add(new LabelProject("Nuevo Proyecto", this, true));
            lstmen.Items.Add(new LabelProject("Abrir Proyecto", this, true));
            lstmen.Items.Add(new LabelProject("Importar Proyecto", this, true));
            if (admin)
            {
                lstmen.Items.Add(new LabelProject("Nueva Plantilla", this, true));
            }
            

            lstrec.Items.Add(new LabelProject("Proyectos de Ingenieria", this, true));
            lstrec.Items.Add(new LabelProject("Desarrollo Investic ", this, true));
            lstrec.Items.Add(new LabelProject("Acreditacion Sistemas", this, true));
            lstrec.Items.Add(new LabelProject("Desarrollo MProject", this, true));
            lstrec.Items.Add(new LabelProject("Aplicacion Inventario", this, true));

            lal.Children.Add(new ListProject(this, "LISTA PROYECTOS"));                        
        }        
    }
}
