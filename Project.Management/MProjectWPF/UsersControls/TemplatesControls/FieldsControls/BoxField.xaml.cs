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

namespace MProjectWPF.UsersControls.TemplatesControls.FieldsControls
{
    /// <summary>
    /// Lógica de interacción para BoxField.xaml
    /// </summary>
    public partial class BoxField : System.Windows.Controls.UserControl
    {
        public int opc;
        NewTemplatePanel newTemp;
        public bool required;
        public string type,type2;
        public  plantillas_meta_datos plaMetDat;

        public BoxField(string name, int o,NewTemplatePanel nt, bool req)
        {
            InitializeComponent();
            comBoxField.SelectedIndex = 0;
            opc = o;
            g1.Visibility = Visibility.Visible;
            if (opc == 0) boxField.Visibility = Visibility.Visible;
            else if (opc == 1) boxField.Visibility = Visibility.Visible;
            else if (opc == 2) dateField.Visibility = Visibility.Visible;
            else if (opc == 3) { comBoxField.Visibility = Visibility.Visible; lbl.Visibility = Visibility.Hidden; }
            else { areaField.Visibility = Visibility.Visible; lbl2.Visibility = Visibility.Visible; }
            labelBoxField.Content = name + ":";
            required = req;
            newTemp = nt;
        }

        public BoxField(plantillas_meta_datos pmd)
        {
            InitializeComponent();
            comBoxField2.SelectedIndex = 0;
            opc = (int)pmd.meta_datos.id_tipo_dato;
            g2.Visibility = Visibility.Visible;
            if (opc == 0) boxField2.Visibility = Visibility.Visible;
            else if (opc == 1) boxField2.Visibility = Visibility.Visible;
            else if (opc == 2) dateField2.Visibility = Visibility.Visible;
            else if (opc == 3) { comBoxField2.Visibility = Visibility.Visible; lbl.Visibility = Visibility.Hidden; }
            else areaField2.Visibility = Visibility.Visible;
            labelBoxField2.Content = pmd.meta_datos.descripcion + ":";
        }

        public BoxField(proyectos_meta_datos pmd)
        {
            InitializeComponent();
            labelBoxField2.Content = pmd.tipo+":";
            type = pmd.tipo;                      
            g2.Visibility = Visibility.Visible;
            boxField2.Visibility = Visibility.Visible;
            boxField2.Text = pmd.valor;
        }

        public BoxField(BoxField bf)
        {
            InitializeComponent();
            labelBoxField2.Content = bf.labelBoxField3.Content;
            type = bf.labelBoxField3.Content.ToString();
            g2.Visibility = Visibility.Visible;
            boxField2.Visibility = Visibility.Visible;
            boxField2.Text = bf.getValueField();
        }

        public BoxField(plantillas_meta_datos pmd,int i)
        {
            InitializeComponent();
            comBoxField3.SelectedIndex = 0;
            opc = (int)pmd.meta_datos.id_tipo_dato;
            type = pmd.meta_datos.descripcion;
            g3.Visibility = Visibility.Visible;
            if (opc == 0) boxField3.Visibility = Visibility.Visible;
            else if (opc == 1) boxField3.Visibility = Visibility.Visible;
            else if (opc == 2) dateField3.Visibility = Visibility.Visible;
            else if (opc == 3) comBoxField3.Visibility = Visibility.Visible;
            else areaField3.Visibility = Visibility.Visible;
            labelBoxField3.Content = pmd.meta_datos.descripcion + ":";
            required = pmd.requerido;
             
            plaMetDat = pmd;
            type2 = plaMetDat.meta_datos.tipos_datos.descripcion;

        }

        public BoxField(int o, string desc, bool req,string ty,string value)
        {
            InitializeComponent();
            comBoxField3.SelectedIndex = 0;
            opc = o;
            type = desc;
            g3.Visibility = Visibility.Visible;
            if (opc == 0) {
                boxField3.Visibility = Visibility.Visible;
                boxField3.Text = value;
            }
            else if (opc == 1) {
                boxField3.Visibility = Visibility.Visible;
                boxField3.Text = value;
            }
            else if (opc == 2) {
                dateField3.Visibility = Visibility.Visible;
                dateField3.Text = value;
            }
            else if (opc == 3) comBoxField3.Visibility = Visibility.Visible;
            else {
                areaField3.Visibility = Visibility.Visible;
                areaField3.Text = value;
            }
            labelBoxField3.Content = desc + ":";
            required = req;
            type2 = ty;
        }

        public void addComBoxField(string txt)
        {
            if(g1.Visibility == Visibility.Visible) comBoxField.Items.Add(txt);
            else comBoxField2.Items.Add(txt);
        }
        
        public void addComBoxField(string txt,bool h)
        {
            if (g1.Visibility == Visibility.Visible) comBoxField.Items.Add(txt);
            else comBoxField3.Items.Add(txt);
        }

        public void changeComBoxField(int pos)
        {
            comBoxField.SelectedIndex = pos;
        }

        private void removeField_Click(object sender, RoutedEventArgs e)
        {
            if (opc == 0)
            {
                newTemp.lstField.Items.Insert(0, "Titulo");
                newTemp.lstField.SelectedIndex = 0;
                newTemp.titleProjectValidation = false;
            }
            newTemp.vTemplate.listBoxFields.Items.Remove(this);
            newTemp.gbNewField.Visibility = Visibility.Visible;
            newTemp.gbEditField.Visibility = Visibility.Hidden;
            newTemp.gbAddItem.Visibility = Visibility.Hidden;

            newTemp.itemName.Text = "";
            newTemp.gbAddItem.Header = "AGREGAR ITEM";
            newTemp.btnEditItem.Visibility = Visibility.Hidden;
            newTemp.btnCancelItem.Visibility = Visibility.Hidden;
            newTemp.btnAddItem.Visibility = Visibility.Visible;

            newTemp.cxml.removeField(newTemp.posField);
        }

        public bool validationComBoxField()
        {
            if (comBoxField.Items.Count > 1)
            {
                return true;
            }
            return false;
        }

        public string getValueField()
        {
            string value;
            if (opc == 0) value = boxField3.Text;
            else if (opc == 1) value = boxField3.Text;
            else if (opc == 2) value = dateField3.Text;
            else if (opc == 3) value = (string)comBoxField3.SelectedItem;
            else value = areaField3.Text;
            
            return value;
        }

        public bool validationFields()
        {
            if(getValueField()=="" && required)
            {
                if (opc != 4)
                {
                    lbl3Sh.Visibility = Visibility.Visible;                    
                }
                else
                {
                    areaField3Sh.Visibility = Visibility.Visible;
                }
                return false;
            }
            return true;
        }

        private void lbl3Sh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lbl3Sh.Visibility = Visibility.Hidden;
            boxField3.Focus();
            dateField3.Focus();
        }

        private void areaField3Sh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            areaField3Sh.Visibility = Visibility.Collapsed;
            areaField3.Focus();
        }
    }
}
