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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Net;
using System.IO;

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

        private void Connect_Button (object sender, RoutedEventArgs e)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                cc = "vv",
            });
          // var request = WebRequest.CreateHttp("https://applicationcliente.firebaseio.com/.json");
          //  request.Method = "POST";
          //  request.ContentType = "application/json";
           // var buffer = Encoding.UTF8.GetBytes(json);
           // request.ContentLength = buffer.Length;
          // request.GetRequestStream().Write(buffer, 0, buffer.Length);
           // var response = request.GetResponse();
            //json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
        }
    }
}
