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
using Plugin.Settings;


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
            EmailOrUsernameEntry.Text = "aze";
            PasswordEntry.Text = "aze";
        }

        private readonly HttpClient client = new HttpClient(new System.Net.Http.HttpClientHandler());
        private async void LIButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailOrUsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Enter username/password", null, null, "Close");
            }
            else
            {
                user u = new user() { Email = EmailOrUsernameEntry.Text, Passwd = PasswordEntry.Text, Username = EmailOrUsernameEntry.Text }; // entry voor het inloggen (Deze entries moeten dus vergeleken worden met die dat in de database aanwezig zijn)

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Username", u.Username),
                    new KeyValuePair<string, string>("Email", u.Email),
                    new KeyValuePair<string, string>("Passwd", u.Passwd), 
                });

                var result = await client.PostAsync("http://10.0.2.2/DATA/USER/login.php", content); // connectie met mijn emulator

                var responseString = await result.Content.ReadAsStringAsync();

                int count = int.Parse(responseString);

                if (count == 1)      //check php file (login.php)                                
                {
                    Application.Current.MainPage = new ShellNavPage();              //user authenticated --> nav to other page
                }
                if (count == 0)       //check php file (login.php) 
                {
                    await DisplayAlert("Error, Retry Again!", null, null, "Ok");            //user not authenticated
                }
            }
            
        }


        private void RememberMe_Clicked(object sender, CheckedChangedEventArgs e)
        {

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
            var result = await DisplayAlert("Forgot password?", "We can send you an email to help you get back into your account", "Send Email", "Cancel");

            if (result == true)
            {
                await Navigation.PushAsync(new ForgotPasswordPage());
            }
        }
    } 
}
