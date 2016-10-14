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
using System.Windows.Shapes;

namespace MProjectWPF.UsersControls.ProjectControls.WindowsControls
{
    /// <summary>
    /// Lógica de interacción para EstimationWindow.xaml
    /// </summary>
    public partial class EstimationWindow : Window
    {
        public TextBoxNames name, unit, valueUnit, valueTotal;
        public ProjectPanel proPan;
        public ActivityPanel actPan;
        public bool allowSelected = true;
        public List<List<string>> listResources;
        public Double sum = 0;
        public int selectedField;
        Transfer tran;
        int opc;

        public EstimationWindow(ProjectPanel pPan, List<List<string>> lstres, int o,bool enable)
        {
            InitializeComponent();
            proPan = pPan;
            opc = o;
            listResources = lstres;
            loadEstimation();
            selectFinalField();

            if (enable)
            {
                addResourceField();
            }
            else
            {
                saveEstimation.Visibility = Visibility.Collapsed;
                updateEstimation.Visibility = Visibility.Collapsed;
                removeEstimation.Visibility = Visibility.Collapsed;
            }
        }

        public EstimationWindow(ActivityPanel actPan, List<List<string>> lstres, int o, bool enable)
        {
            InitializeComponent();
            this.actPan = actPan;
            tran = actPan.tran;
            opc = o;
            listResources = lstres;
            loadEstimation();
            selectFinalField();
            if (enable)
            {
                addResourceField();
            }
            else
            {
                saveEstimation.Visibility = Visibility.Collapsed;
                updateEstimation.Visibility = Visibility.Collapsed;
                removeEstimation.Visibility = Visibility.Collapsed;
            }
        }

        private void loadEstimation()
        {   
            foreach (List<string> lest in listResources)
            {   
                addResourceField(lest.ElementAt(0), lest.ElementAt(1), lest.ElementAt(2), lest.ElementAt(3));
                sum = sum + Convert.ToDouble(lest.ElementAt(3));
            }
            totalValue.Text = "" + sum;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if(proPan != null)
            {
                if (opc == 1)
                {
                    double add = Convert.ToDouble(totalValue.Text) - Convert.ToDouble(proPan.txtEstimation.Text);
                    proPan.valueEstimations = proPan.valueEstimations + add;
                    proPan.txtEstimation.Text = totalValue.Text;                    
                    proPan.best = true;
                }
                else
                {
                    proPan.txtCost.Text = totalValue.Text;
                    proPan.bcost = true;
                }
            }
            else
            {
                if (opc == 1)
                {
                    actPan.txtEstimation.Text = totalValue.Text;
                    actPan.best = true;
                }
                else
                {
                    actPan.txtCost.Text = totalValue.Text;
                    actPan.bcost = true;
                }
            }            
            Close();
        }

        private void updateEstimation_Click(object sender, RoutedEventArgs e)
        {
            if (name.txtText.Text != "")
            {
                if (unit.txtNumber.Text != "")
                {
                    if (valueUnit.txtNumber.Text != "")
                    {
                        listResources.RemoveAt(selectedField - 1);
                        sum = sum + Convert.ToInt32(valueTotal.txtNumber.Text);
                        totalValue.Text = "" + sum;
                        name.lbl.Content = name.txtText.Text;
                        unit.lbl.Content = unit.txtNumber.Text;
                        valueUnit.lbl.Content = valueUnit.txtNumber.Text;
                        valueTotal.lbl.Content = valueTotal.txtNumber.Text;

                        name.lbl.Visibility = Visibility.Visible;
                        unit.lbl.Visibility = Visibility.Visible;
                        valueUnit.lbl.Visibility = Visibility.Visible;
                        valueTotal.lbl.Visibility = Visibility.Visible;

                        name.txtText.Visibility = Visibility.Collapsed;
                        unit.txtNumber.Visibility = Visibility.Collapsed;
                        valueUnit.txtNumber.Visibility = Visibility.Collapsed;
                        valueTotal.txtNumber.Visibility = Visibility.Collapsed;


                        List<string> lstRes = new List<string>();
                        lstRes.Add(name.txtText.Text);
                        lstRes.Add(unit.txtNumber.Text);
                        lstRes.Add(valueUnit.txtNumber.Text);
                        lstRes.Add(valueTotal.txtNumber.Text);

                        listResources.Insert(selectedField - 1, lstRes);
                        allowSelected = true;

                        selectFinalField();

                        updateEstimation.Visibility = Visibility.Hidden;
                        saveEstimation.Visibility = Visibility.Visible;


                    }
                    else
                    {
                        valueUnit.validation.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    unit.validation.Visibility = Visibility.Visible;
                }
            }
            else
            {
                name.validation.Visibility = Visibility.Visible;
            }
        }

        private void saveEstimation_Click(object sender, RoutedEventArgs e)
        {
            if (name.txtText.Text != "")
            {
                if (unit.txtNumber.Text != "")
                {
                    if (valueUnit.txtNumber.Text != "")
                    {
                        sum = sum + Convert.ToInt32(valueTotal.txtNumber.Text);
                        totalValue.Text = "" + sum;

                        name.lbl.Content = name.txtText.Text;
                        unit.lbl.Content = unit.txtNumber.Text;
                        valueUnit.lbl.Content = valueUnit.txtNumber.Text;
                        valueTotal.lbl.Content = valueTotal.txtNumber.Text;

                        name.lbl.Visibility = Visibility.Visible;
                        unit.lbl.Visibility = Visibility.Visible;
                        valueUnit.lbl.Visibility = Visibility.Visible;
                        valueTotal.lbl.Visibility = Visibility.Visible;

                        name.txtText.Visibility = Visibility.Collapsed;
                        unit.txtNumber.Visibility = Visibility.Collapsed;
                        valueUnit.txtNumber.Visibility = Visibility.Collapsed;
                        valueTotal.txtNumber.Visibility = Visibility.Collapsed;

                        List<string> lstRes = new List<string>();
                        lstRes.Add(name.txtText.Text);
                        lstRes.Add(unit.txtNumber.Text);
                        lstRes.Add(valueUnit.txtNumber.Text);
                        lstRes.Add(valueTotal.txtNumber.Text);

                        listResources.Add(lstRes);
                        addResourceField();
                        estimationName.SelectedIndex = estimationName.Items.Count - 1;
                    }
                    else
                    {
                        valueUnit.validation.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    unit.validation.Visibility = Visibility.Visible;
                }
            }
            else
            {
                name.validation.Visibility = Visibility.Visible;
            }

        }

        private void removeEstimation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int sel = estimationName.SelectedIndex;
                if (sel != estimationName.Items.Count - 1)
                {
                    TextBoxNames evt = (TextBoxNames)estimationValueTotal.SelectedItem;
                    sum = sum - Convert.ToDouble(evt.txtNumber.Text);
                    estimationName.Items.RemoveAt(sel);
                    estimationUnits.Items.RemoveAt(sel);
                    estimationValueUnits.Items.RemoveAt(sel);
                    estimationValueTotal.Items.RemoveAt(sel);
                    listResources.RemoveAt(sel);
                    selectFinalField();
                }
            }
            catch { }
        }       

        private void estimationName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allowSelected)
            {
                estimationUnits.SelectedIndex = estimationName.SelectedIndex;
                estimationValueUnits.SelectedIndex = estimationName.SelectedIndex;
                estimationValueTotal.SelectedIndex = estimationName.SelectedIndex;
            }
        }

        private void estimationUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allowSelected)
            {
                estimationName.SelectedIndex = estimationUnits.SelectedIndex;
                estimationValueUnits.SelectedIndex = estimationUnits.SelectedIndex;
                estimationValueTotal.SelectedIndex = estimationUnits.SelectedIndex;
            }
        }

        private void estimationValueUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allowSelected)
            {
                estimationName.SelectedIndex = estimationValueUnits.SelectedIndex;
                estimationUnits.SelectedIndex = estimationValueUnits.SelectedIndex;
                estimationValueTotal.SelectedIndex = estimationValueUnits.SelectedIndex;
            }
        }

        private void estimationValueTotal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allowSelected)
            {
                estimationName.SelectedIndex = estimationValueTotal.SelectedIndex;
                estimationUnits.SelectedIndex = estimationValueTotal.SelectedIndex;
                estimationValueUnits.SelectedIndex = estimationValueTotal.SelectedIndex;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (proPan != null)
            {
                if (opc == 1) proPan.best = true;
                else proPan.bcost = true;
            }
            else
            {
                if (opc == 1) actPan.best = true;
                else actPan.bcost = true;
            }
        }

        private void addResourceField()
        {
            name = new TextBoxNames(1, this);
            unit = new TextBoxNames(2, this);
            valueUnit = new TextBoxNames(3, this);
            valueTotal = new TextBoxNames(4, this);

            valueTotal.txtNumber.IsReadOnly = true;
            unit.txtNumber.Text = "1";
            valueUnit.txtNumber.Text = "0";
            valueTotal.txtNumber.Text = "0";

            estimationName.Items.Add(name);
            estimationUnits.Items.Add(unit);
            estimationValueUnits.Items.Add(valueUnit);
            estimationValueTotal.Items.Add(valueTotal);
        }

        private void addResourceField(string lname, string lunit, string lvunit, string lvtotal)
        {
            name = new TextBoxNames(1, this);
            unit = new TextBoxNames(2, this);
            valueUnit = new TextBoxNames(3, this);
            valueTotal = new TextBoxNames(4, this);

            name.txtText.Text = lname;
            valueTotal.txtNumber.IsReadOnly = true;
            unit.txtNumber.Text = lunit;
            valueUnit.txtNumber.Text = lvunit;
            valueTotal.txtNumber.Text = lvtotal;

            name.lbl.Content = lname;
            unit.lbl.Content = lunit;
            valueUnit.lbl.Content = lvunit;
            valueTotal.lbl.Content = lvtotal;

            name.lbl.Visibility = Visibility.Visible;
            unit.lbl.Visibility = Visibility.Visible;
            valueUnit.lbl.Visibility = Visibility.Visible;
            valueTotal.lbl.Visibility = Visibility.Visible;

            name.txtText.Visibility = Visibility.Collapsed;
            unit.txtNumber.Visibility = Visibility.Collapsed;
            valueUnit.txtNumber.Visibility = Visibility.Collapsed;
            valueTotal.txtNumber.Visibility = Visibility.Collapsed;

            estimationName.Items.Add(name);
            estimationUnits.Items.Add(unit);
            estimationValueUnits.Items.Add(valueUnit);
            estimationValueTotal.Items.Add(valueTotal);
        }

        private void selectFinalField()
        {
            estimationName.SelectedIndex = estimationName.Items.Count;
            name = (TextBoxNames)estimationName.SelectedItem;
            unit = (TextBoxNames)estimationUnits.SelectedItem;
            valueUnit = (TextBoxNames)estimationValueUnits.SelectedItem;
            valueTotal = (TextBoxNames)estimationValueTotal.SelectedItem;
        }

        //CONTROL BARRAS ////////////////////////////////////
        private void estimationName_ScrollChanged(object sender, RoutedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(estimationName, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(estimationUnits, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer3 = GetDescendantByType(estimationValueUnits, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer4 = GetDescendantByType(estimationValueTotal, typeof(ScrollViewer)) as ScrollViewer;

            _listboxScrollViewer2.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
            _listboxScrollViewer3.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
            _listboxScrollViewer4.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);

        }

        private void estimationUnits_ScrollChanged(object sender, RoutedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(estimationName, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(estimationUnits, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer3 = GetDescendantByType(estimationValueUnits, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer4 = GetDescendantByType(estimationValueTotal, typeof(ScrollViewer)) as ScrollViewer;

            _listboxScrollViewer1.ScrollToVerticalOffset(_listboxScrollViewer2.VerticalOffset);
            _listboxScrollViewer3.ScrollToVerticalOffset(_listboxScrollViewer2.VerticalOffset);
            _listboxScrollViewer4.ScrollToVerticalOffset(_listboxScrollViewer2.VerticalOffset);

        }

        private void estimationValueUnits_ScrollChanged(object sender, RoutedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(estimationName, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(estimationUnits, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer3 = GetDescendantByType(estimationValueUnits, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer4 = GetDescendantByType(estimationValueTotal, typeof(ScrollViewer)) as ScrollViewer;

            _listboxScrollViewer1.ScrollToVerticalOffset(_listboxScrollViewer3.VerticalOffset);
            _listboxScrollViewer2.ScrollToVerticalOffset(_listboxScrollViewer3.VerticalOffset);
            _listboxScrollViewer4.ScrollToVerticalOffset(_listboxScrollViewer3.VerticalOffset);
        }

        private void estimationValueTotal_ScrollChanged(object sender, RoutedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(estimationName, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(estimationUnits, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer3 = GetDescendantByType(estimationValueUnits, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer4 = GetDescendantByType(estimationValueTotal, typeof(ScrollViewer)) as ScrollViewer;

            _listboxScrollViewer1.ScrollToVerticalOffset(_listboxScrollViewer4.VerticalOffset);
            _listboxScrollViewer2.ScrollToVerticalOffset(_listboxScrollViewer4.VerticalOffset);
            _listboxScrollViewer3.ScrollToVerticalOffset(_listboxScrollViewer4.VerticalOffset);
        }

        //Sincroniza las barras
        public Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }

        //FINISH ////////////////////////////////////////////



    }
}
