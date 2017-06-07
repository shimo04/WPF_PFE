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

namespace PFE_WPF
{
    /// <summary>
    /// Interaction logic for PlanInfo.xaml
    /// </summary>
    public partial class PlanInfo : Window
    {
        public String cam;
        public String card;
        public String deco;
        public String distanse;
        public String effIN;
        public String effJN;
        public String haut;
        public String objf;
        public String pln;
        public String seq;
        public String son;

        public PlanInfo()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
