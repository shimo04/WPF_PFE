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
                        var namePrise = prise.Object.nomPrise + ".MP4";
                        if( prise.Object.note == res)
                        {
                            listPr.Add(namePrise);
                        }
                    }
                }                    
           }
            var cpt = 0;
            while (cpt < listPr.Count())
            {
                gr.RowDefinitions.Add(new RowDefinition());
                cpt++;
            }
            System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
            //openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            browse.RootFolder = Environment.SpecialFolder.Desktop;
            browse.Description = " +++ select folder +++";
            browse.ShowNewFolderButton = false;
            var i = 0;
   
            if (browse.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = Directory.GetFiles(browse.SelectedPath);
                string[] dirs = Directory.GetDirectories(browse.SelectedPath);
                MessageBox.Show(files.Length.ToString() + " et " + dirs.Length.ToString());

                foreach (String s in files)
                {
                    foreach(String pr in listPr)
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

                            Slider VolumeSlider = new Slider();
                            VolumeSlider.TickFrequency = 5;
                            VolumeSlider.Orientation = Orientation.Horizontal;
                            VolumeSlider.Minimum = 0;
                            VolumeSlider.Maximum = 100;
                            VolumeSlider.Value = 40;
                            //VolumeSlider.HorizontalAlignment = HorizontalAlignment.Right;
                            // VolumeSlider.ValueChanged += (Object, RoutedPropertyChangedEventArgs) => { mediaVolumeSlider(sender, r, myMedia, VolumeSlider); }; 

                            Chip EnteteInfoMedia = new Chip();
                            EnteteInfoMedia.Content = "Film : " + mv;
                            stackEnteteMedia.Children.Add(EnteteInfoMedia);
                            stackEnteteMedia.Children.Add(VolumeSlider);

                            Button play = new Button();
                            play.Content = "Play";

                            play.Click += (Object, RoutedEventArgs) => { mediaPlay(sender, e, myMedia); };
                            Button pause = new Button();
                            pause.Content = "Pause";
                            pause.Click += (Object, RoutedEventArgs) => { mediaPause(sender, e, myMedia); };

                            Button stop = new Button();
                            stop.Content = " Stop";
                            stop.Click += (Object, RoutedEventArgs) => { mediaStop(sender, e, myMedia); };

                            Button mute = new Button();
                            mute.Content = " Mute";
                            mute.Click += (Object, RoutedEventArgs) => { mediaMute(sender, e, myMedia,mute); };


                            Separator espace1 = new Separator();
                            Separator espace2 = new Separator();
                            Label lb1 = new Label();
                            Label lb2 = new Label();
                            Label lb3 = new Label();
                            Label lb4 = new Label();
                            Label lb5 = new Label();
                            Label lb6 = new Label();
                            Label lb7 = new Label();

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
                            stackMedia.Children.Add(lb6);
                            stackMedia.Children.Add(stackMediaButtons);
                            stackMedia.Children.Add(lb7);

                            Grid.SetRow(stackMedia, i);
                            gr.Children.Add(stackMedia);
                            i++;
                        }
                       
                    }
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
                mute.Content = "Ecouter";
            }
            else
            {
                myMedia.Volume = 100;
                mute.Content = "Mute";
            }
        }
        void mediaVolumeSlider(Object sender, RoutedPropertyChangedEventArgs<double> r, MediaElement myMedia,Slider mediaVolumeSlider)
        {
            myMedia.Volume = (double)mediaVolumeSlider.Value; 
        }
    }
}

//bahaeddinebb@outlook.com