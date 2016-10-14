using ControlDB.Model;
using System;
using System.Collections;
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
    /// Lógica de interacción para ResourcesWindow.xaml
    /// </summary>
    public partial class ResourcesWindow : Window
    {
        public string name { get; set; }
        public TextBoxNames resName, resUnit;
        public ProjectPanel proPan;
        public ActivityPanel actPan;
        public bool allowSelected = true;
        public List<List<string>> listResources;
        public int sum = 0, selectedField;

        public ResourcesWindow(ProjectPanel pPan, List<List<string>> lstres,bool enabled)
        {
            proPan = pPan;
            InitializeComponent();
            listResources = lstres;
            loadResources();            
            selectFinalField();

            if (enabled)
            {
                addResourceField();
            }
            else
            {
                saveResource.Visibility = Visibility.Collapsed;
                updateResource.Visibility = Visibility.Collapsed;
                removeResource.Visibility = Visibility.Collapsed;   
            }
        }

        public ResourcesWindow(ActivityPanel actPan, List<List<string>> lstres, bool enabled)
        {
            this.actPan = actPan;
            InitializeComponent();
            listResources = lstres;
            loadResources();
            selectFinalField();

            if (enabled)
            {
                addResourceField();
            }
            else
            {
                saveResource.Visibility = Visibility.Collapsed;
                updateResource.Visibility = Visibility.Collapsed;
                removeResource.Visibility = Visibility.Collapsed;
            }
        }

        private void loadResources()
        {
            foreach (List<string> lr in listResources)
            {
                addResourceField(lr.ElementAt(0), "" + lr.ElementAt(1));
                sum = sum + Convert.ToInt32(lr.ElementAt(1));
            }
            totalUnits.Text = "" + sum;
        }

        private void removeResource_Click(object sender, RoutedEventArgs e)
        {
            int sel = resourceName.SelectedIndex;            
            if(sel != resourceName.Items.Count-1)
            {
                TextBoxNames ru = (TextBoxNames)resourceUnits.SelectedItem;
                sum = sum - Convert.ToInt32(ru.txtNumber.Text);
                totalUnits.Text = "" + sum;
                resourceName.Items.RemoveAt(sel);
                resourceUnits.Items.RemoveAt(sel);
                listResources.RemoveAt(sel);
                selectFinalField();
            }            
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if(proPan != null)
            {
                proPan.txtResourses.Text = totalUnits.Text;
            }
            else
            {
                actPan.txtResourses.Text = totalUnits.Text;
            }
            Close();
        }

        private void saveResource_Click(object sender, RoutedEventArgs e)
        {
            if (resName.txtText.Text != "")
            {
                if (resUnit.txtNumber.Text != "")
                {
                    sum = sum + Convert.ToInt32(resUnit.txtNumber.Text);
                    totalUnits.Text = "" + sum;
                    resName.lbl.Content = resName.txtText.Text;
                    resUnit.lbl.Content = resUnit.txtNumber.Text;
                    resName.lbl.Visibility = Visibility.Visible;
                    resUnit.lbl.Visibility = Visibility.Visible;
                    resName.txtText.Visibility = Visibility.Collapsed;
                    resUnit.txtNumber.Visibility = Visibility.Collapsed;
                    List<string> lstRes = new List<string>();
                    lstRes.Add(resName.txtText.Text);
                    lstRes.Add(resUnit.txtNumber.Text);
                    listResources.Add(lstRes);
                    addResourceField();
                }
                else
                {
                    resUnit.validation.Visibility = Visibility.Visible;
                }
            }
            else
            {
                resName.validation.Visibility = Visibility.Visible;
            }
        }

        private void updateResource_Click(object sender, RoutedEventArgs e)
        {
            if (resName.txtText.Text != "")
            {
                if (resUnit.txtNumber.Text != "")
                {
                    listResources.RemoveAt(selectedField);
                    sum = sum + Convert.ToInt32(resUnit.txtNumber.Text);
                    totalUnits.Text = "" + sum;
                    resName.lbl.Content = resName.txtText.Text;
                    resUnit.lbl.Content = resUnit.txtNumber.Text;
                    resName.lbl.Visibility = Visibility.Visible;
                    resUnit.lbl.Visibility = Visibility.Visible;
                    resName.txtText.Visibility = Visibility.Collapsed;
                    resUnit.txtNumber.Visibility = Visibility.Collapsed;
                    updateResource.Visibility = Visibility.Hidden;
                    saveResource.Visibility = Visibility.Visible;
                    List<string> lstRes = new List<string>();
                    lstRes.Add(resName.txtText.Text);
                    lstRes.Add(resUnit.txtNumber.Text);
                    listResources.Insert(selectedField - 1, lstRes);
                    selectFinalField();
                    allowSelected = true;
                }
            }
        }

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

        private void resourceName_ScrollChanged(object sender, RoutedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(resourceName, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(resourceUnits, typeof(ScrollViewer)) as ScrollViewer;
            _listboxScrollViewer2.ScrollToVerticalOffset(_listboxScrollViewer1.VerticalOffset);
        }

        private void resourceUnits_ScrollChanged(object sender, RoutedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(resourceName, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(resourceUnits, typeof(ScrollViewer)) as ScrollViewer;
            _listboxScrollViewer1.ScrollToVerticalOffset(_listboxScrollViewer2.VerticalOffset);

        }

        private void resourceName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resourceUnits.SelectedIndex = resourceName.SelectedIndex;
        }

        private void resourceUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resourceName.SelectedIndex = resourceUnits.SelectedIndex;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(proPan != null)
            {
                proPan.bres = true;
            }
            else
            {
                actPan.bres = true;
            }
            
        }

        private void addResourceField()
        {
            resName = new TextBoxNames(1, this);
            resUnit = new TextBoxNames(2, this);
            resourceName.Items.Add(resName);
            resourceUnits.Items.Add(resUnit);
        }

        private void addResourceField(string name, string units)
        {
            resName = new TextBoxNames(1, this);
            resUnit = new TextBoxNames(2, this);
            resName.txtText.Text = name;
            resUnit.txtNumber.Text = units;
            resName.lbl.Content = name;
            resUnit.lbl.Content = units;
            resName.lbl.Visibility = Visibility.Visible;
            resUnit.lbl.Visibility = Visibility.Visible;
            resName.txtText.Visibility = Visibility.Collapsed;
            resUnit.txtNumber.Visibility = Visibility.Collapsed;

            resourceName.Items.Add(resName);
            resourceUnits.Items.Add(resUnit);
        }

        private void selectFinalField()
        {
            resourceName.SelectedIndex = resourceName.Items.Count-1;
            resName = (TextBoxNames)resourceName.SelectedItem;
            resUnit = (TextBoxNames)resourceUnits.SelectedItem;
        }
    }
}
