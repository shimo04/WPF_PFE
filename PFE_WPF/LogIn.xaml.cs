using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Firebase.Auth;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using Firebase.Database;

namespace PFE_WPF
{
    public partial class LogIn : Window
    {

        public const string GoogleClientId = "<GOOGLE CLIENT ID>"; // https://console.developers.google.com/apis/credentials
        public const string FirebaseAppKey = "<FIREBASE APP KEY>"; // https://console.firebase.google.com/
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
                //To test, first run SignupAsync, then run LoginAsync
                //For some reason, both Login and Signup return error.
                //You might need to check your console.


                //await InitializeConnexionAsync(LoginAsync);
                await InitializeConnexionAsync(SignupAsync);
            }
            catch (Exception ex)
            {
                loginSuccess = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (loginSuccess)
                    MessageBox.Show("Success");
            }
        }
        public async Task InitializeConnexionAsync(Func<string,string,Task<string>> action)
        {
            string em = email.Text;
            string pass = password.Password;
            var firebase = new FirebaseClient(FirebaseAppUrl, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => action(em, pass)
            });

            var result = await firebase.Child("/").OnceAsync<object>();
            MessageBox.Show(result.Count.ToString());

        }

        //This is normallly done on the server.
        //Exposing FirebaseApiKey on the client is not good.
        //If you can't do this on server, consider using 3rd party providers like Facebook, Google.
        //Auth0 is a good service for this as Firebase does not provider 3rd party authentication for .NET
        private static async Task<string> LoginAsync(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            return auth.FirebaseToken;
        }

        private static async Task<string> SignupAsync(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
            var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            return auth.FirebaseToken;
        }
    }
}
