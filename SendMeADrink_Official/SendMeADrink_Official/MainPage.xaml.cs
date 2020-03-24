using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.IO;
using SendMeADrink_Official.Database;
using System.Net.Http;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Json.Net;

namespace SendMeADrink_Official
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            PasswordEntry.Text = "OiMate";
        }

        private async void LIButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailOrUsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Enter the correct information", null, null, "Close");
                EmailOrUsernameEntry.Text = PasswordEntry.Text = string.Empty;
            }
            else
            {
                HttpClient client = new HttpClient(new HttpClientHandler());
                User u = new User() { Email = EmailOrUsernameEntry.Text, Username = EmailOrUsernameEntry.Text, Passwd = PasswordEntry.Text };

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", u.Email),
                    new KeyValuePair<string, string>("Username", u.Username),
                    new KeyValuePair<string, string>("Passwd", u.Passwd),
                });

                HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/PHP/login.php", content); // connectie met mijn emulator
                var json = await res.Content.ReadAsStringAsync();
                u = JsonConvert.DeserializeObject<User>(json);

                if ((EmailOrUsernameEntry.Text == u.Email || EmailOrUsernameEntry.Text == u.Username) && MD5Hasher(PasswordEntry.Text) == u.Passwd)
                {
                    ((App)Application.Current).CU = u;
                    Application.Current.MainPage = new NavigationPage(new MasterDetailMapPage()); //user authenticated --> nav to other page
                }
                else
                {
                    await DisplayAlert("Error, Retry Again!", null, null, "Ok"); //user not authenticated
                }
            }

        }
        public void ShowPassword(object sender, EventArgs args)
        {
            PasswordEntry.IsPassword = PasswordEntry.IsPassword ? false : true;
        }
        private async void SUButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
        private async void FPButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }
        private void RememberMe_Clicked(object sender, CheckedChangedEventArgs e)
        {
          //damn
        }

        /*
        public string EmailOrUsername //Used to display your username on the MasterDetailPage Menu
        {
            get { return EmailOrUsernameEntry.Text; }
            set { EmailOrUsernameEntry.Text = value; }
        }*/

        /*------------------------------------------*/
        /*Function used to hash our passwords*/
        public string MD5Hasher(string PasswordEntry)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(PasswordEntry));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }  
}
