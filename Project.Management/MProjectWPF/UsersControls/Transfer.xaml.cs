using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using MProjectWPF.UsersControls.ProjectControls;
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
using System.Windows.Shapes;

namespace MProjectWPF.UsersControls
{
    /// <summary>
    /// Lógica de interacción para Transfer.xaml
    /// </summary>
    public partial class Transfer : Window
    {
        ProjectPanel proPan;
        ActivityPanel actPan;
        long percent;
        double estimation;
        public string namefather;

        public Transfer(ProjectPanel ppn)
        {
            InitializeComponent();
            proPan = ppn;

            namefather = proPan.proMod.nombre;
            percent = 100 - proPan.proMod.caracteristicas.porcentaje_asignado;
            txtPf.Text = percent + "%";

            estimation = Convert.ToDouble(proPan.proMod.caracteristicas.presupuesto) - Convert.ToDouble(proPan.proMod.caracteristicas.costos);
            txtEstimationRe.Text = "" + estimation +" $";
         }

        public Transfer(ActivityPanel actPan)
        {
            InitializeComponent();
            this.actPan = actPan;

            namefather = actPan.actMod.nombre;
            percent = 100 - actPan.actMod.caracteristicas.porcentaje_asignado;
            txtPf.Text = percent + "%";

            estimation = Convert.ToDouble(actPan.actMod.caracteristicas.presupuesto) - Convert.ToDouble(actPan.actMod.caracteristicas.costos);
            txtEstimationRe.Text = "" + estimation + " $";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (proPan != null) projectActivity();
            else activityActivity();
        }

        private void txtPaSh_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txtPercentASh.Visibility = Visibility.Collapsed;
        }

        private void txtPreAsSh_MouseUp(object sender, MouseButtonEventArgs e)
        {
            txtEstimationAsSh.Visibility = Visibility.Collapsed;
        }

        private void projectActivity()
        {
            bool isValid = true;
            double subsEst = -1;
            long subsPer = -1;

            try { subsPer = percent - Convert.ToInt32(txtPercentA.Text); }
            catch { }

            try { subsEst = estimation - Convert.ToDouble(txtEstimationAs.Text); }
            catch { }

            if (subsPer < 0)
            {
                isValid = false;
                txtPercentASh.Visibility = Visibility.Visible;
            }

            if (subsEst < 0)
            {
                isValid = false;
                txtEstimationAsSh.Visibility = Visibility.Visible;
            }

            if (isValid)
            {
                proPan.exPro.workplaceGrid.Children.Clear();
                proPan.exPro.tvProSh.Visibility = Visibility.Visible;

                int max = proPan.exPro.childsCount();
                LabelTreeActivity lup = null;
                if (max > 0)
                {
                    lup = (LabelTreeActivity)proPan.exPro.tvPro.Items.GetItemAt(max - 1);
                    lup.pos = max - 1; 
                }

                LabelTreeActivity lta = new LabelTreeActivity(null);
                lta.showSelectedSh();
                proPan.exPro.tvPro.Items.Add(lta);
                max = proPan.exPro.childsCount();

                if (lup != null)
                {
                    lup.show_arrow(lup, lup.pos, max);
                    lup.show_arrow(lta, max - 1, max);
                }

                ActivityPanel act = new ActivityPanel(proPan, lta, this);
                proPan.exPro.workplaceGrid.Children.Add(act);
                Close();
            }
        }

        private void activityActivity()
        {
            bool isValid = true;
            double subsEst = -1;
            long subsPer = -1;
            try { subsPer = percent - Convert.ToInt32(txtPercentA.Text);  }
            catch{ }

            try { subsEst = estimation - Convert.ToDouble(txtEstimationAs.Text);  }
            catch{ }
            

            if (subsPer < 0)
            {
                isValid = false;
                txtPercentASh.Visibility = Visibility.Visible;
            }

            if (subsEst < 0)
            {
                isValid = false;
                txtEstimationAsSh.Visibility = Visibility.Visible;
            }
            if (isValid)
            {
                actPan.exPro.workplaceGrid.Children.Clear();
                actPan.exPro.tvProSh.Visibility = Visibility.Visible;

                LabelTreeActivity lta = new LabelTreeActivity(actPan.lta);
                actPan.exPro.lastSelectlta = lta;
                lta.showSelectedSh();

                //condicion de actividad seleccionada
                bool ac = true;
                if (actPan.exPro.actPanCurrent != null)
                {
                    if (actPan.exPro.actPanCurrent.actMod == actPan.actMod)
                    {
                        int max = actPan.exPro.childsCount();
                        LabelTreeActivity lup = null;
                        if (max > 0)
                        {
                            lup = (LabelTreeActivity)actPan.exPro.tvPro.Items.GetItemAt(max - 1);
                            lup.pos = max - 1;
                        }

                        ///////////////////////////////////////////////////////

                        actPan.exPro.tvPro.Items.Add(lta);

                        ///////////////////////////////////////////////////////

                        max = actPan.exPro.childsCount();
                        if (lup != null)
                        {
                            lup.show_arrow(lup, lup.pos, max);
                            lup.show_arrow(lta, max - 1, max);
                        }
                        ac = false;
                    }
                }
                if (ac)
                {
                    int max = actPan.lta.childsCount();
                    LabelTreeActivity lup = null;
                    if (max > 0)
                    {
                        lup = (LabelTreeActivity)actPan.lta.tvi.Items.GetItemAt(max - 1);
                        lup.pos = max - 1;
                    }

                    ////////////////////////////////////////////
                    lta.Margin = new Thickness(-38, 0, 0, 0);
                    actPan.lta.addchilds(lta);
                    actPan.lta.showOpenFolder();
                    actPan.isExpanded = lta.tvi.IsExpanded;
                    actPan.lta.tvi.IsExpanded = true;
                    actPan.lta.hideSelectedSh();
                    ////////////////////////////////////////////

                    max = actPan.lta.childsCount();
                    if (lup != null)
                    {
                        actPan.lta.show_arrow(lup, lup.pos, max);
                    }
                    actPan.lta.show_arrow(lta, max - 1, max);
                }

                ActivityPanel act = new ActivityPanel(actPan, lta, actPan.exPro, this);
                actPan.exPro.workplaceGrid.Children.Add(act);
                Close();
            }
        }

        public double estimationValue()
        {
            return Convert.ToDouble(txtEstimationAs.Text);
        }

        private void txtPercentA_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void txtEstimationAs_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]");
            Regex regex2 = new Regex("[0-9]+");
            Regex regex3 = new Regex("(\\,|\\.)");

            if (txtEstimationAs.Text == "" && regex.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else if (regex2.IsMatch(txtEstimationAs.Text) && regex.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else if (regex3.IsMatch(txtEstimationAs.Text) && regex3.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            else if (regex2.IsMatch(txtEstimationAs.Text) && regex3.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
