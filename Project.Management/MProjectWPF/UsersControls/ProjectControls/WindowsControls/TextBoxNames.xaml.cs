using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MProjectWPF.UsersControls.ProjectControls.WindowsControls
{
    /// <summary>
    /// Lógica de interacción para TextBoxNames.xaml
    /// </summary>
    public partial class TextBoxNames : System.Windows.Controls.UserControl
    {
        ResourcesWindow resWin;
        EstimationWindow estWin;
        int opc;
        bool enable;

        public TextBoxNames(int o, ResourcesWindow rw)
        {
            InitializeComponent();
            opc = o;
            resWin = rw;
            
            if (opc == 1) { txtText.Visibility = Visibility.Visible; }
            else { txtNumber.Visibility = Visibility.Visible; }
        }

        public TextBoxNames(int o, EstimationWindow ew)
        {
            InitializeComponent();
            opc = o;
            estWin = ew;
            enable = ew.enableEdit;
            if (opc == 1) { txtText.Visibility = Visibility.Visible; }
            else { txtNumber.Visibility = Visibility.Visible; }
        }

        private void validation_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            validation.Visibility = Visibility.Collapsed;
            txtNumber.Focus();
            txtText.Focus();
        }

        private void txtNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]");            
            Regex regex2 = new Regex("[0-9]+");
            Regex regex3 = new Regex("(\\,|\\.)");

            if (txtNumber.Text == "" && regex.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else if (regex2.IsMatch(txtNumber.Text) && regex.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else if (regex3.IsMatch(txtNumber.Text) && regex3.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            else if (regex2.IsMatch(txtNumber.Text) && regex3.IsMatch(e.Text))
            {
                e.Handled = false;
            }            
            else
            {
                e.Handled = true;
            }            
        }

        private void lbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (enable)
            {
                if (resWin != null)
                {
                    clickEventResource();
                }
                else
                {
                    clickEventEstimation();
                }
            }   
        }

        private void clickEventResource()
        {
            if (resWin.allowSelected)
            {
                resWin.updateResource.Visibility = Visibility.Visible;
                resWin.saveResource.Visibility = Visibility.Hidden;
                int ri;

                if (opc == 1)
                {
                    ri = resWin.resourceName.SelectedIndex;
                    resWin.resourceUnits.SelectedIndex = ri;
                }

                else
                {
                    ri = resWin.resourceUnits.SelectedIndex;
                    resWin.resourceName.SelectedIndex = ri;
                }

                resWin.selectedField = ri;

                resWin.resName = (TextBoxNames)resWin.resourceName.SelectedItem;
                resWin.resUnit = (TextBoxNames)resWin.resourceUnits.SelectedItem;

                resWin.sum = resWin.sum - Convert.ToInt32(resWin.resUnit.txtNumber.Text);

                resWin.resName.lbl.Visibility = Visibility.Collapsed;
                resWin.resUnit.lbl.Visibility = Visibility.Collapsed;
                resWin.resName.txtText.Visibility = Visibility.Visible;
                resWin.resUnit.txtNumber.Visibility = Visibility.Visible;
                resWin.allowSelected = false;
            }

        }

        private void clickEventEstimation()
        {
            if (estWin.allowSelected)
            {
                estWin.updateEstimation.Visibility = Visibility.Visible;
                estWin.saveEstimation.Visibility = Visibility.Hidden;
                int ri;

                if (opc == 1)
                {
                    ri = estWin.estimationName.SelectedIndex;
                    estWin.estimationUnits.SelectedIndex = ri;
                    estWin.estimationValueUnits.SelectedIndex = ri;
                    estWin.estimationValueTotal.SelectedIndex = ri;
                }

                else if (opc == 2)
                {
                    ri = estWin.estimationUnits.SelectedIndex;
                    estWin.estimationName.SelectedIndex = ri;
                    estWin.estimationValueUnits.SelectedIndex = ri;
                    estWin.estimationValueTotal.SelectedIndex = ri;
                }
                else if (opc == 3)
                {
                    ri = estWin.estimationValueUnits.SelectedIndex;
                    estWin.estimationName.SelectedIndex = ri;
                    estWin.estimationUnits.SelectedIndex = ri;
                    estWin.estimationValueTotal.SelectedIndex = ri;
                }
                else
                {
                    ri = estWin.estimationUnits.SelectedIndex;
                    estWin.estimationName.SelectedIndex = ri;
                    estWin.estimationUnits.SelectedIndex = ri;
                    estWin.estimationValueUnits.SelectedIndex = ri;
                }

                estWin.selectedField = ri;

                estWin.name = (TextBoxNames)estWin.estimationName.SelectedItem;
                estWin.unit = (TextBoxNames)estWin.estimationUnits.SelectedItem;
                estWin.valueUnit = (TextBoxNames)estWin.estimationValueUnits.SelectedItem;
                estWin.valueTotal = (TextBoxNames)estWin.estimationValueTotal.SelectedItem;
                estWin.valueTotal.txtNumber.Text = estWin.valueTotal.lbl.Content.ToString();

                estWin.sum = estWin.sum - Convert.ToInt32(estWin.valueTotal.lbl.Content);

                estWin.totalValue.Text = ""+estWin.sum; 

                estWin.name.lbl.Visibility = Visibility.Collapsed;
                estWin.unit.lbl.Visibility = Visibility.Collapsed;
                estWin.valueUnit.lbl.Visibility = Visibility.Collapsed;
                estWin.valueTotal.lbl.Visibility = Visibility.Collapsed;


                estWin.name.txtText.Visibility = Visibility.Visible;
                estWin.unit.txtNumber.Visibility = Visibility.Visible;
                estWin.valueUnit.txtNumber.Visibility = Visibility.Visible;
                estWin.valueTotal.txtNumber.Visibility = Visibility.Visible;


                estWin.allowSelected = false;
            }
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = false;
            if(estWin != null)
            {
                if (opc == 2)
                {
                    TextBoxNames evu = (TextBoxNames)estWin.estimationValueUnits.SelectedItem;
                    TextBoxNames un = (TextBoxNames)estWin.estimationUnits.SelectedItem;
                    TextBoxNames evt = (TextBoxNames)estWin.estimationValueTotal.SelectedItem;
                    try
                    {
                        double vUnit = Convert.ToDouble(evu.txtNumber.Text.Replace(".",","));
                        double unit = Convert.ToDouble(un.txtNumber.Text.Replace(".", ","));
                        evt.txtNumber.Text = "" + (unit * vUnit);
                    }
                    catch
                    {
                        if (evt != null) evt.txtNumber.Text = "" + 0;
                    }
                }
                else if (opc == 3)
                {
                    TextBoxNames eu = (TextBoxNames)estWin.estimationUnits.SelectedItem;
                    TextBoxNames evu = (TextBoxNames)estWin.estimationValueUnits.SelectedItem;
                    TextBoxNames evt = (TextBoxNames)estWin.estimationValueTotal.SelectedItem;
                    try
                    {
                        double unit = Convert.ToDouble(eu.txtNumber.Text.Replace(".", ","));
                        double vUnit = Convert.ToDouble(evu.txtNumber.Text.Replace(".", ","));
                        evt.txtNumber.Text = "" + (unit * vUnit);
                    }
                    catch
                    {
                        if (evt != null) evt.txtNumber.Text = "" + 0;
                    }
                }

            }
            
        }

        private void lbl2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lbl2.Visibility = Visibility.Collapsed;
            txtText.Focus();
            txtNumber.Focus();
        }
    }
}
