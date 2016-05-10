using System.Windows;
using MProjectWPF.UserControl.Inicio;
using System.Collections.Generic;
using MProjectWPF.UsersControls;
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
        bool admin = true;
        public MainWindow()
        {
            InitializeComponent();
            /*this.Visibility = Visibility.Hidden;

            LoginWindow log = new LoginWindow();
            log.Show(); */
            
            lstmen.Items.Add(new LabelProject("Nuevo Proyecto", this, 1));
            lstmen.Items.Add(new LabelProject("Abrir Proyecto", this, 2));
            lstmen.Items.Add(new LabelProject("Importar Proyecto", this, 3));

            if (admin)
            {
                lstmen.Items.Add(new LabelProject("Nueva Plantilla", this, 4));
            }           

            lstrec.Items.Add(new LabelProject("Proyectos de Ingenieria", this, 0));
            lstrec.Items.Add(new LabelProject("Desarrollo Investic ", this, 0));
            lstrec.Items.Add(new LabelProject("Acreditacion Sistemas", this, 0));
            lstrec.Items.Add(new LabelProject("Desarrollo MProject", this, 0));
            lstrec.Items.Add(new LabelProject("Aplicacion Inventario", this, 0));

            lal.Children.Add(new ListProject(this, "LISTA PROYECTOS"));

            lstUser.Items.Add(new LabelUser());
            lstUser.Items.Add(new LabelUser());
            lstUser.Items.Add(new LabelUser());
            lstUser.Items.Add(new LabelUser());
            lstUser.Items.Add(new LabelUser());

            //LoginWindow log = new LoginWindow();            
            //log.Visibility = Visibility.Visible;
           /* Dictionary<string, long> dat= new Dictionary<string, long>();
            dat["id"] = 1;
            dat["car"] = 11;
            ExplorerProject exPro = new ExplorerProject(this,dat);
            this.grid_main_window.Children.Add(exPro);*/
            
        }
    }
}
