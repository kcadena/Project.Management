using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
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
    /// Lógica de interacción para viewTemplate.xaml
    /// </summary>
    public partial class viewTemplate : System.Windows.Controls.UserControl
    {
        public BoxField bc;
        NewTemplatePanel newTemp;
        int posField = -1;

        public viewTemplate()
        {
            InitializeComponent();
        }

        public void addNewTemp(NewTemplatePanel nT)
        {
            newTemp = nT;
        }

        public void addOptionTemplate(bool opt)
        {
            if (opt) scrollStackPanel.Visibility = Visibility.Hidden;
            else listBoxFields.Visibility = Visibility.Hidden;
        }

        private void listBoxFields_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxFields.SelectedIndex != posField)
            {
                posField = listBoxFields.SelectedIndex;
                if (bc != null)
                {
                    bc.removeField.Visibility = Visibility.Hidden;
                }
                if (listBoxFields.SelectedIndex >= 0)
                {
                    bc = (BoxField)listBoxFields.SelectedItem;
                    bc.bord.Visibility = Visibility.Hidden;
                    bc.removeField.Visibility = Visibility.Visible;
                    if (bc.opc == 3)
                    {   
                        newTemp.itemName.Text = "";

                        newTemp.gbAddItem.Visibility = Visibility.Visible;
                        newTemp.lstItem.Items.Clear();
                        foreach (var x in newTemp.vTemplate.bc.comBoxField.Items)
                        {
                            newTemp.loadItems(x.ToString());
                        }
                    }
                    else newTemp.gbAddItem.Visibility = Visibility.Hidden;

                    if (newTemp.titleProjectValidation == true)
                    {
                        if (newTemp.elstField.Items.Count == 5)
                        {
                            newTemp.elstField.Items.RemoveAt(0);
                            newTemp.elstField.SelectedIndex = bc.opc - 1;
                        }
                        else
                        {
                            newTemp.elstField.SelectedIndex = bc.opc - 1;
                        }
                        if (bc.opc == 0)
                        {
                            newTemp.elstField.Items.Insert(0, "Titulo");
                            newTemp.elstField.SelectedIndex = bc.opc;
                        }
                    }
                    else newTemp.elstField.SelectedIndex = bc.opc;

                    newTemp.eChkRequired.IsChecked = bc.required;

                    newTemp.btnAddItem.Visibility = Visibility.Visible;
                    newTemp.btnEditItem.Visibility = Visibility.Hidden;
                    newTemp.btnCancelItem.Visibility = Visibility.Hidden;
                    newTemp.gbAddItem.Header = "AGREGAR ITEM";

                    newTemp.gbEditField.Visibility = Visibility.Visible;
                    newTemp.gbNewField.Visibility = Visibility.Hidden;

                    newTemp.efieldName.Text = bc.labelBoxField.Content.ToString().Replace(":", "");                    
                    newTemp.getPosField(listBoxFields.SelectedIndex, bc.opc);
                }
            }

        }

    }
}
