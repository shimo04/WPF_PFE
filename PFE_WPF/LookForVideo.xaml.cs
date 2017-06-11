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
using MaterialDesignThemes.Wpf;
using System.Data;

namespace PFE_WPF
{
    /// we code for FUN
 
    public partial class LookForVideo : Window
    {
        public List<string> listPr = new List<string>();
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
            listPr.Clear();
            gr.Children.Clear();
            gr.RowDefinitions.Clear();
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

            var nbRes = 0;

            if (ComboBoxSequences.SelectedItem.ToString() != "tout" && ComboBoxPlans.SelectedItem.ToString() != "tout")
            {
                listPr.Clear();
                gr.Children.Clear();
                gr.RowDefinitions.Clear();
                var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
                var prises = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").Child(ComboBoxSequences.SelectedItem.ToString()).Child("plans").Child(ComboBoxPlans.SelectedItem.ToString()).Child("listPrise").OrderByKey().OnceAsync<Prise>();
                var nbPl = prises.Count();
                foreach (var prise in prises)
                {
                    var namePrise = prise.Object.nomPrise + ".MP4";
                    if (prise.Object.note == res)
                    {
                        listPr.Add(namePrise);
                        MessageBox.Show(namePrise);
                        nbRes++;
                    }
                }
                if (nbRes != 0)
                {
                    var cpt = 0;
                    while (cpt < listPr.Count())
                    {
                        gr.RowDefinitions.Add(new RowDefinition());
                        cpt++;
                    }
                    System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
                    browse.RootFolder = Environment.SpecialFolder.Desktop;
                    browse.Description = " +++ selectionner un dossier +++";
                    browse.ShowNewFolderButton = false;
                    var i = 0;
                    var test = false;

                    if (browse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string[] files = Directory.GetFiles(browse.SelectedPath); ;
                        foreach (String pr in listPr)
                        {
                            foreach (String s in files)
                            {
                                if (System.IO.Path.GetFileName(s) == pr)
                                {

                                    StackPanel stackMedia = new StackPanel();
                                    stackMedia.HorizontalAlignment = HorizontalAlignment.Center;
                                    stackMedia.VerticalAlignment = VerticalAlignment.Center;
                                    stackMedia.Orientation = Orientation.Vertical;

                                    MediaElement myMedia = new MediaElement();
                                    myMedia.Source = new Uri(s, UriKind.RelativeOrAbsolute);
                                    myMedia.LoadedBehavior = MediaState.Manual;
                                    myMedia.Width = 440;

                                    StackPanel stackMediaButtons = new StackPanel();
                                    stackMediaButtons.Orientation = Orientation.Horizontal;
                                    stackMediaButtons.HorizontalAlignment = HorizontalAlignment.Center;
                                    stackMediaButtons.VerticalAlignment = VerticalAlignment.Center;

                                    StackPanel stackEnteteMedia = new StackPanel();
                                    stackEnteteMedia.Orientation = Orientation.Horizontal;
                                    stackEnteteMedia.HorizontalAlignment = HorizontalAlignment.Center;
                                    stackEnteteMedia.VerticalAlignment = VerticalAlignment.Center;

                                    Chip EnteteInfoMedia = new Chip();
                                    EnteteInfoMedia.Content = "Nom Prise : " + pr + "          Details : " + ComboBoxSequences.SelectedItem.ToString() + " / " + ComboBoxPlans.SelectedItem.ToString();
                                    stackEnteteMedia.Children.Add(EnteteInfoMedia);


                                    PackIcon playIcon = new PackIcon();
                                    playIcon.Kind = PackIconKind.Play;
                                    Button play = new Button();
                                    play.Content = playIcon;
                                    play.Click += (Object, RoutedEventArgs) => { mediaPlay(sender, e, myMedia); };

                                    PackIcon pauseIcon = new PackIcon();
                                    pauseIcon.Kind = PackIconKind.Pause;
                                    Button pause = new Button();
                                    pause.Content = pauseIcon;
                                    pause.Click += (Object, RoutedEventArgs) => { mediaPause(sender, e, myMedia); };

                                    PackIcon stopIcon = new PackIcon();
                                    stopIcon.Kind = PackIconKind.Stop;
                                    Button stop = new Button();
                                    stop.Content = stopIcon;
                                    stop.Click += (Object, RoutedEventArgs) => { mediaStop(sender, e, myMedia); };

                                    PackIcon muteIcon = new PackIcon();
                                    muteIcon.Kind = PackIconKind.MusicNoteOff;
                                    Button mute = new Button();
                                    mute.Content = muteIcon;
                                    mute.Click += (Object, RoutedEventArgs) => { mediaMute(sender, e, myMedia, mute); };

                                    ColorZone PriseZone = new ColorZone();
                                    PriseZone.Background = Brushes.Green;

                                    Separator espace1 = new Separator();
                                    Separator espace2 = new Separator();
                                    Separator espace3 = new Separator();
                                    Label lb1 = new Label();
                                    Label lb2 = new Label();
                                    Label lb3 = new Label();
                                    Label lb4 = new Label();
                                    Label lb5 = new Label();

                                    stackMediaButtons.Children.Add(play);
                                    stackMediaButtons.Children.Add(lb1);
                                    stackMediaButtons.Children.Add(pause);
                                    stackMediaButtons.Children.Add(lb2);
                                    stackMediaButtons.Children.Add(stop);
                                    stackMediaButtons.Children.Add(lb3);
                                    stackMediaButtons.Children.Add(mute);


                                    stackMedia.Children.Add(espace1);
                                    stackMedia.Children.Add(stackEnteteMedia);
                                    stackMedia.Children.Add(espace2);
                                    stackMedia.Children.Add(myMedia);
                                    stackMedia.Children.Add(espace3);
                                    stackMedia.Children.Add(lb4);
                                    stackMedia.Children.Add(stackMediaButtons);
                                    stackMedia.Children.Add(lb5);

                                    PriseZone.Content = stackMedia;
                                    Grid.SetRow(PriseZone, i);
                                    gr.Children.Add(PriseZone);
                                    i++;
                                    test = true;
                                    break;
                                }
                                else
                                    test = false;
                            }
                            if (test == false)
                            {
                                StackPanel stackErreur = new StackPanel();
                                stackErreur.HorizontalAlignment = HorizontalAlignment.Center;
                                stackErreur.VerticalAlignment = VerticalAlignment.Center;
                                stackErreur.Orientation = Orientation.Vertical;

                                //MessageBox.Show("no" + pr);
                                ColorZone ErreurZone = new ColorZone();
                                ErreurZone.Background = Brushes.Red;

                                StackPanel stackEnteteErreur = new StackPanel();
                                stackEnteteErreur.HorizontalAlignment = HorizontalAlignment.Center;
                                stackEnteteErreur.VerticalAlignment = VerticalAlignment.Center;
                                stackEnteteErreur.Orientation = Orientation.Vertical;

                                Chip EnteteInfoErreur = new Chip();
                                EnteteInfoErreur.Content = "Nom Prise : " + pr + "          Details : " + ComboBoxSequences.SelectedItem.ToString() + " / " + ComboBoxPlans.SelectedItem.ToString() + "   N'est pas trouvée";
                                stackEnteteErreur.Children.Add(EnteteInfoErreur);

                                StackPanel stackP = new StackPanel { };
                                stackP.HorizontalAlignment = HorizontalAlignment.Center;
                                stackP.Margin = new Thickness(8);
                                stackP.Orientation = Orientation.Horizontal;

                                Button rechercher = new Button();
                                rechercher.Content = "rechercher";
                                rechercher.Click += (Object, RoutedEventArgs) => { RechercherVideo(sender, e, pr, i, ErreurZone); };

                                PackIcon ic = new PackIcon();
                                ic.Kind = PackIconKind.Refresh;

                                Badged rechercherBg = new Badged();
                                rechercherBg.HorizontalAlignment = HorizontalAlignment.Center;
                                rechercherBg.VerticalAlignment = VerticalAlignment.Center;
                                rechercherBg.Badge = ic;
                                rechercherBg.Content = rechercher;

                                stackP.Children.Add(rechercherBg);

                                stackEnteteErreur.Children.Add(stackP);

                                stackErreur.Children.Add(stackEnteteErreur);
                                ErreurZone.Content = stackErreur;
                                Grid.SetRow(ErreurZone, i);
                                gr.Children.Add(ErreurZone);
                                i++;
                            }

                        }
                    }
                }
                else
                    MessageBox.Show("cette qualité existe pas");
            }
            else if (ComboBoxSequences.SelectedItem.ToString() == "tout")
            {
                listPr.Clear();
                gr.Children.Clear();
                gr.RowDefinitions.Clear();

                var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
                var sequences = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").OrderByKey().OnceAsync<Movies>();
                foreach (var seq in sequences)
                {
                    var plans = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").Child(seq.Key).Child("plans").OrderByKey().OnceAsync<Plan>();
                    foreach (var plan in plans)
                    {
                        var prises = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").Child(seq.Key).Child("plans").Child(plan.Key).Child("listPrise").OrderByKey().OnceAsync<Prise>();
                        foreach (var prise in prises)
                        {
                            var namePrise = prise.Object.nomPrise + ".MP4";
                            if (prise.Object.note == res)
                            {
                                listPr.Add(namePrise);
                                nbRes++;
                            }
                        }
                    }
                }
                if (nbRes != 0)
                {
                    var cpt = 0;
                    while (cpt < listPr.Count())
                    {
                        gr.RowDefinitions.Add(new RowDefinition());
                        cpt++;
                    }
                    System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
                    browse.RootFolder = Environment.SpecialFolder.Desktop;
                    browse.Description = " +++ selectionner un dossier +++";
                    browse.ShowNewFolderButton = false;
                    var i = 0;
                    var test = false;

                    if (browse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string[] files = Directory.GetFiles(browse.SelectedPath); ;
                        foreach (String pr in listPr)
                        {
                            foreach (String s in files)
                            {
                                if (System.IO.Path.GetFileName(s) == pr)
                                {

                                    StackPanel stackMedia = new StackPanel();
                                    stackMedia.HorizontalAlignment = HorizontalAlignment.Center;
                                    stackMedia.VerticalAlignment = VerticalAlignment.Center;
                                    stackMedia.Orientation = Orientation.Vertical;

                                    MediaElement myMedia = new MediaElement();
                                    myMedia.Source = new Uri(s, UriKind.RelativeOrAbsolute);
                                    myMedia.LoadedBehavior = MediaState.Manual;
                                    myMedia.Width = 440;

                                    StackPanel stackMediaButtons = new StackPanel();
                                    stackMediaButtons.Orientation = Orientation.Horizontal;
                                    stackMediaButtons.HorizontalAlignment = HorizontalAlignment.Center;
                                    stackMediaButtons.VerticalAlignment = VerticalAlignment.Center;

                                    StackPanel stackEnteteMedia = new StackPanel();
                                    stackEnteteMedia.Orientation = Orientation.Horizontal;
                                    stackEnteteMedia.HorizontalAlignment = HorizontalAlignment.Center;
                                    stackEnteteMedia.VerticalAlignment = VerticalAlignment.Center;

                                    Chip EnteteInfoMedia = new Chip();
                                    EnteteInfoMedia.Content = "Nom Prise : " + pr + "          Details : " + ComboBoxSequences.SelectedItem.ToString() + " / " + ComboBoxPlans.SelectedItem.ToString();
                                    stackEnteteMedia.Children.Add(EnteteInfoMedia);


                                    PackIcon playIcon = new PackIcon();
                                    playIcon.Kind = PackIconKind.Play;
                                    Button play = new Button();
                                    play.Content = playIcon;
                                    play.Click += (Object, RoutedEventArgs) => { mediaPlay(sender, e, myMedia); };

                                    PackIcon pauseIcon = new PackIcon();
                                    pauseIcon.Kind = PackIconKind.Pause;
                                    Button pause = new Button();
                                    pause.Content = pauseIcon;
                                    pause.Click += (Object, RoutedEventArgs) => { mediaPause(sender, e, myMedia); };

                                    PackIcon stopIcon = new PackIcon();
                                    stopIcon.Kind = PackIconKind.Stop;
                                    Button stop = new Button();
                                    stop.Content = stopIcon;
                                    stop.Click += (Object, RoutedEventArgs) => { mediaStop(sender, e, myMedia); };

                                    PackIcon muteIcon = new PackIcon();
                                    muteIcon.Kind = PackIconKind.MusicNoteOff;
                                    Button mute = new Button();
                                    mute.Content = muteIcon;
                                    mute.Click += (Object, RoutedEventArgs) => { mediaMute(sender, e, myMedia, mute); };

                                    ColorZone PriseZone = new ColorZone();
                                    PriseZone.Background = Brushes.Green;

                                    Separator espace1 = new Separator();
                                    Separator espace2 = new Separator();
                                    Separator espace3 = new Separator();
                                    Label lb1 = new Label();
                                    Label lb2 = new Label();
                                    Label lb3 = new Label();
                                    Label lb4 = new Label();
                                    Label lb5 = new Label();

                                    stackMediaButtons.Children.Add(play);
                                    stackMediaButtons.Children.Add(lb1);
                                    stackMediaButtons.Children.Add(pause);
                                    stackMediaButtons.Children.Add(lb2);
                                    stackMediaButtons.Children.Add(stop);
                                    stackMediaButtons.Children.Add(lb3);
                                    stackMediaButtons.Children.Add(mute);


                                    stackMedia.Children.Add(espace1);
                                    stackMedia.Children.Add(stackEnteteMedia);
                                    stackMedia.Children.Add(espace2);
                                    stackMedia.Children.Add(myMedia);
                                    stackMedia.Children.Add(espace3);
                                    stackMedia.Children.Add(lb4);
                                    stackMedia.Children.Add(stackMediaButtons);
                                    stackMedia.Children.Add(lb5);

                                    PriseZone.Content = stackMedia;
                                    Grid.SetRow(PriseZone, i);
                                    gr.Children.Add(PriseZone);
                                    i++;
                                    test = true;
                                    break;
                                }
                                else
                                    test = false;
                            }
                            if (test == false)
                            {
                                StackPanel stackErreur = new StackPanel();
                                stackErreur.HorizontalAlignment = HorizontalAlignment.Center;
                                stackErreur.VerticalAlignment = VerticalAlignment.Center;
                                stackErreur.Orientation = Orientation.Vertical;

                                //MessageBox.Show("no" + pr);
                                ColorZone ErreurZone = new ColorZone();
                                ErreurZone.Background = Brushes.Red;

                                StackPanel stackEnteteErreur = new StackPanel();
                                stackEnteteErreur.HorizontalAlignment = HorizontalAlignment.Center;
                                stackEnteteErreur.VerticalAlignment = VerticalAlignment.Center;
                                stackEnteteErreur.Orientation = Orientation.Vertical;

                                Chip EnteteInfoErreur = new Chip();
                                EnteteInfoErreur.Content = "Nom Prise : " + pr + "          Details : " + ComboBoxSequences.SelectedItem.ToString() + " / " + ComboBoxPlans.SelectedItem.ToString() + "   N'est pas trouvée";
                                stackEnteteErreur.Children.Add(EnteteInfoErreur);

                                StackPanel stackP = new StackPanel { };
                                stackP.HorizontalAlignment = HorizontalAlignment.Center;
                                stackP.Margin = new Thickness(8);
                                stackP.Orientation = Orientation.Horizontal;

                                Button rechercher = new Button();
                                rechercher.Content = "rechercher";
                                rechercher.Click += (Object, RoutedEventArgs) => { RechercherVideo(sender, e, pr, i, ErreurZone); };

                                PackIcon ic = new PackIcon();
                                ic.Kind = PackIconKind.Refresh;

                                Badged rechercherBg = new Badged();
                                rechercherBg.HorizontalAlignment = HorizontalAlignment.Center;
                                rechercherBg.VerticalAlignment = VerticalAlignment.Center;
                                rechercherBg.Badge = ic;
                                rechercherBg.Content = rechercher;

                                stackP.Children.Add(rechercherBg);

                                stackEnteteErreur.Children.Add(stackP);

                                stackErreur.Children.Add(stackEnteteErreur);
                                ErreurZone.Content = stackErreur;
                                Grid.SetRow(ErreurZone, i);
                                gr.Children.Add(ErreurZone);
                                i++;
                            }

                        }
                    }
                }
            }
        }

        public void RechercherVideo(Object sender, RoutedEventArgs e, String nomPrise,int i,ColorZone Error)
        {
            System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
            browse.RootFolder = Environment.SpecialFolder.Desktop;
            browse.Description = " +++ select folder +++";
            browse.ShowNewFolderButton = false;

            var test = false;
            if (browse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string[] files = Directory.GetFiles(browse.SelectedPath);
                foreach (String s in files)
                {
                        if (System.IO.Path.GetFileName(s) == nomPrise)
                        {
                            StackPanel stackMedia = new StackPanel();
                            stackMedia.HorizontalAlignment = HorizontalAlignment.Center;
                            stackMedia.VerticalAlignment = VerticalAlignment.Center;
                            stackMedia.Orientation = Orientation.Vertical;

                            MediaElement myMedia = new MediaElement();
                            myMedia.Source = new Uri(s, UriKind.RelativeOrAbsolute);
                            myMedia.LoadedBehavior = MediaState.Manual;
                            myMedia.Width = 440;

                            StackPanel stackMediaButtons = new StackPanel();
                            stackMediaButtons.Orientation = Orientation.Horizontal;
                            stackMediaButtons.HorizontalAlignment = HorizontalAlignment.Center;
                            stackMediaButtons.VerticalAlignment = VerticalAlignment.Center;

                            StackPanel stackEnteteMedia = new StackPanel();
                            stackEnteteMedia.Orientation = Orientation.Horizontal;
                            stackEnteteMedia.HorizontalAlignment = HorizontalAlignment.Center;
                            stackEnteteMedia.VerticalAlignment = VerticalAlignment.Center;

                            Chip EnteteInfoMedia = new Chip();
                            EnteteInfoMedia.Content = "Nom Prise : " + nomPrise + "          Details : " + ComboBoxSequences.SelectedItem.ToString() + " / " + ComboBoxPlans.SelectedItem.ToString();
                            stackEnteteMedia.Children.Add(EnteteInfoMedia);

                            PackIcon playIcon = new PackIcon();
                            playIcon.Kind = PackIconKind.Play;
                            Button play = new Button();
                            play.Content = playIcon;
                            play.Click += (Object, RoutedEventArgs) => { mediaPlay(sender, e, myMedia); };

                            PackIcon pauseIcon = new PackIcon();
                            pauseIcon.Kind = PackIconKind.Pause;
                            Button pause = new Button();
                            pause.Content = pauseIcon;
                            pause.Click += (Object, RoutedEventArgs) => { mediaPause(sender, e, myMedia); };

                            PackIcon stopIcon = new PackIcon();
                            stopIcon.Kind = PackIconKind.Stop;
                            Button stop = new Button();
                            stop.Content = stopIcon;
                            stop.Click += (Object, RoutedEventArgs) => { mediaStop(sender, e, myMedia); };

                            PackIcon muteIcon = new PackIcon();
                            muteIcon.Kind = PackIconKind.MusicNoteOff;
                            Button mute = new Button();
                            mute.Content = muteIcon;
                            mute.Click += (Object, RoutedEventArgs) => { mediaMute(sender, e, myMedia, mute); };

                            ColorZone PriseZone = new ColorZone();
                            PriseZone.Background = Brushes.Green;

                            Separator espace1 = new Separator();
                            Separator espace2 = new Separator();
                            Label lb1 = new Label();
                            Label lb2 = new Label();
                            Label lb3 = new Label();
                            Label lb4 = new Label();
                            Label lb5 = new Label();

                            stackMediaButtons.Children.Add(play);
                            stackMediaButtons.Children.Add(lb1);
                            stackMediaButtons.Children.Add(pause);
                            stackMediaButtons.Children.Add(lb2);
                            stackMediaButtons.Children.Add(stop);
                            stackMediaButtons.Children.Add(lb3);
                            stackMediaButtons.Children.Add(mute);


                            stackMedia.Children.Add(espace1);
                            stackMedia.Children.Add(stackEnteteMedia);
                            stackMedia.Children.Add(myMedia);
                            stackMedia.Children.Add(espace2);
                            stackMedia.Children.Add(lb4);
                            stackMedia.Children.Add(stackMediaButtons);
                            stackMedia.Children.Add(lb5);

                            PriseZone.Content = stackMedia;
                            Grid.SetRow(PriseZone, i);
                            gr.Children.Add(PriseZone);
                        test = true;
                        break;
                        }
                        else
                        {
                        test = false;
                        }

                }
                if ( test == false)
                {
                    StackPanel stackErreur = new StackPanel();
                    stackErreur.HorizontalAlignment = HorizontalAlignment.Center;
                    stackErreur.VerticalAlignment = VerticalAlignment.Center;
                    stackErreur.Orientation = Orientation.Vertical;

                    ColorZone ErreurZone = new ColorZone();
                    ErreurZone.Background = Brushes.Red;

                    StackPanel stackEnteteErreur = new StackPanel();
                    stackEnteteErreur.HorizontalAlignment = HorizontalAlignment.Center;
                    stackEnteteErreur.VerticalAlignment = VerticalAlignment.Center;
                    stackEnteteErreur.Orientation = Orientation.Vertical;

                    Chip EnteteInfoErreur = new Chip();
                    EnteteInfoErreur.Content = "Nom Prise : " + nomPrise + "          Details : " + ComboBoxSequences.SelectedItem.ToString() + " / " + ComboBoxPlans.SelectedItem.ToString() + "   N'est pas trouvée";
                    stackEnteteErreur.Children.Add(EnteteInfoErreur);

                    StackPanel stackP = new StackPanel { };
                    stackP.HorizontalAlignment = HorizontalAlignment.Center;
                    stackP.Margin = new Thickness(8);
                    stackP.Orientation = Orientation.Horizontal;

                    Button rechercher = new Button();
                    rechercher.Content = "rechercher";
                    rechercher.Click += (Object, RoutedEventArgs) => { RechercherVideo(sender, e, nomPrise, i, Error); };

                    PackIcon ic = new PackIcon();
                    ic.Kind = PackIconKind.Refresh;

                    Badged rechercherBg = new Badged();
                    rechercherBg.HorizontalAlignment = HorizontalAlignment.Center;
                    rechercherBg.VerticalAlignment = VerticalAlignment.Center;
                    rechercherBg.Badge = ic;
                    rechercherBg.Content = rechercher;

                    stackP.Children.Add(rechercherBg);

                    stackEnteteErreur.Children.Add(stackP);

                    stackErreur.Children.Add(stackEnteteErreur);
                    ErreurZone.Content = stackErreur;
                    Grid.SetRow(ErreurZone, i);
                    gr.Children.Add(ErreurZone);
                    i++;
                    test = true;
                    
                }
            }
        }

        void mediaPlay(Object sender, EventArgs e, MediaElement myMedia)
        {
            myMedia.Play();
        }

        void mediaPause(Object sender, EventArgs e, MediaElement myMedia)
        {
            myMedia.Pause();
        }
        void mediaStop(Object sender, EventArgs e, MediaElement myMedia)
        {
            myMedia.Stop();
        }
        void mediaMute(Object sender, EventArgs e, MediaElement myMedia, Button mute)
        {
            if (myMedia.Volume != 0)
            {
                myMedia.Volume = 0;
                PackIcon ecouterIcon = new PackIcon();
                ecouterIcon.Kind = PackIconKind.MusicNote;
                mute.Content=  ecouterIcon;
            }
            else
            {
                PackIcon muteIcon = new PackIcon();
                muteIcon.Kind = PackIconKind.MusicNoteOff;
                myMedia.Volume = 100;
                mute.Content = muteIcon;
            }
        }
        public void mediaVolumeSlider(Object sender, RoutedPropertyChangedEventArgs<double> r, MediaElement myMedia,Slider mediaVolumeSlider)
        {
            myMedia.Volume = (double)mediaVolumeSlider.Value; 
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxPlans.IsEnabled = false;
            valider.IsEnabled = false;
            nomFilm.HorizontalAlignment = HorizontalAlignment.Center;
            nomFilm.HorizontalContentAlignment = HorizontalAlignment.Center;
            nomFilm.Content = "Film : " + mv;

            var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
            var sequences = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").OrderByKey().OnceAsync<Movies>();
            foreach (var seq in sequences)
            {
                ComboBoxSequences.Items.Add(seq.Key);
            }

            ComboBoxSequences.Items.Add("tout");
            ComboBoxPlans.Items.Add("tout");
        }

        private async void ComboBoxSequences_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            if (ComboBoxSequences.SelectedItem.ToString() == "tout")
            {
                ComboBoxPlans.IsEnabled = false;
                valider.IsEnabled = true;
            }
            else
            {
                gr.Children.Clear();
                gr.RowDefinitions.Clear();
                valider.IsEnabled = false;
                ComboBoxPlans.IsEnabled = true;
                ComboBoxPlans.Items.Clear();
                var firebase = new FirebaseClient("https://applicationcliente.firebaseio.com/");
                var plans = await firebase.Child(id).Child("movies").Child(mv).Child("Sequence").Child(ComboBoxSequences.SelectedItem.ToString()).Child("plans").OrderByKey().OnceAsync<Plan>();
                foreach (var plan in plans)
                {
                    ComboBoxPlans.Items.Add(plan.Key);
                }
                ComboBoxPlans.Items.Add("tout");
            }
           // if (ComboBoxSequences.SelectedItem.ToString() != "Sélectionner Seq" || ComboBoxPlans.SelectedItem.ToString() != "Sélectionner ¨Plan")
              //  valider.IsEnabled = true;
           // else
                //valider.IsEnabled = false;
        }

        private void ComboBoxPlans_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gr.Children.Clear();
            gr.RowDefinitions.Clear();
            if (ComboBoxPlans.SelectedItem.ToString() != "Sélectionner ¨Plan")
                valider.IsEnabled = true;
            else
                valider.IsEnabled = false;
        }
    }
}

//bahaeddinebb@outlook.com