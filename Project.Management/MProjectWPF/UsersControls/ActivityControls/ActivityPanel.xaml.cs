using MProjectWPF.Controller;
using ControlDB.Model;
using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using MProjectWPF.UsersControls.ProjectControls;
using MProjectWPF.UsersControls.ProjectControls.WindowsControls;
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
using mpw = MProjectWebDesinger;
using ml = MultimediaLucene;


namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para ActivityPanel.xaml
    /// </summary>
    public partial class ActivityPanel : System.Windows.Controls.UserControl
    {
        caracteristicas car;
        public ProjectPanel proPanPrev;
        public ActivityPanel actPanPrev;
        public usuarios_meta_datos usuMod;
        public proyectos proMod;
        public actividades actMod;
        public LabelTreeActivity lta;
        public ExplorerProject exPro;
        public MainWindow mainW;
        public ResourcesWindow rw;
        public EstimationWindow ew, co;
        public List<List<string>> lstRes = new List<List<string>>();
        public List<List<string>> lstEst = new List<List<string>>();
        public List<List<string>> lstCos = new List<List<string>>();
        public bool bres = true, best = true, bcost = true;
        bool enableEditProject = true;
        public bool isExpanded;
        LabelUser labUser;
        public Transfer tran;

        //CONTRUCTOR CREAR ACTIVIDAD DE PROYECTO
        public ActivityPanel(ProjectPanel proPan, LabelTreeActivity lta ,Transfer tran)
        {
            InitializeComponent();
            proPanPrev = proPan;
            this.tran = tran;
            txtPA.Text = tran.txtPercentA.Text + "%";
            txtEstimation.Text = tran.txtEstimationAs.Text;

            if (Convert.ToDouble(tran.txtEstimationAs.Text) > 0)
            {
                List<string> lstRes = new List<string>();
                lstRes.Add("Asignación desde " + tran.namefather);
                lstRes.Add("1");
                lstRes.Add(tran.txtEstimationAs.Text);
                lstRes.Add(tran.txtEstimationAs.Text);
                lstEst.Add(lstRes);
            }

            car = proPan.proMod.caracteristicas;
            btnPublic.IsChecked = car.visualizar_superior;
            proMod = proPan.proMod;            
            this.lta = lta;
            exPro = proPan.exPro;
            mainW = exPro.mainW;                                
        }

        //CONTRUCTOR CREAR ACTIVIDAD DE ACTIVIDAD
        public ActivityPanel(ActivityPanel actPan, LabelTreeActivity lta, ExplorerProject exPro, Transfer tran)
        {
            InitializeComponent();
            actPanPrev = actPan;
            this.tran = tran;
            txtPA.Text = tran.txtPercentA.Text + "%";
            txtEstimation.Text = tran.txtEstimationAs.Text;

            if(Convert.ToDouble(tran.txtEstimationAs.Text) > 0)
            {   
                List<string> lstRes = new List<string>();
                lstRes.Add("Asignación desde " + tran.namefather);
                lstRes.Add("1");
                lstRes.Add(tran.txtEstimationAs.Text);
                lstRes.Add(tran.txtEstimationAs.Text);
                lstEst.Add(lstRes);
            }

            car = actPan.actMod.caracteristicas;
            actMod = actPan.actMod;
            this.lta = lta;
            this.exPro = exPro;
            mainW = exPro.mainW;
        }

        //CARGAR PANEL ACTIVIDAD
        public ActivityPanel(actividades act, LabelTreeActivity lta, ExplorerProject exPro)
        {   
            InitializeComponent();
            actMod = act;
            usuMod = act.usuarios_meta_datos;
            car = act.caracteristicas;
            this.lta = lta;
            this.exPro = exPro;
            mainW = exPro.mainW;

            txtNom.Text = actMod.nombre;
            txtPA.Text =  car.Porcentaje + "%";
            txtDesc.Text = actMod.descripcion;

            initialDate.Text = "" + car.fecha_inicio;
            finalDate.Text = "" + car.fecha_fin;

            txtResourses.Text = car.recursos;
            txtEstimation.Text = car.presupuesto;
            txtCost.Text = car.costos;
            loadResources();
            loadEstimations();
            loadCost();

            if (car.usuarios_meta_datos_asignado != null)
            {
                labUser = new LabelUser(car.usuarios_meta_datos_asignado);
                labUser.changeBackground();
                gridUser.Children.Add(labUser);
            }

            int con = 0;
            foreach (var cas in stageBox.Items)
            {
                string str = cas.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                if (str == car.estado)
                {
                    stageBox.SelectedIndex = con;
                    break;
                }
                con++;
            }

            lockFieds();
        }

        //CARGA LOS RECURSOS
        private void loadResources()
        {
            foreach (recursos rec in car.recursoslist)
            {
                List<string> lres = new List<string>();
                lres.Add(rec.nombre_recurso);
                lres.Add("" + rec.cantidad);
                lstRes.Add(lres);
            }
        }

        //CARGA LOS PRESUPUESTOS
        private void loadEstimations()
        {
            foreach (presupuesto pre in car.presupuestolist)
            {
                List<string> lpre = new List<string>();
                lpre.Add(pre.nombre);
                lpre.Add("" + pre.cantidad);
                lpre.Add("" + pre.valor);
                lpre.Add("" + (pre.cantidad * pre.valor));
                lstEst.Add(lpre);
            }
        }

        //CARGA LOS COSTOS
        private void loadCost()
        {
            foreach (costos cos in car.costoslist)
            {
                List<string> lcost = new List<string>();
                lcost.Add(cos.nombre);
                lcost.Add("" + cos.cantidad);
                lcost.Add("" + cos.valor);
                lcost.Add("" + (cos.cantidad * cos.valor));
                lstCos.Add(lcost);
            }
        }               
        
        //ABRE VENTANA DE RECURSOS
        private void btnResources_Click(object sender, RoutedEventArgs e)
        {
            if (bres)
            {

                rw = new ResourcesWindow(this, lstRes, enableEditProject);
                rw.Show();
                bres = false;
            }
            else
            {
                rw.Activate();
            }            
        }
        
        //ABRE VENTANA PRESUPUESTO
        private void btnEstimation_Click(object sender, RoutedEventArgs e)
        {
            if (best)
            {
                ew = new EstimationWindow(this, lstEst, 1, enableEditProject);
                ew.Show();
                best = false;
            }
            else
            {
                ew.Activate();
            }
        }
        
        //ABRE VENTANA COSTOS
        private void btnCost_Click(object sender, RoutedEventArgs e)
        {
            if (bcost)
            {
                co = new EstimationWindow(this, lstCos, 2, enableEditProject);
                co.Title = "Costos";
                co.Show();
                bcost = false;
            }
            else
            {
                co.Activate();
            }
        }        

        //GUARDAR EN LA BASE DE DATOS LA ACTIVIDAD
        private void btnAddActivity_Click(object sender, RoutedEventArgs e)
        {
            if (validateFields())
            {
                Actividades act = new Actividades(mainW.dbMP);
                if (act.createActivity(this))
                {
                    caracteristicas car = actMod.caracteristicas;
                    string idCar = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;

                    LogXml logUpdate = new LogXml(actMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local + "/Log/log.xml");
                    logUpdate.addField(idCar, "addAct");

                    exPro.tvProSh.Visibility = Visibility.Collapsed;
                    exPro.lastSelectlta = lta;
                    lta.loadActivityMod(actMod,exPro);
                    lockFieds();

                    MessageBox.Show("Actividad creada");
                }
            }
        }

        //ACTUALIZA EN LA BASE DE DATOS LA ACTIVIDAD
        private void btnUpdateActivity_Click(object sender, RoutedEventArgs e)
        {
            if (validateFields())
            {
                Actividades act = new Actividades(mainW.dbMP);
                if (act.editActivity(this))
                {
                    caracteristicas car = actMod.caracteristicas;
                    string idCar = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;

                    LogXml logUpdate = new LogXml(actMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local + "/Log/log.xml");
                    logUpdate.addField(idCar, "updAct");

                    exPro.tvProSh.Visibility = Visibility.Collapsed;                    
                    lockFieds();

                    MessageBox.Show("Actividad actualizada");
                }
            }          
        }

        // PERMITE AGREGAR UNA SUBACTIVIDAD
        private void btnAddNewActivity_Click(object sender, RoutedEventArgs e)
        {
            Transfer transfer = new Transfer(this);
            transfer.Show();
        }

        //MODIFICA LA INTERFAZ PARA ACTUALIZAR       
        private void btnEditActivity_Click(object sender, RoutedEventArgs e)
        {
            unlockFieds();
            exPro.tvProSh.Visibility = Visibility.Visible;
        }

        //ELIMINAR ACTIVIDAD SELECCIONADA
        private void btnDeleteActivity_Click(object sender, RoutedEventArgs e)
        {
            Actividades act = new Actividades(mainW.dbMP);
            caracteristicas car = actMod.caracteristicas;

            string idCar = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
            string dir = actMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local + "/Log/log.xml";

            if (act.removeActivity(actMod))
            {
                LogXml logUpdate = new LogXml(dir);
                logUpdate.addField(idCar, "delAct");

                exPro.workplaceGrid.Children.Clear();
                bool ac = true;
                if(exPro.actPanCurrent != null)
                {
                    if (exPro.actPanCurrent.actMod == lta.lab_father.actMod)
                    {
                        exPro.tvPro.Items.Remove(lta);
                        act.updatePos(exPro);
                        ac = false;
                    }
                }
                
                if(ac)
                {
                    try
                    {
                        lta.lab_father.removechilds(lta);
                        act.updatePos(lta.lab_father);
                    }
                    catch
                    {
                        exPro.tvPro.Items.Remove(lta);
                        act.updatePos(exPro);
                    }
                }
                MessageBox.Show("Actividad Eliminada");
            }
        }

        private void txtNom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNom.Text != "")
            {
                lta.lblName.Text = txtNom.Text;
            }
            else
            {
                lta.lblName.Text = "Nombre Actividad";
            }
        }

        //PERMITE CANCELAR LA CREACION DE UNA ACTIVIDAD
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {            
            exPro.tvProSh.Visibility = Visibility.Collapsed;
            lockFieds();

            if (proPanPrev != null)
            {   
                exPro.workplaceGrid.Children.Clear();
                exPro.tvPro.Items.Remove(lta);

                int max = exPro.childsCount();
                if(max > 0)
                {
                    LabelTreeActivity lup = (LabelTreeActivity)exPro.tvPro.Items.GetItemAt(max - 1);
                    lup.show_arrow(lup, lup.pos, max);
                }

                lta = null;             
                exPro.workplaceGrid.Children.Add(proPanPrev);
            }
            else if(actPanPrev != null)
            {
                bool ac = true;
                if(exPro.actPanCurrent != null)
                {   
                    if (exPro.actPanCurrent.actMod == lta.lab_father.actMod)
                    {
                        exPro.workplaceGrid.Children.Clear();
                        exPro.tvPro.Items.Remove(lta);
                        int max = exPro.childsCount();
                        if (max > 0)
                        {
                            LabelTreeActivity lup = (LabelTreeActivity)exPro.tvPro.Items.GetItemAt(max - 1);
                            lup.show_arrow(lup, lup.pos, max);
                        }
                        
                        ac = false;
                    }
                }

                if(ac)
                {
                    actPanPrev.lta.showSelectedSh();
                    actPanPrev.lta.removechilds(lta);
                    actPanPrev.lta.tvi.IsExpanded = isExpanded;
                    int max = actPanPrev.lta.childsCount();

                    if (max > 0)
                    {
                        LabelTreeActivity lup = (LabelTreeActivity)actPanPrev.lta.tvi.Items.GetItemAt(max - 1);
                        lup.show_arrow(lup, lup.pos, max);
                    }

                    if (actPanPrev.lta.childsCount() == 0) actPanPrev.lta.showActivityIcon();
                    else actPanPrev.lta.showClosedFolder();
                }
                
                lta = null;
                exPro.workplaceGrid.Children.Clear();
                exPro.workplaceGrid.Children.Add(actPanPrev);
                exPro.lastSelectlta = actPanPrev.lta;
            }
        }

        //CREA VENTANA WEB
        private void webWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mpw.MainWindow m = new mpw.MainWindow(car,mainW.dbMP);
                m.Show();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //RECIBE LOS PARAMETROS DE USUARIO        
        private void gridUser_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);
                UIElement element = (UIElement)e.Data.GetData("Object");
                labUser = (LabelUser)element;
                gridUser.Children.Add(labUser);               
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }        

        //VALIDAR LOS DATOS
        private bool validateFields()
        {
            bool f = true;
            if (txtNom.Text == "")
            {
                txtNomSh.Visibility = Visibility.Visible;
                f = false;
            }
            if (txtPA.Text == "")
            {
                txtPASh.Visibility = Visibility.Visible;
                f = false;
            }
            if (txtDesc.Text == "")
            {
                txtDescSh.Visibility = Visibility.Visible;
                f = false;
            }
            return f;
        }        
        
        //FOCALIZAR CAMPOS////
        private void txtNomSh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtNomSh.Visibility = Visibility.Collapsed;
            txtNom.Focus();
        }

        private void txtPASh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtPASh.Visibility = Visibility.Collapsed;
            txtPA.Focus();
        }       

        private void txtDescSh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtDescSh.Visibility = Visibility.Collapsed;
            txtDesc.Focus();
        }
        //FINISH///////////

        //PERMITE VER SUPERIOR 
        private void btnPublic_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                actMod.caracteristicas.visualizar_superior = true;
                mainW.dbMP.SaveChanges();
            }
            catch { }
        }

        //BLOQUEA VER SUPERIOR
        private void btnPublic_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                actMod.caracteristicas.visualizar_superior = false;
                mainW.dbMP.SaveChanges();
            }
            catch { }
        }

        private void clearUser_Click(object sender, RoutedEventArgs e)
        {
            gridUser.Children.Clear();
        }

        private void assignUser_Click(object sender, RoutedEventArgs e)
        {
            if(labUser != null)
            {
                car.usuarios_meta_datos_asignado = labUser.usuMod;
                car.visualizar_superior = true;

                long lastidpro;
                try { lastidpro = labUser.usuMod.proyectos.OrderBy(p => p.id_proyecto).Last().id_proyecto + 1; }
                catch { lastidpro = 1; }

                proyectos proasig = new proyectos();
                proasig.keym = "M1";
                proasig.id_proyecto = lastidpro;
                proasig.caracteristicas = car;
                proasig.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
                proasig.usuarios_meta_datos = labUser.usuMod;
                proasig.nombre = txtNom.Text;
                proasig.plantilla = "plantillamprojectcompany.xml";
                proasig.descripcion = "locokelvin";
                proasig.fecha_ultima_modificacion = DateTime.Now;
                mainW.dbMP.proyectos.Add(proasig);
                mainW.dbMP.SaveChanges();
            }
        }

        //ABRIR MULTIMEDIA
        private void webMultimedia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ml.MainWindow mwl = new ml.MainWindow(car,mainW.dbMP,false,mainW.usuMod);
                mwl.Show();
            }
            catch
            {

            }
        }

        //BLOQUEAR LOS CAMPOS => PARA NO EDITAR
        private void lockFieds()
        {
            txtNom.IsEnabled = false;
            txtPA.IsEnabled = false;
            txtDesc.IsEnabled = false;

            initialDateSh.Visibility = Visibility.Visible;
            finalDateSh.Visibility = Visibility.Visible;

            lblResources.Visibility = Visibility.Visible;
            lblEstimation.Visibility = Visibility.Visible;
            lblCost.Visibility = Visibility.Visible;

            stageBoxSh.Visibility = Visibility.Visible;
            enableEditProject = false;

            btnCancel.Visibility = Visibility.Collapsed;
            btnAddActivity.Visibility = Visibility.Collapsed;
            btnUpdateActivity.Visibility = Visibility.Collapsed;

            btnDeleteActivity.Visibility = Visibility.Visible;
            btnEditActivity.Visibility = Visibility.Visible;
            btnAddNewActivity.Visibility = Visibility.Visible;
        }

        //DESBLOQUEAR LOS CAMPOS => PERMITE EDITAR
        private void unlockFieds()
        {
            txtNom.IsEnabled = true;
            txtPA.IsEnabled = true;
            txtDesc.IsEnabled = true;

            initialDateSh.Visibility = Visibility.Collapsed;
            finalDateSh.Visibility = Visibility.Collapsed;            

            stageBoxSh.Visibility = Visibility.Collapsed;
            enableEditProject = true;

            btnCancel.Visibility = Visibility.Visible;
            btnUpdateActivity.Visibility = Visibility.Visible;

            btnDeleteActivity.Visibility = Visibility.Collapsed;
            btnEditActivity.Visibility = Visibility.Collapsed;
            btnAddNewActivity.Visibility = Visibility.Collapsed;
        }
    }
}
