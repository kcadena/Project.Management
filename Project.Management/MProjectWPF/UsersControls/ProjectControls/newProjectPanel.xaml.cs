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
        public string iconName,iconSource;
        List<BoxField> lisBF;
        Plantillas plant;
        ProjectPanel proPan;     
           
        // CONTRUCTOR  CREAR PROYECTO
        public newProjectPanel(MainWindow mw)
        {
            InitializeComponent();
            mainW = mw;
            plant = new Plantillas(mainW.dbMP);
            loadListTemplate();
            listBox.SelectedIndex = 0;           
            vTemplate.addOptionTemplate(false);
        }

        // CONSTRUCTOR ACTUALIZAR DATOS DEL PROYECTO
        public newProjectPanel(ProjectPanel proPan)
        {
            InitializeComponent();
            this.proPan = proPan;
            mainW = proPan.mainW;
            btnBack.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Visible;

            vTemplate.addOptionTemplate(false);
            LabelProject lp = new LabelProject();
            listBox.Items.Add(lp);
            lisBF = proPan.tLisBF;
            
            foreach (BoxField bf in proPan.tLisBF)
            {
                if (bf.opc == 0)
                {
                    fieldTitle = bf;
                    fieldTitle.boxField3.TextChanged += new TextChangedEventHandler(titleBoxField_TextChanged);
                }
                vTemplate.stackPanelFields.Children.Add(bf);
            }            
            projectName.Text = proPan.pName;
            detailText.Text = proPan.detail;
            labelNameProject.Content = fieldTitle.labelBoxField3.Content;

            //obtiene parametors del icono a cargar
            iconName = proPan.iconName;
            iconSource = proPan.iconSource;

            if (iconName != null)
            {   
                try
                {
                    BitmapImage b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri(iconSource);
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
            try
            {   
                if(proPan == null)
                {
                    vTemplate.stackPanelFields.Children.Clear();
                    vTemplate.gbTemplate.Header = "TITULO PROYECTO";
                    lisBF = plant.listBoxField(lblPro.pla, this);
                }
            }
            catch { }
            
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

        #region EVENTOS BOTONES
        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Archivos de imágen (.jpg)|*.jpg|All Files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.Multiselect = false;
            bool? checarOK = openFile.ShowDialog();

            if (checarOK == true)
            {
                iconSource = openFile.FileName;
                iconName = openFile.SafeFileName;

                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(iconSource);
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.EndInit();
                iconProject.Source = b;
                b = null;
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

                foreach (BoxField ibf in lisBF)
                {
                    BoxField bf = new BoxField(ibf);
                    lbf.Add(bf);
                }

                ExplorerProject exPro = null;

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic["pName"] = pName;
                dic["iconSource"] = iconSource;
                dic["iconName"] = iconName;
                dic["detail"] = detailText.Text;

                if (proPan == null)
                {
                    exPro = new ExplorerProject(mainW, lbf, lisBF, dic);
                }
                else
                {
                    proPan.exPro.UpdateTitle(dic);
                    proPan.updateTemplateProject(lbf,lisBF,dic);
                    exPro = proPan.exPro;
                }

                mainW.viewPlan.Children.Add(exPro);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            exit();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            vTemplate.stackPanelFields.Children.Clear();
            mainW.viewPlan.Children.Add(proPan.exPro);

        }
        #endregion

        private void projectName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (fieldTitle != null)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void exit()
        {
            mainW.addLabels();                        
            Visibility = Visibility.Collapsed;                       
            mainW.vp1.Visibility = Visibility.Visible;            
        }
    }
}
