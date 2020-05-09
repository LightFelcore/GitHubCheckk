using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using SendMeADrink_Official.Database;
using System.Net.Http;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SendMeADrink_Official
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class Login : ContentPage
    {
        readonly App Current = (App)App.Current;

        public Login()
        {
            InitializeComponent();
        }

        /*Login button clicked handeler*/
        public async void LIButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                ErrorMessage.Text = "Enter the correct information";
                
                EmailEntry.Text = PasswordEntry.Text = string.Empty;
            }
            else
            {
                HttpClient client = new HttpClient(new HttpClientHandler());

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", EmailEntry.Text),
                    new KeyValuePair<string, string>("Passwd", PasswordEntry.Text),
                });

                HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Login/login.php", content);

                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    User u = JsonConvert.DeserializeObject<User>(json);

                    if (u != null)
                    {
                        Current.CU = u; //Store the gathered data in CU
                        Current.CreditCards = await GetCards(Current.CU.Id);

                        if (RememberMe.IsChecked)
                        {
                            await RememberLoggedInUser();
                        }

                        Application.Current.MainPage = new NavigationPage(new MapPage()); //user authenticated --> navigate to MapPage
                    }
                    else
                    {
                        ErrorMessage.Text = "The entered email or password is incorrect!";
                    }
                }
            }
        }


        /*Get the users added credit cards*/
        public static async Task<IList<Creditcard>> GetCards(string Id)
        {
            HttpClient client = new HttpClient(new HttpClientHandler());

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("User_Id", Id),
            });

            HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Payment/GetCards.php", content);

            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<Creditcard>>(json); //Store the gathered credit cards in Current.CreditCards
            }
            else
            {
                return null;
            }
        }

        /*Remember the user when checked*/
        public async Task RememberLoggedInUser()
        {
            Preferences.Set("IsUserLoggedIn", true);
            Preferences.Set("EmailToken", EmailEntry.Text);

            try
            {
                await SecureStorage.SetAsync("PasswordToken", PasswordEntry.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to store data in secure storage: " + ex);
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
            await Navigation.PushAsync(new Register());
        }

        /*Navigate to forgot password page*/
        private async void FPButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }
    }  
}
