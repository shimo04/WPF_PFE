﻿using System;
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
        public String id;
        public Homie()
        {
        }

        public Homie (String cle)
        {
            InitializeComponent();
            this.id = cle;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var dinos = await firebase.Child(id).Child("movies").OrderByKey().OnceAsync<Movies>();
            foreach (var dino in dinos)
            {
                MessageBox.Show ( $"Titre est { dino.Key} : production est { dino.Object.Production} et realisateur est { dino.Object.Realisateur}.");
            }
        }
    }
}
