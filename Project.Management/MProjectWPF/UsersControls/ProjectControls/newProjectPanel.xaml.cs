using Microsoft.Win32;
using MProjectWPF.Controller;
using ControlDB.Model;
using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using MProjectWPF.UsersControls.ProjectControls;
using MProjectWPF.UsersControls.ProjectControls.FieldsControls;
using MProjectWPF.UsersControls.TemplatesControls;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
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

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para newProjectPanel.xaml
    /// </summary>
    public partial class newProjectPanel : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        LabelProject lblPro;        
        public BoxField fieldTitle;
        string fileName,fileSource;
        List<BoxField> lisBF;
        Plantillas plant;
        ProjectPanel proPan;        

        public newProjectPanel(MainWindow mw)
        {
            InitializeComponent();
            mainW = mw;
            plant = new Plantillas(mainW.dbMP);
            loadListTemplate();
            listBox.SelectedIndex = 0;           
            vTemplate.addOptionTemplate(false);
        }

        public newProjectPanel(ProjectPanel ppn)
        {
            InitializeComponent();
            mainW = ppn.mainW;
            proPan = ppn;
            vTemplate.addOptionTemplate(false);
            LabelProject lp = new LabelProject();
            listBox.Items.Add(lp);
            lisBF = ppn.tLisBF;
            
            foreach (BoxField bf in ppn.tLisBF)
            {
                if (bf.opc == 0)
                {
                    fieldTitle = bf;
                    fieldTitle.boxField3.TextChanged += new TextChangedEventHandler(titleBoxField_TextChanged);
                }
                vTemplate.stackPanelFields.Children.Add(bf);
            }            
            projectName.Text = ppn.pName;
            detailText.Text = ppn.detail;
            labelNameProject.Content = fieldTitle.labelBoxField3.Content;
            
            if (ppn.proMod.icon != null)
            {
                string repositoriolocal = ppn.proMod.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local;
                string titlepro = "/proyectos/proyecto" + ppn.proMod.nombre.ToString().Replace(" ", "").ToLower() + "/icons/";
                string image = ppn.proMod.icon;
                string imgSource = repositoriolocal + titlepro + image;

                fileSource = repositoriolocal + titlepro;
                fileName = image;

                try
                {
                    BitmapImage b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri(imgSource);
                    b.CacheOption = BitmapCacheOption.OnLoad;
                    b.EndInit();
                    iconProject.Source = b;
                    b = null; 
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
            
        }

        private void loadListTemplate()
        {
            Plantillas plant = new Plantillas(mainW.dbMP);

            foreach (plantillas pla in plant.listTemplate())
            {
                LabelProject lp = new LabelProject(pla);
                listBox.Items.Add(lp);                
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lblPro != null) lblPro.lblCheck.Visibility = Visibility.Hidden;

            if (listBox.SelectedIndex >= 0)
            {
                lblPro = (LabelProject)listBox.SelectedItem;
                lblPro.lblCheck.Visibility = Visibility.Visible;
                labelNameProject.Content = lblPro.lblTitleProject;
                projectName.Text = "";
                detailText.Text = "";
                loadFields();
            }
        }

        public void loadFields()
        {
            vTemplate.stackPanelFields.Children.Clear();
            vTemplate.gbTemplate.Header = "TITULO PROYECTO";
            lisBF = plant.listBoxField(lblPro.pla, this);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            exit();
        }

        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Archivos de imágen (.jpg)|*.jpg|All Files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.Multiselect = false;
            bool? checarOK = openFile.ShowDialog();

            if (checarOK == true)
            {
                fileSource = openFile.FileName;
                fileName   = openFile.SafeFileName;

                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(fileSource);
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.EndInit();
                iconProject.Source = b;
                b = null;                
            }

        }

        private void projectName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (projectName.Text == "")
            {
                vTemplate.gbTemplate.Header = "TITULO DEL PROYECTO";
                fieldTitle.boxField3.Text = "";
            }
            else
            {
                vTemplate.gbTemplate.Header = projectName.Text.ToUpper();
                fieldTitle.boxField3.Text = projectName.Text;
            }
        }

        public void titleBoxField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fieldTitle.boxField3.Text == "")
            {
                vTemplate.gbTemplate.Header = "TITULO DEL PROYECTO";
                projectName.Text = "";

            }
            else
            {
                vTemplate.gbTemplate.Header = fieldTitle.boxField3.Text.ToUpper();
                projectName.Text = fieldTitle.boxField3.Text;
            }
        }

        private void btnAddProject_Click(object sender, RoutedEventArgs e)
        {
            if (fieldValidation())
            {
                iconProject.Source = null;                
                string pName = projectName.Text;
                Visibility = Visibility.Hidden;
                vTemplate.stackPanelFields.Children.Clear();                                
                mainW.viewPlan.Children.Remove(this);
                List<BoxField> lbf = new List<BoxField>();

                foreach(BoxField ibf in lisBF)
                {
                    BoxField bf = new BoxField(ibf);
                    lbf.Add(bf);
                }                

                ExplorerProject exPro;
                if(proPan == null)  exPro = new ExplorerProject(mainW, pName, lbf,lisBF, fileSource, fileName, detailText.Text);
                else exPro = new ExplorerProject(mainW,pName,proPan.proMod, lbf, lisBF, fileSource,fileName,detailText.Text);               

                mainW.viewPlan.Children.Add(exPro);
            }
        }

        private bool fieldValidation()
        {
            bool val = true;

            if (lisBF!=null)
            {
                foreach (BoxField bf in lisBF)
                {
                    if (!bf.validationFields())
                    {
                        val = false;
                    }
                }
            }
            else
            {
                val = false;
            }
            return val;
        }

        private void exit()
        {
            mainW.addLabels();                        
            Visibility = Visibility.Collapsed;                       
            mainW.vp1.Visibility = Visibility.Visible;            
        }
        
    }
}
