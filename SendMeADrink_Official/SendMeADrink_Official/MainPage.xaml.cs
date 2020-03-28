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
using Json.Net;

namespace SendMeADrink_Official
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public User u = new User();

        public MainPage()
        {
            InitializeComponent();
            EmailOrUsernameEntry.Text = "lucavz";
            PasswordEntry.Text = "azerty";
        }

        /*Login button clicked handeler*/
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

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", EmailOrUsernameEntry.Text),
                    new KeyValuePair<string, string>("Username", EmailOrUsernameEntry.Text),
                    new KeyValuePair<string, string>("Passwd", PasswordEntry.Text),
                });

                HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/PHP/login.php", content); // connectie met mijn emulator
                var json = await res.Content.ReadAsStringAsync();
                u = JsonConvert.DeserializeObject<User>(json);

                if (u != null)
                {
                    await GetUserLocationAsync(); //Get the users current location, await because we first need to run the function and debug the rest after that is compleet

                    ((App)App.Current).CU = u; //store the gathered data in CU

                    Application.Current.MainPage = new NavigationPage(new MasterDetailMapPage()); //user authenticated --> nav to other page
                }
                else
                {
                    await DisplayAlert("Login Failed!", "The entered email/username or password is incorrect!", null, "Ok"); //user not authenticated
                }
            }
        }

        /*Show the password when pressed*/
        public void ShowPassword(object sender, EventArgs args)
        {
            PasswordEntry.IsPassword = PasswordEntry.IsPassword ? false : true;
        }

        /*Navigate to register page*/
        private async void SUButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        /*Navigate to forgot password page*/
        private async void FPButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }

        /*Remember the user when checked*/
        private void RememberMe_Clicked(object sender, CheckedChangedEventArgs e)
        {
          //damn
        }

        /*Get the users current location*/
        public async Task GetUserLocationAsync()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best);
                Location coördinates = await Geolocation.GetLocationAsync(request);

                if (coördinates != null)
                {
                    u.Longitude = coördinates.Longitude;
                    u.Latitude = coördinates.Latitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Failed", fnsEx.Message, "OK");;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Failed", fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Failed", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", ex.Message, "OK");
            }
        }
    }  
}
