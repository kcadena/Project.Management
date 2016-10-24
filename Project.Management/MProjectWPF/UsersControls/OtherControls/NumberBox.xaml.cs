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

namespace MProjectWPF.UsersControls.OtherControls
{
    /// <summary>
    /// Lógica de interacción para NumberBox.xaml
    /// </summary>
    public partial class NumberBox : System.Windows.Controls.UserControl
    {
        public NumberBox()
        {
            InitializeComponent();
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {   
            NumberTxt.Text = "" + (Convert.ToDouble(NumberTxt.Text) + 1);
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            NumberTxt.Text = "" + (Convert.ToDouble(NumberTxt.Text) - 1);
        }

        private void NumberTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]");
            Regex regex2 = new Regex("[0-9]+");
            Regex regex3 = new Regex("(\\,|\\.)");

            if (NumberTxt.Text == "" && regex.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else if (regex2.IsMatch(NumberTxt.Text) && regex.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else if (regex3.IsMatch(NumberTxt.Text) && regex3.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            else if (regex2.IsMatch(NumberTxt.Text) && regex3.IsMatch(e.Text))
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
