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

using MultimediaLucene.Model;
using MultimediaLucene.LuceneSearch;

namespace MultimediaLucene.UControl
{
    /// <summary>
    /// Interaction logic for SharedFiles.xaml
    /// </summary>
    public partial class SharedFiles : Window
    {
        Pictures pic;
        Videos vid;
        Sounds sou;
        Document doc;
        int op = 0;
        Dictionary<string, string> dat;
        ArchivosMultimedia arc;
        public SharedFiles(Pictures med)
        {
            InitializeComponent();
            pic = med;
            this.dat = med.info;
            checkOpt(Convert.ToInt32(med.info["vis"]));
            arc = new ArchivosMultimedia(med.mw.db);
            try
            {
                this.Visibility = Visibility.Visible;
            }
            catch { }
            
        }
        public SharedFiles(Videos med)
        {
            InitializeComponent();
            vid = med;
            this.dat = med.info;
            checkOpt(Convert.ToInt32(med.info["vis"]));
            try
            {
                this.Visibility = Visibility.Visible;
            }
            catch { }
            arc = new ArchivosMultimedia(med.mw.db);
        }
        public SharedFiles(Sounds med)
        {
            InitializeComponent();
            sou = med;
            this.dat = med.info;
            checkOpt(Convert.ToInt32(med.info["vis"]));
            try
            {
                this.Visibility = Visibility.Visible;
            }
            catch { }
            arc = new ArchivosMultimedia(med.mw.db);
        }
        public SharedFiles(Document med)
        {
            InitializeComponent();
            doc = med;
            this.dat = med.info;
            checkOpt(Convert.ToInt32(med.info["vis"]));
            try
            {
                this.Visibility = Visibility.Visible;
            }
            catch { }
            arc = new ArchivosMultimedia(med.mw.db);
        }
        
        private void checkOpt(int op)
        {

            //try
            //{
            //    MessageBox.Show(
            //         "\n srcServ:    " + dat["srcServ"]
                   
            //        );
            //    dat["srcServ"]=dat["srcServ"].Replace("knower.udenar.edu.co:92", "knower.udenar.edu.co");
            //}
            //catch
            //{
            //    MessageBox.Show(dat["srcServ"]);
            //}
            




            this.op = op;
            switch (op)
            {
                case 1:
                    rbIm.IsChecked = true;
                    break;
                case 2:
                    rbPar.IsChecked = true;
                    break;
                case 3:
                    rbPub.IsChecked = true;
                    break;

            }
        }
           
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (rbIm.IsChecked == true)
                dat["vis"] = "1";
            else if (rbPar.IsChecked == true)
                dat["vis"] = "2";
            else if (rbPub.IsChecked == true)
                dat["vis"] = "3";

            //dat["own"] = "3";

            
            LuceneAct lc = new LuceneAct();
            bool sb = arc.updateShare(dat);
            bool st = lc.luceneUpdate(dat);

            if (st == true)
                MessageBox.Show("Se actualizo correctamente.");
            else
            if(st==false || sb==false)
                MessageBox.Show("Error: No se puede actualizar.");

            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
