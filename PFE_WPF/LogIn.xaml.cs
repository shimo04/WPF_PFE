using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Firebase.Auth;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
namespace PFE_WPF
{
    public partial class LogIn : Window
    {

        public const string GoogleClientId = "<GOOGLE CLIENT ID>"; // https://console.developers.google.com/apis/credentials
        public const string FirebaseAppKey = "<FIREBASE APP KEY>"; // https://console.firebase.google.com/
        public const string FirebaseAppUrl = "https://applicationcliente.firebaseio.com/";

        public LogIn()
        {
            InitializeComponent();
        }

        private async void Connect_Button(object sender, RoutedEventArgs e)
        {
            await initializeConnexion();
        }
        public async Task initializeConnexion()
          {
        String em = email.ToString();
        String pass = password.ToString();
        var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyCvoKY814M1ipT2A2OPp16VJCxRKqAXkpQ"));
        var auth = await authProvider.SignInWithEmailAndPasswordAsync(em, pass);
        }
    }
}
