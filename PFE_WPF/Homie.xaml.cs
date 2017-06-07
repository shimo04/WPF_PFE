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
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace PFE_WPF
{
    /// we code for FUN

    public partial class Homie : Window
    {
        public String id;
        public String movieTile;
        public String sequenceMovie;
        public Homie()
        {
        }

        public Homie(String cle)
        {
            InitializeComponent();
            this.id = cle;
        }

        private void LogOut_Button(object sender, RoutedEventArgs e)
        {
            var LogIn = new LogIn();
            LogIn.Show();
            this.Close();
        }
        public void Saisie_Grid(object sender, RoutedEventArgs e, Grid g)
        {
            gr.Children.Clear();
            gr.RowDefinitions.Clear();

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
        }
        public void Saisie_Image(object sender, RoutedEventArgs e, Image img, int height, String source)
        {
            img.Height = height;
            img.Source = new BitmapImage(new Uri(source));
        }
        public void Saisie_Icon(object sender, RoutedEventArgs e, Image img, int height, String source)
        {

        }
        private async void recherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var movies = await firebase.Child(id).Child("movies").OrderByKey().OnceAsync<Movies>();
            foreach (var movie in movies)
            {
                if (recherche.Text == movie.Object.Titre)
                {
                    Grid g = new Grid();
                    Saisie_Grid(sender, e,g);

                    Image img = new Image();
                    img.HorizontalAlignment = HorizontalAlignment.Stretch;
                    Saisie_Image(sender, e, img, 140, "C:\\Users\\lUnA ShImO\\Documents\\Visual Studio 2015\\Projects\\PFE_WPF\\PFE_WPF\\Resources\\Chartridge046_small.jpg");
                    g.Children.Add(img);

                    PackIcon ic = new PackIcon();
                    ic.Width = 20;
                    ic.Height = 20;
                    ic.Kind = PackIconKind.Camera;
                    ic.HorizontalAlignment = HorizontalAlignment.Right;
                    ic.VerticalAlignment = VerticalAlignment.Bottom;

                    Grid.SetRow(ic, 0);
                    g.Children.Add(ic);

                    StackPanel stackT = new StackPanel { };
                    stackT.Orientation = Orientation.Vertical;
                    Chip titre = new Chip();
                    titre.HorizontalAlignment = HorizontalAlignment.Center;
                    titre.FontWeight = FontWeights.Bold;
                    titre.Content = movie.Key;
                    titre.IconBackground = Brushes.Yellow;
                    TextBlock prod = new TextBlock();
                    prod.FontWeight = FontWeights.Bold;
                    prod.Text = $"Production : { movie.Object.Production}";
                    prod.TextAlignment = TextAlignment.Center;
                    TextBlock real = new TextBlock();
                    real.FontWeight = FontWeights.Bold;
                    real.Text = $"Realisateur : { movie.Object.Realisateur}";
                    real.TextAlignment = TextAlignment.Center;

                    stackT.Children.Add(titre);
                    stackT.Children.Add(prod);
                    stackT.Children.Add(real);

                    Grid.SetRow(stackT, 1);
                    g.Children.Add(stackT);

                    var sequences = await firebase.Child(id).Child("movies").Child(movie.Key).Child("Sequence").OrderByKey().OnceAsync<Movies>();
                    var nbSeq = sequences.Count().ToString();

                    StackPanel stackP = new StackPanel { };
                    stackP.HorizontalAlignment = HorizontalAlignment.Right;
                    stackP.Margin = new Thickness(8);
                    stackP.Orientation = Orientation.Horizontal;
                    PackIcon pop = new PackIcon();
                    Style myStyle = (Style)Resources["{StaticResource MaterialDesignToolPopupBox}"];
                    pop.Style = myStyle;
                    pop.Kind = PackIconKind.Movie;
                    pop.Padding = new Thickness(2, 0, 2, 0);
                    Button more = new Button();
                    more.Width = 50;
                    more.Content = pop;
                    var mv = movie.Key;
                    more.Click += (Object, RoutedEventArgs) => { Movie_Click(sender, e, mv); };

                    Badged moreBg = new Badged();
                    moreBg.Badge = nbSeq;
                    moreBg.Content = more;

                    stackP.Children.Add(moreBg);
                    Grid.SetRow(stackP, 2);
                    g.Children.Add(stackP);

                    Grid grdEspace = new Grid();

                    // Create Rows  
                    RowDefinition row1 = new RowDefinition();
                    GridLengthConverter gridLengthConverterrow1 = new GridLengthConverter();
                    row1.Height = (GridLength)gridLengthConverterrow1.ConvertFrom("*");
                    RowDefinition row2 = new RowDefinition();
                    GridLengthConverter gridLengthConverterrow2 = new GridLengthConverter();
                    row2.Height = (GridLength)gridLengthConverterrow2.ConvertFrom("Auto");

                    grdEspace.RowDefinitions.Add(row1);
                    grdEspace.RowDefinitions.Add(row2);

                    TextBlock vide = new TextBlock();

                    Card cd = new Card();
                    //cd.HorizontalAlignment = HorizontalAlignment.Left;
                    ShadowAssist.SetShadowDepth(cd, ShadowDepth.Depth5);
                    cd.Height = 270;
                    cd.Width = 200;
                    cd.Content = g;
                    gr.Children.Add(cd);
                }
            }
        }
        private void Load_Recherche(object sender, RoutedEventArgs e)
        {
            var LookForVideo = new LookForVideo(id, movieTile);
            LookForVideo.Left = 890;
            LookForVideo.Top = 10;
            LookForVideo.Width = 480;
            LookForVideo.Height = 700;
            LookForVideo.Show();
        }

        private void Previous_Button(object sender, RoutedEventArgs e, String nomMethod)
        {
            Button prev = new Button();
            Color color = (Color)ColorConverter.ConvertFromString("#FFAEEA00");
            prev.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
            ShadowAssist.SetShadowDepth(prev, ShadowDepth.Depth5);
            if (nomMethod == "Window_Loaded") 
            {
                PackIcon prevIcon = new PackIcon();
                prevIcon.Kind = PackIconKind.ArrowLeftBold;
                prevIcon.Background = Brushes.Black;
                prev.Content = prevIcon;
                headerStack.Children.Add(prev);
                prev.Click += (Object, RoutedEventArgs) => { Window_Loaded(sender, e); };
            }
            else if (nomMethod == "Movie_Click") 
            {
                Button home = new Button();
                home.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                ShadowAssist.SetShadowDepth(home, ShadowDepth.Depth5);
                PackIcon homeIcon = new PackIcon();
                homeIcon.Kind = PackIconKind.Home;
                homeIcon.Background = Brushes.Black;
                home.Content = homeIcon;
                home.Click += (Object, RoutedEventArgs) => { Window_Loaded(sender, e); };

                PackIcon prevIcon = new PackIcon();
                prevIcon.Kind = PackIconKind.ArrowLeftBold;
                prevIcon.Background = Brushes.Black;
                prev.Content = prevIcon;
                Label espace = new Label();
                headerStack.Children.Add(home);
                headerStack.Children.Add(espace);
                headerStack.Children.Add(prev);
                prev.Click += (Object, RoutedEventArgs) => { Movie_Click(sender, e, movieTile); };
            }
            else if (nomMethod == "Plan_Prises_Click")
            {
                Button home = new Button();
                home.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                ShadowAssist.SetShadowDepth(home, ShadowDepth.Depth5);
                PackIcon homeIcon = new PackIcon();
                homeIcon.Kind = PackIconKind.Home;
                homeIcon.Background = Brushes.Black;
                home.Content = homeIcon;
                home.Click += (Object, RoutedEventArgs) => { Window_Loaded(sender, e); };

                PackIcon prevIcon = new PackIcon();
                prevIcon.Kind = PackIconKind.ArrowLeftBold;
                prevIcon.Background = Brushes.Black;
                prev.Content = prevIcon;
                Label espace = new Label();
                headerStack.Children.Add(home);
                headerStack.Children.Add(espace);
                headerStack.Children.Add(prev);
                prev.Click += (Object, RoutedEventArgs) => { Sequence_Click(sender, e, movieTile,sequenceMovie); };
            }
        }
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            recherche.Text = "";
            headerStack.Children.Clear();
            lookForV.Visibility = Visibility.Hidden;
            gr.Children.Clear();
            gr.RowDefinitions.Clear();
            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var movies = await firebase.Child(id).Child("movies").OrderByKey().OnceAsync<Movies>();
            var nb = movies.Count();
            var res = 0;
            if (nb % 4 != 0)
            {
                res = 1;
            }
            var r = (int)(nb / 4);
            var x = 0;
            while (x < r + res)
            {

                gr.RowDefinitions.Add(new RowDefinition());
                x++;
            }

            var i = 0;
            var j = 0;
            foreach (var movie in movies)
            {
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
                img.Source = new BitmapImage(new Uri("C:\\Users\\lUnA ShImO\\Documents\\Visual Studio 2015\\Projects\\PFE_WPF\\PFE_WPF\\Resources\\Chartridge046_small.jpg"));
                g.Children.Add(img);


                PackIcon ic = new PackIcon();
                ic.Width = 20;
                ic.Height = 20;
                ic.Kind = PackIconKind.Camera;
                ic.HorizontalAlignment = HorizontalAlignment.Right;
                ic.VerticalAlignment = VerticalAlignment.Bottom;

                Grid.SetRow(ic, 0);
                g.Children.Add(ic);

                StackPanel stackT = new StackPanel { };
                stackT.Orientation = Orientation.Vertical;
                Chip titre = new Chip();
                titre.HorizontalAlignment = HorizontalAlignment.Center;
                titre.FontWeight = FontWeights.Bold;
                titre.Content = movie.Key;
                titre.IconBackground = Brushes.Yellow;
                TextBlock prod = new TextBlock();
                prod.FontWeight = FontWeights.Bold;
                prod.Text = $"Production : { movie.Object.Production}";
                prod.TextAlignment = TextAlignment.Center;
                TextBlock real = new TextBlock();
                real.FontWeight = FontWeights.Bold;
                real.Text = $"Realisateur : { movie.Object.Realisateur}";
                real.TextAlignment = TextAlignment.Center;

                stackT.Children.Add(titre);
                stackT.Children.Add(prod);
                stackT.Children.Add(real);

                Grid.SetRow(stackT, 1);
                g.Children.Add(stackT);

                var sequences = await firebase.Child(id).Child("movies").Child(movie.Key).Child("Sequence").OrderByKey().OnceAsync<Movies>();
                var nbSeq = sequences.Count().ToString();

                StackPanel stackP = new StackPanel { };
                stackP.HorizontalAlignment = HorizontalAlignment.Right;
                stackP.Margin = new Thickness(8);
                stackP.Orientation = Orientation.Horizontal;
                PackIcon pop = new PackIcon();
                Style myStyle = (Style)Resources["{StaticResource MaterialDesignToolPopupBox}"];
                pop.Style = myStyle;
                pop.Kind = PackIconKind.Movie;
                pop.Padding = new Thickness(2, 0, 2, 0);
                Button more = new Button();
                more.Width = 50;
                more.Content = pop;
                var mv = movie.Key;
                more.Click += (Object, RoutedEventArgs) => { Movie_Click(sender, e, mv); };                

                Badged moreBg = new Badged();
                moreBg.Badge = nbSeq;
                moreBg.Content = more;

                stackP.Children.Add(moreBg);
                Grid.SetRow(stackP, 2);
                g.Children.Add(stackP);

                Grid grdEspace = new Grid();

                // Create Rows  
                RowDefinition row1 = new RowDefinition();
                GridLengthConverter gridLengthConverterrow1 = new GridLengthConverter();
                row1.Height = (GridLength)gridLengthConverterrow1.ConvertFrom("*");
                RowDefinition row2 = new RowDefinition();
                GridLengthConverter gridLengthConverterrow2 = new GridLengthConverter();
                row2.Height = (GridLength)gridLengthConverterrow2.ConvertFrom("Auto");

                grdEspace.RowDefinitions.Add(row1);
                grdEspace.RowDefinitions.Add(row2);

                TextBlock vide = new TextBlock();

                Card cd = new Card();
                //cd.HorizontalAlignment = HorizontalAlignment.Left;
                ShadowAssist.SetShadowDepth(cd, ShadowDepth.Depth5);
                cd.Height = 270;
                cd.Width = 200;
                cd.Content = g;

                grdEspace.Children.Add(cd);
                Grid.SetRow(vide, 1);
                grdEspace.Children.Add(vide);

                Grid.SetRow(grdEspace, j);
                Grid.SetColumn(grdEspace, i);

                gr.Children.Add(grdEspace);
                i++;
            }
        }
        private async void Movie_Click(Object sender, RoutedEventArgs e, String mv)
        {
            movieTile = mv;
            headerStack.Children.Clear();
            Previous_Button(sender, e,"Window_Loaded");
            lookForV.Visibility = Visibility.Visible;
            gr.Children.Clear();
            gr.RowDefinitions.Clear();
            //gr.ColumnDefinitions.Clear();

            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var sequences = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").OrderByKey().OnceAsync<Movies>();
            var nbS = sequences.Count();
            var res = 0;
            if (nbS % 4 != 0)
            {
                res = 1;
            }
            var r = (int)(nbS / 4);
            var x = 0;
            while (x < r + res)
            {

                gr.RowDefinitions.Add(new RowDefinition());
                x++;
            }

            var i = 0;
            var j = 0;
            foreach (var seq in sequences)
            {
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
                img.Source = new BitmapImage(new Uri("C:\\Users\\lUnA ShImO\\Source\\Repos\\WPF_PFE\\PFE_WPF\\Resources\\movie.png"));
                g.Children.Add(img);

                StackPanel stackT = new StackPanel { };
                stackT.Orientation = Orientation.Vertical;
                Chip titre = new Chip();
                titre.FontWeight = FontWeights.Bold;
                titre.Content = seq.Key;
                titre.HorizontalAlignment = HorizontalAlignment.Center;
                titre.VerticalAlignment = VerticalAlignment.Center;

                Label vide1 = new Label();
                Label vide2 = new Label();
                Label vide3 = new Label();

                stackT.Children.Add(vide1);
                stackT.Children.Add(vide2);
                stackT.Children.Add(vide3);
                stackT.Children.Add(titre);

                Grid.SetRow(stackT, 1);
                g.Children.Add(stackT);

                var plans = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").Child(seq.Key).Child("plans").OrderByKey().OnceAsync<Plan>();
                var nbPlans = plans.Count().ToString();

                StackPanel stackP = new StackPanel { };
                stackP.HorizontalAlignment = HorizontalAlignment.Right;
                stackP.Margin = new Thickness(8);
                stackP.Orientation = Orientation.Horizontal;
                PackIcon pop = new PackIcon();
                Style myStyle = (Style)Resources["{StaticResource MaterialDesignToolPopupBox}"];
                pop.Style = myStyle;
                pop.Kind = PackIconKind.Camera;
                pop.Padding = new Thickness(2, 0, 2, 0);
                Button more = new Button();
                var bc = new BrushConverter();
                more.Background = (Brush)bc.ConvertFrom("#FF5D9EA6");
                more.Content = pop;
                var sequi = seq.Key;               
                more.Click += (Object, RoutedEventArgs) => { Sequence_Click(sender, e, mv, sequi); };

                Badged moreBg = new Badged();
                moreBg.Badge = nbPlans;
                moreBg.Content = more;

                stackP.Children.Add(moreBg);
                Grid.SetRow(stackP, 2);
                g.Children.Add(stackP);

                Grid grdEspace = new Grid();

                // Create Rows  
                RowDefinition row1 = new RowDefinition();
                GridLengthConverter gridLengthConverterrow1 = new GridLengthConverter();
                row1.Height = (GridLength)gridLengthConverterrow1.ConvertFrom("*");
                RowDefinition row2 = new RowDefinition();
                GridLengthConverter gridLengthConverterrow2 = new GridLengthConverter();
                row2.Height = (GridLength)gridLengthConverterrow2.ConvertFrom("Auto");

                grdEspace.RowDefinitions.Add(row1);
                grdEspace.RowDefinitions.Add(row2);

                TextBlock vide = new TextBlock();

                Card cd = new Card();
                //cd.HorizontalAlignment = HorizontalAlignment.Left;
                ShadowAssist.SetShadowDepth(cd, ShadowDepth.Depth5);
                cd.Height = 270;
                cd.Width = 200;
                cd.Content = g;

                grdEspace.Children.Add(cd);
                Grid.SetRow(vide, 1);
                grdEspace.Children.Add(vide);

                Grid.SetRow(grdEspace, j);
                Grid.SetColumn(grdEspace, i);

                gr.Children.Add(grdEspace);
                i++;
            }
        }
        private async void Sequence_Click(object sender, RoutedEventArgs e, String mt, String sequi)
        {
            sequenceMovie = sequi;
            headerStack.Children.Clear();
            Previous_Button(sender, e, "Movie_Click");
            gr.Children.Clear();
            gr.RowDefinitions.Clear();
            //gr.ColumnDefinitions.Clear();

            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var plans = await firebase.Child(id).Child("movies").Child(mt).Child("Sequence").Child(sequi).Child("plans").OrderByKey().OnceAsync<Plan>();
            var nbP = plans.Count();
            var res = 0;
            if (nbP % 4 != 0)
            {
                res = 1;
            }
            var r = (int)(nbP / 4);
            var x = 0;
            while (x <= r + res)
            {

                gr.RowDefinitions.Add(new RowDefinition());
                x++;
            }

            var i = 0;
            var j = 0;
            foreach (var plan in plans)
            {
                if (i == 4)
                {
                    i = 0;
                    j++;
                }

                Grid gridFront = new Grid();

                // Create Rows  
                RowDefinition r1 = new RowDefinition();
                r1.Height = new GridLength(145);
                RowDefinition r2 = new RowDefinition();
                GridLengthConverter gridLengthConverter1 = new GridLengthConverter();
                r2.Height = (GridLength)gridLengthConverter1.ConvertFrom("*");
                RowDefinition r3 = new RowDefinition();
                GridLengthConverter gridLengthConverter2 = new GridLengthConverter();
                r3.Height = (GridLength)gridLengthConverter2.ConvertFrom("Auto");

                gridFront.RowDefinitions.Add(r1);
                gridFront.RowDefinitions.Add(r2);
                gridFront.RowDefinitions.Add(r3);

                Image img = new Image();
                img.Height = 140;
                img.HorizontalAlignment = HorizontalAlignment.Stretch;
                img.Source = new BitmapImage(new Uri(plan.Object.urlImageLink));
                gridFront.Children.Add(img);

                StackPanel stackT = new StackPanel { };
                stackT.Orientation = Orientation.Vertical;
                Label l1 = new Label();
                Label l2 = new Label();
                Chip titre = new Chip();
                titre.Content = sequi + " / " + plan.Key;
                titre.HorizontalAlignment = HorizontalAlignment.Center;
                titre.FontWeight = FontWeights.Bold;

                stackT.Children.Add(l1);
                stackT.Children.Add(l2);
                stackT.Children.Add(titre);

                Grid.SetRow(stackT, 1);
                gridFront.Children.Add(stackT);

                StackPanel stackP = new StackPanel { };
                stackP.HorizontalAlignment = HorizontalAlignment.Right;
                stackP.Margin = new Thickness(8);
                stackP.Orientation = Orientation.Horizontal;

                Button more = new Button();
                more.Content = "prises";

                var prises = await firebase.Child(id).Child("movies").Child(mt).Child("Sequence").Child(sequi).Child("plans").Child(plan.Key).Child("listPrise").OrderByKey().OnceAsync<Prise>();
                var nbPrises = prises.Count();

                Badged moreBg = new Badged();
                moreBg.Badge = nbPrises;
                moreBg.Content = more;

                ShadowAssist.SetShadowDepth(more, ShadowDepth.Depth5);

                var pln = plan.Key;
                more.Click += (Object, RoutedEventArgs) => { Plan_Prises_Click(more, e, mt, sequi, pln); };

                Button info = new Button();
                PackIcon infoIcon = new PackIcon();
                infoIcon.Kind = PackIconKind.Information;
                info.Content = infoIcon;
                ShadowAssist.SetShadowDepth(info, ShadowDepth.Depth5);

                var plann = plan.Object.plan;
                var camera = plan.Object.camera;
                var cardSD = plan.Object.camera;
                var decor = plan.Object.camera;
                var effetIN = plan.Object.camera;
                var effetJN = plan.Object.camera;
                var hauteur = plan.Object.camera;
                var objectif = plan.Object.camera;
                var sonOption = plan.Object.camera;
                var distance = plan.Object.camera;
                info.Click += (Object, RoutedEventArgs) => { Plan_Info_Click(more, e, mt, sequi, camera, cardSD, decor, effetIN, effetJN, hauteur, objectif, sonOption, distance,plann); };

                Label lb1 = new Label();
                Label lb2 = new Label();
                Label lb3 = new Label();
                Label lb4 = new Label();
                Label lb5 = new Label();
                Label lb6 = new Label();

                stackP.Children.Add(info);
                stackP.Children.Add(lb1);
                stackP.Children.Add(lb2);
                stackP.Children.Add(lb3);
                stackP.Children.Add(lb4);
                stackP.Children.Add(lb5);
                stackP.Children.Add(lb6);
                stackP.Children.Add(moreBg);
                Grid.SetRow(stackP, 2);
                gridFront.Children.Add(stackP);


                Grid grdEspace = new Grid();

                // Create Rows  
                RowDefinition row1 = new RowDefinition();
                GridLengthConverter gridLengthConverterrow1 = new GridLengthConverter();
                row1.Height = (GridLength)gridLengthConverterrow1.ConvertFrom("*");
                RowDefinition row2 = new RowDefinition();
                GridLengthConverter gridLengthConverterrow2 = new GridLengthConverter();
                row2.Height = (GridLength)gridLengthConverterrow2.ConvertFrom("Auto");

                grdEspace.RowDefinitions.Add(row1);
                grdEspace.RowDefinitions.Add(row2);

                TextBlock vide = new TextBlock();

                Card cd = new Card();
                //cd.HorizontalAlignment = HorizontalAlignment.Left;
                ShadowAssist.SetShadowDepth(cd, ShadowDepth.Depth5);
                cd.Height = 270;
                cd.Width = 200;
                cd.Content = gridFront;

                grdEspace.Children.Add(cd);
                Grid.SetRow(vide, 1);
                grdEspace.Children.Add(vide);

                Grid.SetRow(grdEspace, j);
                Grid.SetColumn(grdEspace, i);

                gr.Children.Add(grdEspace);
                i++;
            }
        }
        private void Plan_Info_Click(object sender, RoutedEventArgs e, String mt, String sequi, String camera, String cardSD, String decor, String effetIN, String effetJN, String hauteur, String objectif, String sonOption, String distance,String plann)
        {
            var PlanInfo = new PlanInfo();
            PlanInfo.Left = 890;
            PlanInfo.Top = 10;
            PlanInfo.Width = 480;
            PlanInfo.Height = 700;
            PlanInfo.Show();
        }
        private async void Plan_Prises_Click(object sender, RoutedEventArgs e, String mt, String sequi, String pln)
        {
            headerStack.Children.Clear();
            Previous_Button(sender, e, "Plan_Prises_Click");
            gr.Children.Clear();
            gr.RowDefinitions.Clear();
            //gr.ColumnDefinitions.Clear();

            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var prises = await firebase.Child(id).Child("movies").Child(mt).Child("Sequence").Child(sequi).Child("plans").Child(pln).Child("listPrise").OrderByKey().OnceAsync<Prise>();
            var nb = prises.Count();
            var res = 0;
            if (nb % 4 != 0)
            {
                res = 1;
            }
            var r = (int)(nb / 4);
            var x = 0;
            while (x < r + res)
            {

                gr.RowDefinitions.Add(new RowDefinition());
                x++;
            }

            var i = 0;
            var j = 0;
            foreach (var prise in prises)
            {
                if (i == 4)
                {
                    i = 0;
                    j++;
                }

                Grid g = new Grid();

                // Create Rows  
                var cp = 0;
                while (cp <= 8)
                {

                    g.RowDefinitions.Add(new RowDefinition());
                    cp++;
                }
                Chip titre = new Chip();
                titre.HorizontalAlignment = HorizontalAlignment.Center;
                titre.FontWeight = FontWeights.Bold;               
                titre.Content = prise.Key;

                ColorZone colorz = new ColorZone();
                colorz.Mode = ColorZoneMode.Accent;
                colorz.Content = titre;              

                Grid.SetRow(colorz, 0);
                g.Children.Add(colorz);

                Chip note = new Chip();
                note.Width = 200;

                RatingBar noteRt = new RatingBar();
                noteRt.HorizontalAlignment = HorizontalAlignment.Center;
                noteRt.Max = 3;
                if (prise.Object.note == "exellente")
                {
                    noteRt.Value = 3;
                }
                else if (prise.Object.note == "moyenne")
                {
                    noteRt.Value = 2;
                }
                else
                {
                    noteRt.Value = 1;
                }
                note.Content = noteRt;

                Grid.SetRow(note, 1);
                g.Children.Add(note);

                Chip commentaire = new Chip();
                commentaire.Width = 200;
                commentaire.Content = "commentaire :" + prise.Object.commentaire;

                Grid.SetRow(commentaire, 2);
                g.Children.Add(commentaire);

                Chip diaff = new Chip();
                diaff.Width = 200;
                diaff.Content = "diaff :" + prise.Object.diaff;

                Grid.SetRow(diaff, 3);
                g.Children.Add(diaff);

                Chip filtre = new Chip();
                filtre.Width = 200;
                filtre.Content = "filtre :" + prise.Object.filtre;

                Grid.SetRow(filtre, 4);
                g.Children.Add(filtre);

                Chip minutage = new Chip();
                minutage.Width = 200;
                minutage.Content = "minutage :" + prise.Object.minutage;

                Grid.SetRow(minutage, 5);
                g.Children.Add(minutage);

                Chip taille = new Chip();
                taille.Width = 200;
                taille.Content = "taille :" + prise.Object.taille;

                Grid.SetRow(taille, 6);
                g.Children.Add(taille);

                Chip unite = new Chip();
                unite.Width = 200;
                unite.Content = "unite :" + prise.Object.unite;

                Grid.SetRow(unite, 7);
                g.Children.Add(unite);

                ScrollViewer scrollPrise = new ScrollViewer();
                scrollPrise.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollPrise.Height = 270;
                scrollPrise.Content = g;

                Grid grdEspace = new Grid();

                // Create Rows  
                RowDefinition row1 = new RowDefinition();
                GridLengthConverter gridLengthConverterrow1 = new GridLengthConverter();
                row1.Height = (GridLength)gridLengthConverterrow1.ConvertFrom("*");
                RowDefinition row2 = new RowDefinition();
                GridLengthConverter gridLengthConverterrow2 = new GridLengthConverter();
                row2.Height = (GridLength)gridLengthConverterrow2.ConvertFrom("Auto");

                grdEspace.RowDefinitions.Add(row1);
                grdEspace.RowDefinitions.Add(row2);

                TextBlock vide = new TextBlock();

                Card cd = new Card();
                //cd.HorizontalAlignment = HorizontalAlignment.Left;
                ShadowAssist.SetShadowDepth(cd, ShadowDepth.Depth5);
                cd.Height = 270;
                cd.Width = 200;
                cd.Content = scrollPrise;


                grdEspace.Children.Add(cd);
                Grid.SetRow(vide, 1);
                grdEspace.Children.Add(vide);

                Grid.SetRow(grdEspace, j);
                Grid.SetColumn(grdEspace, i);

                gr.Children.Add(grdEspace);
                i++;
            }
        }
    }
}
