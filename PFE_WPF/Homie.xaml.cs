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
            while (x < r + res)
            {

                gr.RowDefinitions.Add(new RowDefinition());
                x++;
            }

            var i = 0;
            var j = 0;
            foreach (var dino in dinos)
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

                Grid grdEspace = new Grid();

                // Create Rows  
                RowDefinition row1Front = new RowDefinition();
                GridLengthConverter gridLengthConverterrow1 = new GridLengthConverter();
                row1Front.Height = (GridLength)gridLengthConverterrow1.ConvertFrom("*");
                RowDefinition row2Front = new RowDefinition();
                GridLengthConverter gridLengthConverterrow2 = new GridLengthConverter();
                row2Front.Height = (GridLength)gridLengthConverterrow2.ConvertFrom("Auto");

                grdEspace.RowDefinitions.Add(row1Front);
                grdEspace.RowDefinitions.Add(row2Front);

                TextBlock t = new TextBlock();
                t.Text = "";

                

                Card cd = new Card();
                //cd.HorizontalAlignment = HorizontalAlignment.Left;
                ShadowAssist.SetShadowDepth(cd, ShadowDepth.Depth5);
                cd.Height = 270;
                cd.Width = 200;
                cd.Content = g;

                grdEspace.Children.Add(cd);
                Grid.SetRow(t,1);
                grdEspace.Children.Add(t);

                Grid.SetRow(grdEspace, j);
                Grid.SetColumn(grdEspace, i);

                gr.Children.Add(grdEspace);


                i++;
                //MessageBox.Show ( $"Titre est { dino.Key} : production est { dino.Object.Production} 
                //et realisateur est { dino.Object.Realisateur}.");           
            }
        }
        private async void Movie_Click(object sender, RoutedEventArgs e, String mt)
        {
            gr.Children.Clear();
            gr.RowDefinitions.Clear();
            gr.ColumnDefinitions.Clear();

            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var sequences = await firebase.Child(id).Child("movies").Child(mt).Child("Sequence").OrderByKey().OnceAsync<Movies>();
            foreach (var seq in sequences)
            {
                //MessageBox.Show(seq.Key);
                var plans = await firebase.Child(id).Child("movies").Child(mt).Child("Sequence").Child(seq.Key).Child("plans").OrderByKey().OnceAsync<Plan>();
                var nb = plans.Count();
                var res = 0;
                var r = (int)(nb / 2);
                var x = 0;
                while (x <= r + res)
                {

                    gr.RowDefinitions.Add(new RowDefinition());
                    x++;
                   
                }
                // Create Columns
                ColumnDefinition gridCol1 = new ColumnDefinition();
                ColumnDefinition gridCol2 = new ColumnDefinition();
                gr.ColumnDefinitions.Add(gridCol1);
                gr.ColumnDefinitions.Add(gridCol2);

                var i = 0;
                var j = 0;
                foreach (var pln in plans)
                {
                    if (i == 2)
                    {
                        i = 0;
                        j++;
                    }
                    Grid gridFinal = new Grid();
                    StackPanel stack = new StackPanel();

                    /////////// front ///////////////////////

                    Grid gridFront = new Grid();
                    gridFront.ShowGridLines = true;
                    gridFront.Width = 390;
                    gridFront.Height = 340;

                    // Create Rows  
                    RowDefinition row1Front = new RowDefinition();
                    GridLengthConverter gridLengthConverterrow1 = new GridLengthConverter();
                    row1Front.Height = (GridLength)gridLengthConverterrow1.ConvertFrom("*");
                    RowDefinition row2Front = new RowDefinition();
                    GridLengthConverter gridLengthConverterrow2 = new GridLengthConverter();
                    row2Front.Height = (GridLength)gridLengthConverterrow2.ConvertFrom("*");

                    gridFront.RowDefinitions.Add(row1Front);
                    gridFront.RowDefinitions.Add(row2Front);


                    // Create Columns
                    ColumnDefinition colFront1 = new ColumnDefinition();
                    colFront1.Width = new GridLength(120);
                    ColumnDefinition colFront2 = new ColumnDefinition();
                    GridLengthConverter gridLengthConvertercol = new GridLengthConverter();
                    colFront2.Width = (GridLength)gridLengthConvertercol.ConvertFrom("Auto");
                    

                    gridFront.ColumnDefinitions.Add(colFront1);
                    gridFront.ColumnDefinitions.Add(colFront2);

                    Flipper front = new Flipper();
                    front.Margin = new Thickness(4, 4, 0, 0);
                    front.FrontContent = gridFront;

                    Image img = new Image();
                    img.HorizontalAlignment = HorizontalAlignment.Stretch;
                    img.Source = new BitmapImage(new Uri(pln.Object.urlImageLink));

                    //ColorZone colorzoneFront = new ColorZone();
                    //colorzoneFront.Mode = ColorZoneMode.PrimaryLight;
                    //colorzoneFront.VerticalAlignment = VerticalAlignment.Stretch;

                    //colorzoneFront.Content = img;


                    gridFront.Children.Add(img);

                    StackPanel SeqPlan = new StackPanel();
                    SeqPlan.Orientation = Orientation.Vertical;
                    SeqPlan.HorizontalAlignment = HorizontalAlignment.Center;
                    SeqPlan.VerticalAlignment = VerticalAlignment.Center;

                    Button more = new Button();
                    //Style moreStyle = (Style)Resources["{DynamicResource MaterialDesignFloatingActionMiniButton}"];
                    //more.Style = moreStyle;
                    more.Command = Flipper.FlipCommand;
                    more.Width = 70;
                    more.HorizontalAlignment = HorizontalAlignment.Center;
                    more.Content = "More!";
                    ShadowAssist.SetShadowDepth(more, ShadowDepth.Depth5);


                    Chip titre = new Chip();
                    titre.Content = " Seq " + pln.Object.seq + " / Pln " + pln.Key;
                    titre.HorizontalAlignment = HorizontalAlignment.Center;
                    Label lb = new Label();
                    
                    SeqPlan.Children.Add(titre);
                    SeqPlan.Children.Add(lb);
                    SeqPlan.Children.Add(more);


                    Grid.SetColumn(SeqPlan, 0);
                    Grid.SetRow(SeqPlan, 1);
                    gridFront.Children.Add(SeqPlan);


                    ///////////////////////// back ///////////////////
                    Grid gridBack = new Grid();
                    gridBack.Width = 400;
                    gridBack.Height = 350;

                    // Create Rows  
                    RowDefinition row1Back = new RowDefinition();
                    GridLengthConverter gridLengthConverter1B = new GridLengthConverter();
                    row1Back.Height = (GridLength)gridLengthConverter1B.ConvertFrom("Auto");
                    RowDefinition row2Back = new RowDefinition();
                    GridLengthConverter gridLengthConverter2B = new GridLengthConverter();
                    row2Back.Height = (GridLength)gridLengthConverter2B.ConvertFrom("*");

                    gridBack.RowDefinitions.Add(row1Back);
                    gridBack.RowDefinitions.Add(row2Back);

                    // Create Columns
                    ColumnDefinition colBack1 = new ColumnDefinition();
                    ColumnDefinition colBack2 = new ColumnDefinition();
                    gridBack.ColumnDefinitions.Add(colBack1);
                    gridBack.ColumnDefinitions.Add(colBack2);

                    front.Padding = new Thickness(6);
                    front.BackContent = gridBack;

                    ColorZone colorzoneBack = new ColorZone();
                    colorzoneBack.Mode = ColorZoneMode.Accent;


                    StackPanel details = new StackPanel();
                    details.Orientation = Orientation.Horizontal;

                    Button GoBack = new Button();
                    Style GoBackeStyle = (Style)Resources["{StaticResource MaterialDesignToolForegroundButton}"];
                    GoBack.Style = GoBackeStyle;
                    GoBack.Command = Flipper.FlipCommand;
                    GoBack.HorizontalAlignment = HorizontalAlignment.Left;

                    PackIcon iconBack = new PackIcon();
                    iconBack.Kind = PackIconKind.ArrowLeft;
                    iconBack.HorizontalAlignment = HorizontalAlignment.Right;

                    TextBlock retour = new TextBlock();
                    retour.Margin = new Thickness(8, 0, 0, 0);
                    retour.VerticalAlignment = VerticalAlignment.Center;
                    retour.Text = pln.Object.seq + " / " + pln.Key;

                    TextBox camera = new TextBox();
                    camera.Text = pln.Object.camera;

                    TextBox cardSD = new TextBox();
                    cardSD.Text = pln.Object.cardSd;

                    TextBox decor = new TextBox();
                    decor.Text = pln.Object.decor;

                    TextBox date = new TextBox();
                    date.Text = "20/05/2017";

                    TextBox description = new TextBox();
                    description.Text = pln.Object.description;

                    TextBox effetIN = new TextBox();
                    effetIN.Text = pln.Object.effetIN;

                    TextBox effetJN = new TextBox();
                    effetJN.Text = pln.Object.effetJN;

                    TextBox distance = new TextBox();
                    distance.Text = pln.Object.distance;

                    TextBox hauteur = new TextBox();
                    hauteur.Text = pln.Object.hauteur;

                    TextBox dialogue = new TextBox();
                    dialogue.Text = pln.Object.dialogue;

                    TextBox objectif = new TextBox();
                    objectif.Text = pln.Object.objectif;

                    TextBox sonOption = new TextBox();
                    sonOption.Text = pln.Object.sonOption;

                    StackPanel infoPlan = new StackPanel();
                    infoPlan.Children.Add(camera);
                    infoPlan.Children.Add(cardSD);
                    infoPlan.Children.Add(decor);
                    infoPlan.Children.Add(date);
                    infoPlan.Children.Add(description);
                    infoPlan.Children.Add(effetIN);
                    infoPlan.Children.Add(effetJN);
                    infoPlan.Children.Add(distance);
                    infoPlan.Children.Add(hauteur);
                    infoPlan.Children.Add(dialogue);
                    infoPlan.Children.Add(objectif);
                    infoPlan.Children.Add(sonOption);


                    GoBack.Content = iconBack;
                    details.Children.Add(GoBack);
                    details.Children.Add(retour);

                    colorzoneBack.Content = details;

                    gridBack.Children.Add(colorzoneBack);
                    gridBack.Children.Add(infoPlan);

                    stack.Children.Add(front);


                    gridFinal.Children.Add(stack);

                    Card cd = new Card();
                     

                    //BrushConverter bc = new BrushConverter();
                    //Brush brush = (Brush)bc.ConvertFrom("{DynamicResource PrimaryHueDarkBrush}");
                   //brush.Freeze();
                   // cd.Background = brush;
                    cd.Height = 350;
                    cd.Width = 400;
                    ShadowAssist.SetShadowDepth(cd, ShadowDepth.Depth5);
                    cd.Content = gridFinal;

                    Grid.SetRow(cd, j);
                    Grid.SetColumn(cd, i);

                    gr.Children.Add(cd);
                    i++;
                }
            }
        }
    }
}
