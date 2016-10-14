using ControlDB.Model;
using MProjectWPF.UsersControls.TemplatesControls;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para NewTemplatePanel.xaml
    /// </summary>
    public partial class NewTemplatePanel : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        public int posField,opcField,posItem;
        public LabelItems lit;
        public bool titleProjectValidation = false;
        public List<BoxField> listValidation;
        public ControlXml cxml;
        public int option = 0;
        public LabelTemplate lt;

        public NewTemplatePanel(MainWindow mw, ControlXml cx,bool isAccepted)
        {
            InitializeComponent();
            mainW = mw;
            cxml = cx;
            listValidation = new List<BoxField>();

            if (!isAccepted)
            {
                vTemplate.gbTemplate.Header = "TITULO PLANTILLA";
                cxml.removeAllFields();              
            }
            else
            {
                loadFields(cxml);
            }
            
            vTemplate.addNewTemp(this);
            vTemplate.addOptionTemplate(true);
            updateFields();
        }
        
        private void addField()
        {
            BoxField bf = null;
            string fName = lstField.Text;

            if (fName == "Titulo")
            {
                bf = new BoxField(fieldName.Text , 0,this,true);
                if (fieldName.Text == "") bf.labelBoxField.Content = "Titulo";                
                lstField.Items.Remove(lstField.SelectedItem);
                lstField.SelectedIndex = 0;
                titleProjectValidation = true;
                ChkRequired.IsChecked = true;
            }
            else if (fieldName.Text == "")
            {   
                fieldNameSh.Visibility = Visibility.Visible;
                fieldName.Focusable = false;
            }
            else if (fName == "Caja de Texto")
            {
                bf = new BoxField(fieldName.Text ,1,this,(bool)ChkRequired.IsChecked);                
            }
            else if (fName == "Fecha")
            {
                bf = new BoxField(fieldName.Text , 2,this,(bool)ChkRequired.IsChecked);                
            }
            else if (fName == "Lista")
            {
                bf = new BoxField(fieldName.Text, 3,this,(bool)ChkRequired.IsChecked);
                listValidation.Add(bf);
            }
            else if (fName == "Area de Texto")
            {
                bf = new BoxField(fieldName.Text , 4,this, (bool)ChkRequired.IsChecked);
            }
            if (bf != null)
            {   
                vTemplate.listBoxFields.Items.Add(bf);
                cxml.addField(fName, bf.labelBoxField.Content.ToString(), ChkRequired.IsChecked.ToString(),bf.opc);
                ChkRequired.IsChecked = false;
            }
            fieldName.Text = "";
        }

        private void editField()
        {     
            BoxField bf = null;
            string fName = elstField.Text;

            if (fName == "Titulo")
            {
                bf = new BoxField(efieldName.Text,0, this, (bool)eChkRequired.IsChecked);
                if (efieldName.Text == "") bf.labelBoxField.Content = "Titulo";
                if (lstField.Items.Count == 5) lstField.Items.RemoveAt(0);
                titleProjectValidation = true;
                if (opcField == 3) listValidation.Remove((BoxField)vTemplate.listBoxFields.Items.GetItemAt(posField));
            }
            else if (efieldName.Text == "")
            {
                efieldNameSh.Visibility = Visibility.Visible;
                efieldName.Focusable = false;
            }
            else if (fName == "Caja de Texto")
            {
                bf = new BoxField(efieldName.Text , 1, this, (bool)eChkRequired.IsChecked);
                if (lstField.Items.Count == 4 && opcField == 0)
                {
                    titleProjectValidation = false;
                    lstField.Items.Insert(0,"Titulo");
                }
                if (opcField == 3) listValidation.Remove((BoxField)vTemplate.listBoxFields.Items.GetItemAt(posField));

            }
            else if (fName == "Fecha")
            {
                bf = new BoxField(efieldName.Text , 2, this, (bool)eChkRequired.IsChecked);
                if (lstField.Items.Count == 4 && opcField == 0)
                {
                    titleProjectValidation = false;
                    lstField.Items.Insert(0, "Titulo");
                }
                if (opcField == 3) listValidation.Remove((BoxField)vTemplate.listBoxFields.Items.GetItemAt(posField));
            }
            else if (fName == "Lista")
            {
                if (opcField == 3)
                {
                    bf = (BoxField)vTemplate.listBoxFields.Items.GetItemAt(posField);
                    bf.labelBoxField.Content = efieldName.Text;
                    bf.required = (bool)eChkRequired.IsChecked;
                }
                else {
                    bf = new BoxField(efieldName.Text , 3, this, (bool)eChkRequired.IsChecked);
                    listValidation.Add(bf);
                    if (lstField.Items.Count == 4 && opcField == 0)
                    {
                        titleProjectValidation = false;
                        lstField.Items.Insert(0, "Titulo");
                    }
                }
            }
            else if (fName == "Area de Texto")
            {
                bf = new BoxField(efieldName.Text , 4, this, (bool)eChkRequired.IsChecked);
                if (lstField.Items.Count == 4 && opcField == 0)
                {
                    titleProjectValidation = false;
                    lstField.Items.Insert(0, "Titulo");
                }
                if (opcField == 3) listValidation.Remove((BoxField)vTemplate.listBoxFields.Items.GetItemAt(posField));
            }
            if (bf != null)
            {
                lstField.SelectedIndex = 0;
                vTemplate.listBoxFields.Items.RemoveAt(posField);
                vTemplate.listBoxFields.Items.Insert(posField, bf);

                cxml.editField(fName,efieldName.Text,eChkRequired.IsChecked.ToString(),posField,bf.opc);

                if (bf.comBoxField.Items.Count > 0 && bf.comBoxField.Text != "Agregar Item")
                    foreach (string item in bf.comBoxField.Items)
                        cxml.addItem(item, posField);

                gbNewField.Visibility = Visibility.Visible;
                gbEditField.Visibility = Visibility.Hidden;
                gbAddItem.Visibility = Visibility.Hidden;
            }
            efieldName.Text = "";
        }

        private void addItem()
        {
            if (itemName.Text == "")
            {   
                itemNameSh.Visibility = Visibility.Visible;
                itemName.Focusable = false;
            }
            else
            {
                LabelItems li = new LabelItems(this);
                li.lblItem.Content = itemName.Text;
                lstItem.Items.Add(li);                
                if (vTemplate.bc.comBoxField.Text == "Agregar Item")
                {
                    vTemplate.bc.comBoxField.Items.RemoveAt(0);
                    vTemplate.bc.changeComBoxField(0);
                }
                vTemplate.bc.addComBoxField(itemName.Text);
                cxml.addItem(itemName.Text,posField);
                itemName.Text = "";                                                   
            }
        }

        private void editItem()
        {
            if (itemName.Text == "")
            {
                itemNameSh.Visibility = Visibility.Visible;
                itemName.Focusable = false;
            }
            else
            {   
                lit.lblItem.Content = itemName.Text;
                vTemplate.bc.comBoxField.Items.RemoveAt(posItem);
                vTemplate.bc.comBoxField.Items.Insert(posItem,itemName.Text);
                cxml.editItem(itemName.Text, posField, posItem);
                itemName.Text = "";

                vTemplate.bc.comBoxField.SelectedIndex = 0;

                gbAddItem.Header = "AGREGAR ITEM";
                btnEditItem.Visibility = Visibility.Hidden;
                btnCancelItem.Visibility = Visibility.Hidden;
                btnAddItem.Visibility = Visibility.Visible;
            }
        }
        
        public void loadItems(string itemName)
        {   
            if (itemName != "Agregar Item")
            {
                LabelItems li = new LabelItems(this);
                li.lblItem.Content = itemName;
                lstItem.Items.Add(li);
            }
        }
         
        private void templateName_TextChanged(object sender, TextChangedEventArgs e)
        {
            vTemplate.gbTemplate.Header = templateName.Text.ToUpper();
            if (templateName.Text == "") vTemplate.gbTemplate.Header = "TITULO PLANTILLA";
        }

        private void templateNameSh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            templateNameSh.Visibility = Visibility.Hidden;
            templateName.Focus();
        }

        private void fieldNameSh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            fieldName.Text = "";
            fieldNameSh.Visibility = Visibility.Hidden;
            fieldName.Focusable = true;
            fieldName.Focus();
        }

        private void efieldNameSh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            efieldName.Text = "";
            efieldNameSh.Visibility = Visibility.Hidden;
            efieldName.Focusable = true;
            efieldName.Focus();
        }

        private void itemNameSh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            itemName.Text = "";
            itemNameSh.Visibility = Visibility.Hidden;
            itemName.Focusable = true;
            itemName.Focus();
        }

        public void getPosField(int p,int o)
        {   
            posField = p;
            opcField = o;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            gbNewField.Visibility = Visibility.Visible;
            gbEditField.Visibility = Visibility.Hidden;
            gbAddItem.Visibility = Visibility.Hidden;
        }

        private void fieldName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                addField();
            }
        }

        private void efieldName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                editField();
            }
        }

        private void itemName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (btnEditItem.Visibility == Visibility.Hidden) addItem();
                else editItem();
            }
        }

        private void lstItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gbAddItem.Header = "EDITAR ITEM";
            btnEditItem.Visibility = Visibility.Visible;
            btnCancelItem.Visibility = Visibility.Visible;
            btnAddItem.Visibility = Visibility.Hidden;

            if (lit != null) lit.removeItem.Visibility = Visibility.Hidden;

            if(lstItem.SelectedIndex >= 0)
            { 
                lit = (LabelItems)lstItem.SelectedItem;
                itemName.Text = lit.lblItem.Content.ToString();
                lit.removeItem.Visibility = Visibility.Visible;
                posItem = lstItem.SelectedIndex;
                vTemplate.bc.changeComBoxField(posItem);
            }
        }

        private void btnAddField_Click(object sender, RoutedEventArgs e)
        {
            addField();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            addItem();
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            editItem();
        }

        private void btnCancelItem_Click(object sender, RoutedEventArgs e)
        {
            gbAddItem.Header = "AGREGAR ITEM";
            btnEditItem.Visibility = Visibility.Hidden;
            btnCancelItem.Visibility = Visibility.Hidden;
            btnAddItem.Visibility = Visibility.Visible;
        }

        private void btnEditField_Click(object sender, RoutedEventArgs e)
        {
            editField();
        }
        
        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (templateName.Text == "")
            {
                MessageBox.Show("Debe Agregar un Titulo a la Plantilla");
                templateNameSh.Visibility = Visibility.Visible;
            }
            else if(!titleProjectValidation)
            {
                MessageBox.Show("Debe Agregar un Titulo al Proyecto");
            }
            else if (!verifyListValidation())
            {
                MessageBox.Show("Debe Agregar items 2 a las listas");
            }
            else
            {
                if(option == 0)  cxml.saveToDataBase(mainW, templateName.Text, detailText.Text);
                else cxml.saveToDataBase(mainW, templateName.Text, detailText.Text,lt.plant);

                cxml = new ControlXml("Logs//TemplateLog.xml");
                resetPanelTemplate();
                loadFields(cxml);
                updateFields();                
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (option == 0)
            {
                mainW.addLabels();
                cxml.verifyContent(templateName.Text, detailText.Text);
                this.Visibility = Visibility.Hidden;
                mainW.vp1.Children.Remove(this);
                mainW.vp1.Visibility = Visibility.Visible;
            }
            else
            {
                if (MessageBox.Show("Desea salir al Menu inicial ?", "Eliminar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    mainW.addLabels();
                    cxml.verifyContent(templateName.Text, detailText.Text);
                    this.Visibility = Visibility.Hidden;
                    mainW.vp1.Visibility = Visibility.Visible;
                }
                else
                {
                    resetPanelTemplate();
                    cxml = new ControlXml("Logs//TemplateLog.xml");
                    loadFields(cxml);
                }
            }
        }

        public void resetPanelTemplate()
        {
            titlePanel.Content = "CREAR PLANTILLA";

            option = 0;
            templateName.Text = "";
            detailText.Text = "";
            fieldName.Text = "";


            templateNameSh.Visibility = Visibility.Hidden;
            fieldNameSh.Visibility = Visibility.Hidden;
            efieldNameSh.Visibility = Visibility.Hidden;
            itemNameSh.Visibility = Visibility.Hidden;

            lstField.Items.Clear();
            lstField.Items.Add("Titulo");
            lstField.Items.Add("Caja de Texto");
            lstField.Items.Add("Fecha");
            lstField.Items.Add("Lista");
            lstField.Items.Add("Area de Texto");

            lstField.SelectedIndex = 0;

            vTemplate.listBoxFields.Items.Clear();

            titleProjectValidation = false;

            listValidation.Clear();

            gbNewField.Visibility = Visibility.Visible;
            gbEditField.Visibility = Visibility.Hidden;

            gbAddItem.Visibility = Visibility.Hidden;
            itemName.Text = "";

            btnAddItem.Visibility = Visibility.Visible;
            btnEditItem.Visibility = Visibility.Hidden;
            btnCancelItem.Visibility = Visibility.Hidden;
            
            if(lt != null) lt.btnDeleteTemplate.IsEnabled = true;            
        }

        public void loadFields(ControlXml cx)
        {
            cxml = cx;

            if (cxml.nameTemplate != "") vTemplate.gbTemplate.Header = cxml.nameTemplate;
            else vTemplate.gbTemplate.Header = "TITULO PLANTILLA";

            templateName.Text = cxml.nameTemplate;
            detailText.Text = cxml.detailTemplate;
            foreach (BoxField bf in cxml.loadXmlToTemplate(this)) vTemplate.listBoxFields.Items.Add(bf);
        }

        private void updateFields()
        {
            mainW.spViewA.Children.Clear();
            mainW.spViewA.Children.Add(new FieldTemplatePanel());

            mainW.spViewB.Children.Clear();
            mainW.spViewB.Children.Add(new ListTemplatePanel(mainW, this));
        }

        private bool verifyListValidation()
        {
            bool v = true;            
            foreach (var lis in listValidation)
            {
                BoxField list = lis;                
                if (!list.validationComBoxField())
                {
                    v = false;
                    list.bord.Visibility = Visibility.Visible;
                }                    
            }
            return v;
        }
    }
}
