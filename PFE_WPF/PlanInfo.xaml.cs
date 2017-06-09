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
        public String dist;
        public String effIN;
        public String effJN;
        public String haut;
        public String objf;
        public String pln;
        public String seq;
        public String son;
        public String mv;
        public String source;

        public PlanInfo()
        {
            
        }
        public PlanInfo(String mv, String seq, String pln, String camera, String card, String deco, String effIN, String effJN, String haut, String distance, String objf, String son, String src)
        {
            InitializeComponent();
            this.mv = mv; 
            this.seq = seq;
            this.pln = pln;
            this.cam = camera;
            this.card = card;
            this.deco = deco;
            this.effIN = effIN;
            this.effJN = effJN;
            this.haut = haut;
            this.dist = distance;
            this.objf = objf;
            this.son = son;
            this.source = src;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            entetePlan.Content = "      FILM : " + mv + "      " + seq + " / " + pln;

            camera.Content = "CAMERA : "+cam;
            cardSD.Content = "CARD SD : " + card;
            decor.Content = "DECOR : " + deco;
            effetIN.Content = "EFFET IN : " + effIN;
            effetJN.Content = "EFFET JN : " + effJN;
            hauteur.Content = "HAUTEUR : " + haut;
            distance.Content = "DISTANCE : " + dist;
            objectif.Content = "OBJECTIF : " + objf;
            sonOption.Content = "SON OPTION : " + son;
            imgPlan.Source = new BitmapImage(new Uri(source));
        }
    }
}
