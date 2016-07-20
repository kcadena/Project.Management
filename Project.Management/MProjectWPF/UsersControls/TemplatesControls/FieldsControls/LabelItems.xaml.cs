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
    /// Lógica de interacción para LabelItems.xaml
    /// </summary>
    public partial class LabelItems : System.Windows.Controls.UserControl
    {
        NewTemplatePanel newTemp;

        public LabelItems(NewTemplatePanel nt)
        {
            InitializeComponent();
            newTemp = nt;
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            newTemp.cxml.removeItem(newTemp.posField, newTemp.lstItem.SelectedIndex);
            newTemp.lstItem.Items.Remove(this);
            newTemp.vTemplate.bc.comBoxField.Items.RemoveAt(newTemp.posItem);
            if (newTemp.vTemplate.bc.comBoxField.Items.Count == 0)
            {
                newTemp.vTemplate.bc.comBoxField.Items.Add("Agregar Item");
            }
            newTemp.vTemplate.bc.changeComBoxField(0);
            newTemp.itemName.Text = "";
            newTemp.gbAddItem.Header = "AGREGAR ITEM";
            newTemp.btnEditItem.Visibility = Visibility.Hidden;
            newTemp.btnCancelItem.Visibility = Visibility.Hidden;
            newTemp.btnAddItem.Visibility = Visibility.Visible;            
        }
    }
}
