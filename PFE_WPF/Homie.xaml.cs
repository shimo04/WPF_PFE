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
                var r = (int)(nb / 3);
                var x = 0;
                while (x <= r + res)
                {

                    gr.RowDefinitions.Add(new RowDefinition());
                    x++;
                   
                }
                // Create Columns
                ColumnDefinition gridCol1 = new ColumnDefinition();
                ColumnDefinition gridCol2 = new ColumnDefinition();
                ColumnDefinition gridCol3 = new ColumnDefinition();
                gr.ColumnDefinitions.Add(gridCol1);
                gr.ColumnDefinitions.Add(gridCol2);
                gr.ColumnDefinitions.Add(gridCol3);

                var i = 0;
                var j = 0;
                foreach (var pln in plans)
                {
                    if (i == 3)
                    {
                        i = 0;
                        j++;
                    }
                    Grid gridFinal = new Grid();
                    StackPanel stack = new StackPanel();

                    /////////// front ///////////////////////

                    Grid gridFront = new Grid();
                    //gridFront.ShowGridLines = true;
                    gridFront.Width = 250;
    

                    // Create Rows  
                    RowDefinition row1Front = new RowDefinition();
                    row1Front.Height = new GridLength(250);

                    gridFront.RowDefinitions.Add(row1Front);             

                    Flipper front = new Flipper();
                    front.Margin = new Thickness(4, 4, 0, 0);
                    front.FrontContent = gridFront;

                    Image img = new Image();
                    img.HorizontalAlignment = HorizontalAlignment.Stretch;
                    img.VerticalAlignment = VerticalAlignment.Stretch;
                    img.Source = new BitmapImage(new Uri(pln.Object.urlImageLink));
                    img.Height = 270;

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
                    more.VerticalAlignment = VerticalAlignment.Center;
                    more.Content = "More!";
                    ShadowAssist.SetShadowDepth(more, ShadowDepth.Depth5);


                    Chip titre = new Chip();
                    titre.Content = " Seq " + pln.Object.seq + " / Pln " + pln.Key;
                    titre.HorizontalAlignment = HorizontalAlignment.Center;
                    Label lb = new Label();
                    
                    SeqPlan.Children.Add(titre);
                    SeqPlan.Children.Add(lb);
                    SeqPlan.Children.Add(more);

                    Grid.SetRow(SeqPlan, 0);
                    gridFront.Children.Add(SeqPlan);

                    ///////////////////////// back ///////////////////
                    Grid gridBack = new Grid();
                    //gridBack.ShowGridLines = true;
                    gridBack.Width = 250;


                    // Create Rows  
                    var cp = 0;
                    while (cp <= 17)
                    {

                        gridBack.RowDefinitions.Add(new RowDefinition());
                        cp++;
                    }
                    ColumnDefinition Col1 = new ColumnDefinition();
                    ColumnDefinition Col2 = new ColumnDefinition();
                    Col2.Width = new GridLength(10);
                    gridBack.ColumnDefinitions.Add(Col1);
                    gridBack.ColumnDefinitions.Add(Col2);

                    ScrollViewer scrollPlan = new ScrollViewer();
                    scrollPlan.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    scrollPlan.Height = 270;
                    scrollPlan.Content = gridBack;

                    front.Padding = new Thickness(6);
                    front.BackContent = scrollPlan;

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

                    GoBack.Content = iconBack;
                    details.Children.Add(GoBack);
                    details.Children.Add(retour);

                    colorzoneBack.Content = details;
                    Grid.SetColumnSpan(colorzoneBack, 2);
                    gridBack.Children.Add(colorzoneBack);

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    Expander cameraExpander = new Expander();
                    cameraExpander.Header = "Camera";
                    cameraExpander.HorizontalAlignment = HorizontalAlignment.Stretch;                   
                    TextBlock camera = new TextBlock();
                    camera.Text = pln.Object.camera;
                    cameraExpander.Content = camera;

                    Grid.SetRow(cameraExpander, 1);
                    gridBack.Children.Add(cameraExpander);
                    //*****************************************************
                    Expander cardSDExpander = new Expander();
                    cardSDExpander.Header = "cardSD";
                    cardSDExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock cardSD = new TextBlock();
                    cardSD.Text = pln.Object.cardSd;
                    cardSDExpander.Content = cardSD;

                    Grid.SetRow(cardSDExpander, 2);
                    gridBack.Children.Add(cardSDExpander);
                    //***********************************
                    Expander decorExpander = new Expander();
                    decorExpander.Header = "decor";
                    decorExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock decor = new TextBlock();
                    decor.Text = pln.Object.decor;
                    decorExpander.Content = decor;

                    Grid.SetRow(decorExpander, 3);
                    gridBack.Children.Add(decorExpander);
                    //***********************
                    Expander dateExpander = new Expander();
                    dateExpander.Header = "date";
                    dateExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock date = new TextBlock();
                    date.Text = "20/05/2017";
                    dateExpander.Content = date;

                    Grid.SetRow(dateExpander, 4);
                    gridBack.Children.Add(dateExpander);
                    //************************************
                    Expander descriptionExpander = new Expander();
                    descriptionExpander.Header = "description";
                    descriptionExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock description = new TextBlock();
                    description.Text = pln.Object.description;
                    descriptionExpander.Content = description;

                    Grid.SetRow(descriptionExpander, 5);
                    gridBack.Children.Add(descriptionExpander);
                    //**********************************
                    Expander dialogueExpander = new Expander();
                    dialogueExpander.Header = "dialogue";
                    dialogueExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock dialogue = new TextBlock();
                    dialogue.Text = pln.Object.dialogue;
                    dialogueExpander.Content = dialogue;

                    Grid.SetRow(dialogueExpander, 6);
                    gridBack.Children.Add(dialogueExpander);
                    //********************
                    Expander effetINExpander = new Expander();
                    effetINExpander.Header = "effetIN";
                    effetINExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock effetIN = new TextBlock();
                    effetIN.Text = pln.Object.effetIN;
                    effetINExpander.Content = effetIN;

                    Grid.SetRow(effetINExpander, 7);
                    gridBack.Children.Add(effetINExpander);
                    //****************************************************
                    Expander effetJNExpander = new Expander();
                    effetJNExpander.Header = "effetJN";
                    effetJNExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock effetJN = new TextBlock();
                    effetJN.Text = pln.Object.effetJN;
                    effetJNExpander.Content = effetJN;

                    Grid.SetRow(effetJNExpander, 8);
                    gridBack.Children.Add(effetJNExpander);
                    //******************************************************
                    Expander distanceExpander = new Expander();
                    distanceExpander.Header = "distance";
                    distanceExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock distance = new TextBlock();
                    distance.Text = pln.Object.distance;
                    distanceExpander.Content = distance;

                    Grid.SetRow(distanceExpander, 9);
                    gridBack.Children.Add(distanceExpander);
                    //*********************************************
                    Expander hauteurExpander = new Expander();
                    hauteurExpander.Header = "hauteur";
                    hauteurExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock hauteur = new TextBlock();
                    hauteur.Text = pln.Object.hauteur;             
                    hauteurExpander.Content = hauteur;

                    Grid.SetRow(hauteurExpander, 10);
                    gridBack.Children.Add(hauteurExpander);
                    //*******************************************************
                    Expander objectifExpander = new Expander();
                    objectifExpander.Header = "objectif";
                    objectifExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock objectif = new TextBlock();
                    objectif.Text = pln.Object.objectif;
                    objectifExpander.Content = objectif;

                    Grid.SetRow(objectifExpander, 11);
                    gridBack.Children.Add(objectifExpander);
                    //*********************************************************
                    Expander sonOptionExpander = new Expander();
                    sonOptionExpander.Header = "sonOption";
                    sonOptionExpander.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBlock sonOption = new TextBlock();
                    sonOption.Text = pln.Object.sonOption;
                    sonOptionExpander.Content = sonOption;

                    Grid.SetRow(sonOptionExpander, 12);
                    gridBack.Children.Add(sonOptionExpander);
                    //*****************************
                    Label vide = new Label();
                    Grid.SetRow(vide, 13);
                    gridBack.Children.Add(vide);
                    //********************************************************
                    Button Prises = new Button();
                    Prises.Content = "Prises";
                    Prises.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetRow(Prises, 14);
                    gridBack.Children.Add(Prises);
                    //************************************************************
                    Label vide2 = new Label();
                    Grid.SetRow(vide2, 15);
                    gridBack.Children.Add(vide2);
                    //*******************************************************
                    Label vide3 = new Label();
                    Grid.SetRow(vide3, 16);
                    gridBack.Children.Add(vide3);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





                    stack.Children.Add(front);

                    gridFinal.Children.Add(stack);

                    

                    Grid grdEspace = new Grid();

                    // Create Rows  
                    RowDefinition row1 = new RowDefinition();
                    GridLengthConverter gridLengthConvert1 = new GridLengthConverter();
                    row1.Height = (GridLength)gridLengthConvert1.ConvertFrom("*");
                    RowDefinition row2 = new RowDefinition();
                    GridLengthConverter gridLengthConvert2 = new GridLengthConverter();
                    row2.Height = (GridLength)gridLengthConvert2.ConvertFrom("Auto");
                    RowDefinition row3 = new RowDefinition();
                    GridLengthConverter gridLengthConverter3 = new GridLengthConverter();
                    row3.Height = (GridLength)gridLengthConverter3.ConvertFrom("Auto");

                    grdEspace.RowDefinitions.Add(row1);
                    grdEspace.RowDefinitions.Add(row2);
                    grdEspace.RowDefinitions.Add(row3);

                    TextBlock t = new TextBlock();
                    t.Text = "";
                    TextBlock t2 = new TextBlock();
                    t2.Text = "";

                    Card cd = new Card();

                    //cd.Background = Brushes.Transparent;
                    cd.Height = 270;
                    cd.Width = 250;
                    ShadowAssist.SetShadowDepth(cd, ShadowDepth.Depth5);
                    cd.Content = gridFinal;

                    grdEspace.Children.Add(t2);
                    Grid.SetRow(cd, 1);
                    grdEspace.Children.Add(cd);
                    Grid.SetRow(t, 2);
                    grdEspace.Children.Add(t);

                    Grid.SetRow(grdEspace, j);
                    Grid.SetColumn(grdEspace, i);

                    gr.Children.Add(grdEspace);
                    i++;
                }
            }
        }
    }
}
