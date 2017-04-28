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
using Firebase.Database;
using Firebase.Database.Query;
using System.Net;
using PFE_WPF;

namespace PFE_WPF
{
    /// <summary>
    /// Interaction logic for Homie.xaml
    /// </summary>
    public partial class Homie : Window
    {
        public Homie()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var dinos = await firebase.Child("y9chuOyRbRRtD0KsPhQqKT9ETeC3").Child("movies").OrderByKey().StartAt("fjj").OnceAsync<Movies>();
            foreach (var dino in dinos)
            {
                MessageBox.Show ( $"Titre est { dino.Key} : production est { dino.Object.Production} et realisateur est { dino.Object.Realisateur}".ToString());
                //var res = $"{ dino.Key} is { dino.Object.Height}m high.".ToString();
                //MessageBox.Show(res);
                //MessageBox.Show($"{ dino.Key} is { dino.Object.Order}m high.”");
            }
        }
    }
}
