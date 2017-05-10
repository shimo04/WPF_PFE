using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Firebase.Auth;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using Firebase.Database;
using System.Threading;
using System.Linq;
using MaterialDesignThemes.Wpf;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using ToastNotifications;
using ToastNotifications.Core;

namespace PFE_WPF
{
    public partial class LogIn : Window
    {
        public static String UID { get; set; }

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomLeft,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        public const string GoogleClientId = "<GOOGLE CLIENT ID>"; // https://console.developers.google.com/apis/credentials
        public const string f = "<FIREBASE APP KEY>"; // https://console.firebase.google.com/
        public const string FirebaseAppUrl = "https://applicationcliente.firebaseio.com/";
        public const string FirebaseApiKey = "AIzaSyCvoKY814M1ipT2A2OPp16VJCxRKqAXkpQ";

        public LogIn()
        {
            InitializeComponent();
        }

        private async void Connect_Button(object sender, RoutedEventArgs e)
        {
            bool loginSuccess = true;
            try
            {
                Progress.Visibility = Visibility.Visible;
                await InitializeConnexionAsync(LoginAsync);
                var Homie = new Homie(UID);
                Homie.Show();
                this.Close();
            }
            catch (Exception)
            {
                loginSuccess = false;
                //MessageBoxResult x = MessageBox.Show("Try", "again", MessageBoxButton.OK);
                //if (x == MessageBoxResult.OK)
                // {
                Progress.Visibility = Visibility.Hidden;
                notifier.ShowError("Try again");
                //}
            }
            finally
            {
                if (loginSuccess)
                {
                    //MessageBoxResult x = MessageBox.Show("Note", "sucess", MessageBoxButton.OK);
                    //if (x == MessageBoxResult.OK)
                    // {
                    Progress.Visibility = Visibility.Hidden;
                    //}
                }
            }
        }

        public async Task InitializeConnexionAsync(Func<string, string, Task<string>> action)
        {
            string em = email.Text;
            string pass = password.Password;
            var firebase = new FirebaseClient(FirebaseAppUrl, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => action(em, pass)
            });

            var result = await firebase.Child("/").OnceAsync<object>();
            //MessageBox.Show(result.Count.ToString());

        }

        private static async Task<string> LoginAsync(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            UID = auth.User.LocalId.ToString();
            return auth.FirebaseToken;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        //private static async Task<string> SignupAsync(string email, string password)
        // {
        // var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
        // var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
        // return auth.FirebaseToken;
        // }
    }
}