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
using MaterialDesignThemes.Wpf;

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

        public Homie(String cle)
        {
            InitializeComponent();
            this.id = cle;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var dinos = await firebase.Child(id).Child("movies").OrderByKey().OnceAsync<Movies>();
            var nb = dinos.Count();
            var res = 0;
            if (nb % 4 != 0)
            {
                res = 1;
            }
            var r = (int)(nb / 4);
            var x = 0;
            while (x <= r + res)
            {

                gr.RowDefinitions.Add(new RowDefinition());
                x++;
            }

            var i = 0;
            var j = 0;
            foreach (var dino in dinos)
            {
                //while(j<=nb/4)
                if (i == 4)
                {
                    i = 0;
                    j++;
                }

                Grid g = new Grid();

                // Create Rows  
                RowDefinition r1 = new RowDefinition();
                r1.Height = new GridLength(145);
                RowDefinition r2 = new RowDefinition();
                GridLengthConverter gridLengthConverter1 = new GridLengthConverter();
                r2.Height = (GridLength)gridLengthConverter1.ConvertFrom("*");
                RowDefinition r3 = new RowDefinition();
                GridLengthConverter gridLengthConverter2 = new GridLengthConverter();
                r3.Height = (GridLength)gridLengthConverter2.ConvertFrom("Auto");

                g.RowDefinitions.Add(r1);
                g.RowDefinitions.Add(r2);
                g.RowDefinitions.Add(r3);

                Image img = new Image();
                img.Height = 140;
                img.HorizontalAlignment = HorizontalAlignment.Stretch;
                //img.Source = new BitmapImage(new Uri("C:\\Users\\lUnA ShImO\\Documents\\Visual Studio 2015\\Projects\\PFE_WPF\\PFE_WPF\\Resources\\Chartridge046_small.jpg"));
                g.Children.Add(img);



                StackPanel stackT = new StackPanel { };
                //stackT.Margin = new Thickness(8, 24, 8, 0);
                stackT.Orientation = Orientation.Vertical;
                //stackT.VerticalAlignment = VerticalAlignment.Stretch;
                //stackT.HorizontalAlignment = HorizontalAlignment.Stretch;
                TextBlock titre = new TextBlock();
                titre.Text = dino.Key;
                titre.TextAlignment = TextAlignment.Center;
                TextBlock prod = new TextBlock();
                //var st = $"production : { dino.Object.Production},realisateur : { dino.Object.Realisateur}.";
                prod.Text = $"Production : { dino.Object.Production}";
                prod.TextAlignment = TextAlignment.Center;
                TextBlock real = new TextBlock();
                real.Text = $"Realisateur : { dino.Object.Realisateur}";
                real.TextAlignment = TextAlignment.Center;

                stackT.Children.Add(titre);
                stackT.Children.Add(prod);
                stackT.Children.Add(real);

                Grid.SetRow(stackT, 1);
                g.Children.Add(stackT);

                StackPanel stackP = new StackPanel { };
                stackP.HorizontalAlignment = HorizontalAlignment.Right;
                stackP.Margin = new Thickness(8);
                stackP.Orientation = Orientation.Horizontal;
                PopupBox pop = new PopupBox();
                Style myStyle = (Style)Resources["{StaticResource MaterialDesignToolPopupBox}"];
                pop.Style = myStyle;
                pop.Padding = new Thickness(2, 0, 2, 0);
                Button more = new Button();
                more.Content = pop;
                var mT = dino.Key;
                more.Click += (Object, RoutedEventArgs) => { Movie_Click(sender, e, mT); };

                stackP.Children.Add(more);
                Grid.SetRow(stackP, 2);
                g.Children.Add(stackP);

                Card cd = new Card();
                //cd.HorizontalAlignment = HorizontalAlignment.Left;

                cd.Height = 270;
                cd.Width = 200;
                cd.Content = g;

                Grid.SetRow(cd, j);
                Grid.SetColumn(cd, i);

                gr.Children.Add(cd);

                i++;
                //MessageBox.Show ( $"Titre est { dino.Key} : production est { dino.Object.Production} 
                //et realisateur est { dino.Object.Realisateur}.");           
            }
        }

    }
}
