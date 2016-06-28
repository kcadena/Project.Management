using MProjectWPF.Controller;
using MProjectWPF.Model;
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
    /// Lógica de interacción para CardProject.xaml
    /// </summary>
    public partial class CardProject : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        caracteristicas car;

        public CardProject(MainWindow mw,caracteristicas c)
        {
            InitializeComponent();
            car = c;
            mainW = mw;

            Proyectos pro = new Proyectos(mainW.dbMP);
            proyectos pro1 = new proyectos();
            titleCard.Content = car.proyectos.First().proyectos_meta_datos.Where(a => a.is_title).Single().valor.ToUpper();
            leaderName.Text = car.usuarios_meta_datos.nombre + car.usuarios_meta_datos.apellido;
            descriptionCard.Text = car.proyectos.First().descripcion;            
            stage.Text = car.estado;
            percentCard.Text = "" + car.porcentaje_cumplido +"%";

            if (car.proyectos.First().icon != null)
            {
                string repositoriolocal = car.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local;
                string titlepro = "/proyectos/proyecto" + titleCard.Content.ToString().Replace(" ", "").ToLower() + "/icons/";
                string image = car.proyectos.First().icon;
                string imgSource = repositoriolocal + titlepro + image;
                logoPry.Source = new BitmapImage(new Uri(imgSource));
            }
        }

        private void enterBtnCard_Click(object sender, RoutedEventArgs e)
        {
            mainW.vp1.Visibility = Visibility.Hidden;
            ExplorerProject exPro = new ExplorerProject(mainW,car,mainW.dbMP,""+titleCard.Content);
            mainW.viewPlan.Children.Add(exPro);            
        }
    }
        
}
