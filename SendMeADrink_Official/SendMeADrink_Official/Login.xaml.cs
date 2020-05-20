using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using SendMeADrink_Official.Database;
using SendMeADrink_Official.ForgotPasswordViews;
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
            /*Checks if the input fields aren't empty or do not contain only a white space*/
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                ErrorMessage.Text = "Enter the correct information"; //Show an error message
                
                EmailEntry.Text = PasswordEntry.Text = string.Empty; //Make the input fields empty again
            }
            else
            {
                HttpClient client = new HttpClient(new HttpClientHandler());

                /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", EmailEntry.Text),
                    new KeyValuePair<string, string>("Passwd", PasswordEntry.Text)
                });

                HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Login/login.php", content); //Calling the database

                /*Checks if the data is retreived from the database*/
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    User u = JsonConvert.DeserializeObject<User>(json);

                    /*Checks if the returned data isn't equal to null*/
                    if (u != null)
                    {
                        Current.CU = u; //Store the gathered data in CU
                        Current.CreditCards = await GetCards(Current.CU.Id); //Calls the function to get all the cards added by the logged in user

                        /*Checks if the "Remember Me" checkbox is checked*/
                        if (RememberMe.IsChecked)
                        {
                            await RememberLoggedInUser(); //Calls the function to store the login data of the logged in user
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

            /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("User_Id", Id)
            });

            HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Payment/GetCards.php", content); //Calling the database

            /*Checks if the returned data isn't equal to null*/
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<Creditcard>>(json); //Returns the gathered data as an IList of creditcards
            }
            else
            {
                return null;
            }
        }

        /*Remember the user when checked*/
        public async Task RememberLoggedInUser()
        {
            Preferences.Set("IsUserLoggedInToken", true); //Changes the value of the variable IsUserLoggedInToken to true
            Preferences.Set("EmailToken", EmailEntry.Text); //Changes the value of the variable EmailToken to the entered email

            try
            {
                await SecureStorage.SetAsync("PasswordToken", PasswordEntry.Text); //Storing the password in secure storage
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to store data in secure storage: " + ex);
            }
        }

        /*Show/hide the entered password*/
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
