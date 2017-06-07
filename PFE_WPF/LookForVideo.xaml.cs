using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace PFE_WPF
{
    /// we code for FUN

    public partial class LookForVideo : Window
    {
        public String id;
        public String mv;
        public LookForVideo(String cle,String mov)
        {
            InitializeComponent();
            this.id = cle;
            this.mv = mov;
        }
        public LookForVideo()
        {
            InitializeComponent();
        }

        private async void valider_Click(object sender, RoutedEventArgs e)
        {
            var res = "";
            if (cha.Value == 3)
            {
                res = "exellente";
            }
            else if (cha.Value == 2)
            {
                res = "moyenne";
            }
            else
            {
                res = "médiocre";
            }

            List<string> listPr = new List<string>();
            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var sequences = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").OrderByKey().OnceAsync<Movies>();
            var nbS = sequences.Count();
            foreach(var seq in sequences)
            {
                var plans = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").Child(seq.Key).Child("plans").OrderByKey().OnceAsync<Plan>();
                var nbP = plans.Count();
                foreach(var plan in plans)
                {
                    var prises = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").Child(seq.Key).Child("plans").Child(plan.Key).Child("listPrise").OrderByKey().OnceAsync<Prise>();
                    var nbPl = prises.Count();
                    foreach(var prise in prises)
                    {
                        if( prise.Object.note == res)
                        {
                            listPr.Add(prise.Key);
                        }
                    }
                }                    
           }

            MessageBox.Show(mv);

        }
    }
}
//bahaeddinebb@outlook.com