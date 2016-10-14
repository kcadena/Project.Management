using MProjectWPF.Controller;
using ControlDB.Model;
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

namespace MProjectWPF.UsersControls.TemplatesControls
{
    
    public partial class ListTemplatePanel : System.Windows.Controls.UserControl
    {
        LabelTemplate labTemp;
        public ControlXml cxml;
        MainWindow mainW;
        NewTemplatePanel newTemp;
        Plantillas plant;

        public ListTemplatePanel(MainWindow mw, NewTemplatePanel ntp)
        {
            InitializeComponent();
            cxml = new ControlXml("Logs//TemplateTemp.xml");
            mainW = mw;
            plant=new Plantillas(mainW.dbMP);
            newTemp = ntp;
            loadTemplates(ntp);            
        }

        private void loadTemplates( NewTemplatePanel ntp)
        {   
            foreach (plantillas pla in plant.listTemplate())
            {
                LabelTemplate lTemp = new LabelTemplate(pla,mainW.dbMP,this,ntp);
                lstTemplates.Items.Add(lTemp);
            }
            lstTemplates.SelectedIndex = 0;
        }

        private void lstTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FieldTemplatePanel ftp = new FieldTemplatePanel();
            if (labTemp != null)
            {
                labTemp.hiddeButtons();
                cxml.removeAllFields();
            }
            if (lstTemplates.SelectedIndex >= 0)
            {
                labTemp = (LabelTemplate)lstTemplates.SelectedItem;
                labTemp.showButtons();
 
                cxml.createXmlFromDatabase(labTemp.plant);
                
                ftp = new FieldTemplatePanel(labTemp.plant.nombre.ToUpper());
                plant.listBoxField(labTemp.plant,ftp);                
            }
            mainW.spViewA.Children.Clear();
            mainW.spViewA.Children.Add(ftp);
        }

        public void updateListTemplates()
        {
            lstTemplates.Items.Clear();
            loadTemplates(newTemp);
        }

        public void updateListTemplates(LabelTemplate lTemp)
        {
            lstTemplates.Items.Remove(lTemp);
            lstTemplates.SelectedIndex = 0;
        }
    }
}
