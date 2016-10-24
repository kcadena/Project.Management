using MProjectWPF.Controller;
using MProjectWPF.DocumentXml;
using ControlDB.Model;
using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using MProjectWPF.UsersControls.ProjectControls.WindowsControls;
using MProjectWPF.UsersControls.TemplatesControls;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace MProjectWPF.UsersControls.ProjectControls
{
    /// <summary>
    /// Lógica de interacción para ProjectPanel.xaml
    /// </summary>
    public partial class ProjectPanel : System.Windows.Controls.UserControl
    {
        public proyectos proMod;
        Proyectos proControl;
        public MainWindow mainW;
        public ExplorerProject exPro;
        public List<BoxField> lisBF, tLisBF;
        public List<List<string>> lstRes = new List<List<string>>();
        public List<List<string>> lstEst = new List<List<string>>();
        public List<List<string>> lstCos = new List<List<string>>();
        ResourcesWindow rw;
        EstimationWindow ew, co;
        public bool bres = true, best = true, bcost = true;
        public bool enableEditProject = false;
        public double valueEstimations = 0;

        public string idName, pName, fileSource, detail, iconName ,iconSource;
        public WordClass wc;

        //CARGAR
        public ProjectPanel(proyectos proMod, MainWindow mw, ExplorerProject exPro)
        {
            InitializeComponent();
            this.proMod = proMod;
            this.exPro = exPro;
            mainW = mw;
            pName = proMod.nombre;
            idName = proMod.keym + "-" + proMod.id_proyecto + "-" + proMod.id_usuario;
            iconName = proMod.icon;
            detail = proMod.descripcion;

            string repositoriolocal = proMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local;
            string titlepro = "/proyectos/" + idName + "/icons/";
            iconSource = repositoriolocal + titlepro + proMod.icon;
            fileSource = proMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local;

            proControl = new Proyectos(mainW.dbMP);
            lisBF = proControl.loadTemplate(proMod, this);
            btnAddActivity.Visibility = Visibility.Visible;
            btnAddProject.Visibility = Visibility.Collapsed;
            loadlistTemplate();
            loadResources();
            loadEstimations();
            loadCost();
            initialDate.Text = "" + proMod.caracteristicas.fecha_inicio;
            finalDate.Text = "" + proMod.caracteristicas.fecha_fin;
            txtResourses.Text = proMod.caracteristicas.recursos;
            txtEstimation.Text = proMod.caracteristicas.presupuesto;
            valueEstimations = Convert.ToDouble(proMod.caracteristicas.presupuesto);
            txtCost.Text = proMod.caracteristicas.costos;

            int con = 0;
            foreach (var cas in stageBox.Items)
            {
                string str = cas.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                if (str == proMod.caracteristicas.estado)
                {
                    stageBox.SelectedIndex = con;
                    break;
                }
                con++;
            }
            wc = new WordClass(this, fileSource);
        }

        //CREAR
        public ProjectPanel(MainWindow mw, ExplorerProject exPro, List<BoxField> bf, List<BoxField> tbf, Dictionary<string,string> dic)
        {
            InitializeComponent();
            
            pName = dic["pName"];
            iconSource = dic["iconSource"];
            iconName = dic["iconName"];
            detail = dic["detail"];

            lisBF = bf;
            tLisBF = tbf;
            
            this.exPro = exPro;
            mainW = mw;
            wc = new WordClass(this);
            proControl = new Proyectos(mainW.dbMP);
            enableEditProject = true;
            proControl.loadTemplate(lisBF, this);
            btnEditTemplate.Visibility = Visibility.Visible;
            initialDateSh.Visibility = Visibility.Collapsed;
            finalDateSh.Visibility = Visibility.Collapsed;
            btnEditProject.Visibility = Visibility.Collapsed;
            stageBoxSh.Visibility = Visibility.Collapsed;
        }

        //ACTUALIZAR 
        public void updateTemplateProject(List<BoxField> bf, List<BoxField> tbf, Dictionary<string, string> dic)
        {
            InitializeComponent();
            
            pName = dic["pName"];
            iconSource = dic["iconSource"];
            iconName = dic["iconName"];
            detail = dic["detail"];
            
            lisBF = bf;
            tLisBF = tbf;
            proControl = new Proyectos(mainW.dbMP);

            templatePanel.Children.Clear();
            proControl.loadTemplate(lisBF, this);
        }

        public void loadlistTemplate()
        {
            string routelocal = proMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local;
            string namexml = proMod.plantilla;
            string route = routelocal + "\\proyectos\\" + idName + "\\plantilla\\" + namexml;
            ControlXml cxml = new ControlXml(route);
            tLisBF = new List<BoxField>();
            cxml.loadListBoxField(tLisBF);
        }

        private void loadResources()
        {
            foreach (recursos rec in proMod.caracteristicas.recursoslist)
            {
                List<string> lres = new List<string>();
                lres.Add(rec.nombre_recurso);
                lres.Add("" + rec.cantidad);
                lstRes.Add(lres);
            }
        }

        private void loadEstimations()
        {
            foreach (presupuesto pre in proMod.caracteristicas.presupuestolist)
            {
                List<string> lpre = new List<string>();
                lpre.Add(pre.nombre);
                lpre.Add("" + pre.cantidad);
                lpre.Add("" + pre.valor);
                lpre.Add("" + (pre.cantidad * pre.valor));
                lstEst.Add(lpre);
            }
        }

        private void loadCost()
        {
            foreach (costos cos in proMod.caracteristicas.costoslist)
            {
                List<string> lcost = new List<string>();
                lcost.Add(cos.nombre);
                lcost.Add("" + cos.cantidad);
                lcost.Add("" + cos.valor);
                lcost.Add("" + (cos.cantidad * cos.valor));
                lstCos.Add(lcost);
            }
        }

        private void assingDocument_Click(object sender, RoutedEventArgs e)
        {
            wc.openWord();
        }

        //CRUD PROYECTOS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++           
        //GUARDAR PROYECTO EN LA BASE DE DATOS 
        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            Proyectos proControl = new Proyectos(mainW.dbMP);
            ControlXml cxml = new ControlXml("Logs//TemplateTemp.xml");

            if (proControl.saveProject(this, cxml))
            {
                idName = proMod.keym + "-" + proMod.id_proyecto + "-" + proMod.id_usuario;
                caracteristicas car = proMod.caracteristicas;
                string idCar = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;

                LogXml logUpdate = new LogXml(proMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local +  "/Log/log.xml");
                logUpdate.addField(idCar,"addPro");

                if (iconName != null)
                {
                    string rutaDestino = mainW.usuModel.repositorios_usuarios.ruta_repositorio_local + "/proyectos/" + idName + "/icons/";

                    string archivoDestino = Path.Combine(rutaDestino, iconName);

                    if (!Directory.Exists(rutaDestino))
                    {
                        Directory.CreateDirectory(rutaDestino);
                    }
                    try
                    {
                        File.Copy(iconSource, archivoDestino, true);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
                string ruta = mainW.usuModel.repositorios_usuarios.ruta_repositorio_local + "/proyectos/" + idName + "/plantilla/";
                string source = "Logs\\TemplateTemp.xml";
                string template = "plantilla" + idName + ".xml";


                copyFile(ruta, template, source);

                ruta = mainW.usuModel.repositorios_usuarios.ruta_repositorio_local + "/proyectos/" + idName + "/documentos/";
                source = "DocumentXml\\DocTemplate\\Objectives.docx";
                template = "objetivos" + idName + ".docx";

                copyFile(ruta, template, source);

                btnAddProject.Visibility = Visibility.Collapsed;
                btnUpdateProject.Visibility = Visibility.Collapsed;
                btnEditTemplate.Visibility = Visibility.Collapsed;
                btnCancelProject.Visibility = Visibility.Collapsed;
                btnEditProject.Visibility = Visibility.Visible;
                btnAddActivity.Visibility = Visibility.Visible;
                initialDateSh.Visibility = Visibility.Visible;
                finalDateSh.Visibility = Visibility.Visible;
                stageBoxSh.Visibility = Visibility.Visible;
                btnDeleteProject.Visibility = Visibility.Visible;
                enableEditProject = false;

                MessageBox.Show("Proyecto guardado exitosamente!!");
            }
        }

        // ACTUALIZAR LA INFORMACION DE UN PROYECTO
        private void btnUpdateProject_Click(object sender, RoutedEventArgs e)
        {
            Proyectos proControl = new Proyectos(mainW.dbMP);
            ControlXml cxml = new ControlXml("Logs//TemplateTemp.xml");

            string rutaLocal = mainW.usuModel.repositorios_usuarios.ruta_repositorio_local;
            string folderName = "/proyectos/" + idName;
            string ruta = rutaLocal + folderName;

            string lastname = proMod.nombre;
            string lastimg = proMod.icon;

            if (proControl.updateProject(this, cxml))
            {
                caracteristicas car = proMod.caracteristicas;
                string idCar = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;

                LogXml logUpdate = new LogXml(proMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local + "/Log/log.xml");
                logUpdate.addField(idCar, "updPro");


                //COPIAR PLANTILLA NUEVA
                string rutaplan = ruta + "/plantilla/";
                string source = "Logs\\TemplateTemp.xml";
                string template = "plantilla" + idName + ".xml";

                string templateDirection = Path.Combine(rutaplan, template);

                File.Copy(source, templateDirection, true);

                //COPIAR PLANTILLA ICONO
                if (lastimg != iconName)
                {
                    string rutaDestino = ruta + "/icons/";
                    deleteDirectory(rutaDestino);
                    string archivoDestino = Path.Combine(rutaDestino, iconName);

                    if (!Directory.Exists(rutaDestino))
                    {
                        Directory.CreateDirectory(rutaDestino);
                    }

                    try
                    {
                        File.Copy(iconSource, archivoDestino, true);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }

                btnUpdateProject.Visibility = Visibility.Collapsed;
                btnEditTemplate.Visibility = Visibility.Collapsed;
                btnCancelProject.Visibility = Visibility.Collapsed;
                btnEditProject.Visibility = Visibility.Visible;
                btnAddActivity.Visibility = Visibility.Visible;
                initialDateSh.Visibility = Visibility.Visible;
                finalDateSh.Visibility = Visibility.Visible;
                stageBoxSh.Visibility = Visibility.Visible;
                btnDeleteProject.Visibility = Visibility.Visible;
                enableEditProject = false;
                MessageBox.Show("Proyecto Actualizado");
            }
        }

        //ELIMINAR PROYECTO 
        private void btnDeleteProject_Click(object sender, RoutedEventArgs e)
        {
            string route = proMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local + "\\proyectos\\";
            string name = proMod.keym + "-" + proMod.id_proyecto + "-" + proMod.id_usuario;
            string finalroute = route + name;

            caracteristicas car = proMod.caracteristicas;
            string idCar = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
            string dir = proMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local + "/Log/log.xml";

            if (proControl.deleteProject(proMod))
            {

                if (MessageBox.Show("Desea ELIMINAR la Informacion de la NUBE la proxima vez que Actualice?", "Eliminar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    if (MessageBox.Show("La informacion en la nube se Perdera, Esta seguro de eliminar?", "Eliminar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        LogXml logUpdate = new LogXml(dir);
                        logUpdate.addField(idCar, "delPro");
                    }   
                }
                

                mainW.addLabels();
                exPro.Visibility = Visibility.Collapsed;
                mainW.vp1.Children.Remove(exPro);
                mainW.vp1.Visibility = Visibility.Visible;

                if (MessageBox.Show("Desea Eliminar todo el contenido?. Documentos,Multimedia,Mapas\n ", "Eliminar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    deleteDirectory(finalroute);
                }
                MessageBox.Show("Proyecto eliminado con Exito!!");
            }
        } 

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //BOTON PERMITE MODIFICAR LA INFORMACION DE LA PLANTILLA
        private void btnEditTemplate_Click(object sender, RoutedEventArgs e)
        {   
            mainW.viewPlan.Children.Remove(exPro);
            mainW.viewPlan.Children.Add(new newProjectPanel(this));
        }

        private void webWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void webMultimedia_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditProject_Click(object sender, RoutedEventArgs e)
        {   
            btnAddActivity.Visibility = Visibility.Collapsed;
            btnEditProject.Visibility = Visibility.Collapsed;            
            initialDateSh.Visibility = Visibility.Collapsed;
            finalDateSh.Visibility = Visibility.Collapsed;
            btnDeleteProject.Visibility = Visibility.Collapsed;
            btnCancelProject.Visibility = Visibility.Visible;
            btnUpdateProject.Visibility = Visibility.Visible;
            btnEditTemplate.Visibility = Visibility.Visible;
            stageBoxSh.Visibility = Visibility.Collapsed;
            enableEditProject = true;
        }

        private void btnResources_Click(object sender, RoutedEventArgs e)
        {
            if (bres)
            {   
                
                rw = new ResourcesWindow(this, lstRes,enableEditProject);
                rw.Show();
                bres = false;
            }
            else
            {
                rw.Activate();
            }
        }

        private void btnCancelProject_Click(object sender, RoutedEventArgs e)
        {   
            btnUpdateProject.Visibility = Visibility.Collapsed;
            btnEditTemplate.Visibility = Visibility.Collapsed;
            btnCancelProject.Visibility = Visibility.Collapsed;
            btnEditProject.Visibility = Visibility.Visible;
            btnAddActivity.Visibility = Visibility.Visible;
            initialDateSh.Visibility = Visibility.Visible;
            finalDateSh.Visibility = Visibility.Visible;
            stageBoxSh.Visibility = Visibility.Visible;
            btnDeleteProject.Visibility = Visibility.Visible;
            enableEditProject = false;
        }

        private void btnEstimation_Click(object sender, RoutedEventArgs e)
        {
            if (best)
            {
                ew = new EstimationWindow(this, lstEst, 1,enableEditProject);
                ew.Show();
                best = false;
            }
            else
            {
                ew.Activate();
            }
        }

        private void btnCost_Click(object sender, RoutedEventArgs e)
        {
            if (bcost)
            {
                co = new EstimationWindow(this, lstCos, 2,enableEditProject);
                co.Title = "Costos";
                co.Show();
                bcost = false;
            }
            else
            {
                co.Activate();
            }
        }

        //AGREGAR UNA ACTIVIDAD AL PROYECTO 
        private void btnAddActivity_Click(object sender, RoutedEventArgs e)
        {
            Transfer transfer = new Transfer(this);
            transfer.Show();
        }       

        private void deleteDirectory(string ruta)
        {
            try
            {
                wc.unlockDocument();             
                Directory.Delete(ruta, true);
            }
            catch
            {

            }
            
        }

        private void copyFile(string ruta,string name, string file)
        {
            try
            {
                string templateDirection = Path.Combine(ruta, name);

                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }
                File.Copy(file, templateDirection, true);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        
    }
}
