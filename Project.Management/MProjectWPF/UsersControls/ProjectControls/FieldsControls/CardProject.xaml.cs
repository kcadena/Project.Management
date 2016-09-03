using MProjectWPF.Controller;
using ControlDB.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Threading;
using System.Windows.Threading;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para CardProject.xaml
    /// </summary>
    public partial class CardProject : System.Windows.Controls.UserControl
    {

        MainWindow mainW;
        proyectos pro;

        public CardProject(MainWindow mw,proyectos p)
        {
            InitializeComponent();
            pro = p;
            mainW = mw;

            titleCard.Content = pro.nombre.ToUpper();
            leaderName.Text = pro.caracteristicas.usuarios_meta_datos.nombre + " " + pro.caracteristicas.usuarios_meta_datos.apellido;

            descriptionCard.Text = pro.descripcion;
            stage.Text = pro.caracteristicas.estado;
            percentCard.Text = "" + pro.caracteristicas.porcentaje_cumplido + "%";

            if (pro.icon != null)
            {
                string repositoriolocal = pro.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local;                
                string titlepro = "/proyectos/proyecto" + titleCard.Content.ToString().Replace(" ", "").ToLower() + "/icons/";
                string image = pro.icon;
                string imgSource = repositoriolocal + titlepro + image;
                try
                {
                    BitmapImage b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri(imgSource);
                    b.CacheOption = BitmapCacheOption.OnLoad;
                    b.EndInit();
                    logoPry.Source = b;
                    b = null;    
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void enterBtnCard_Click(object sender, RoutedEventArgs e)
        {
            logoPry.Source = null;            
            mainW.vp1.Visibility = Visibility.Hidden;
            ExplorerProject exPro = new ExplorerProject(mainW, pro, "" + titleCard.Content);
            mainW.viewPlan.Children.Add(exPro);
        }
    }   
}
