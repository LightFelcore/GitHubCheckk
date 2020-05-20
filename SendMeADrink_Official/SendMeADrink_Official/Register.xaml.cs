using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        /*Sign up button clicked handeler*/
        public async void SUButton_Clicked(object sender, EventArgs e)
        {
            /*Checks if the input fields aren't empty or do not contain only a white space*/
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(AgeEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text) || string.IsNullOrWhiteSpace(RepeatPasswordEntry.Text))
            {
                ErrorMessage.Text = "Enter the correct information"; //Show an error message
            }
            else
            {
                /*Checks if the entered password are equal*/
                if (PasswordEntry.Text == RepeatPasswordEntry.Text)
                {
                    HttpClient client = new HttpClient(new HttpClientHandler());

                    /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
                    var content = new FormUrlEncodedContent(new[]
                    {
                       new KeyValuePair<string, string>("Username", UsernameEntry.Text),
                       new KeyValuePair<string, string>("Email", EmailEntry.Text),
                       new KeyValuePair<string, string>("Passwd", PasswordEntry.Text),
                       new KeyValuePair<string, string>("Age", AgeEntry.Text)
                    });

                    HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Register/Register.php", content);

                    /*Checks if the data is retreived from the database*/
                    if (res.IsSuccessStatusCode)
                    {
                        var json = await res.Content.ReadAsStringAsync();

                        /*Checks if the data returned from the database is equal to true*/
                        if (JsonConvert.DeserializeObject<bool>(json) == true)
                        {
                            await DisplayAlert("The email address you have entered is already registered", null, "Close"); //Alert the user that an account already has been registered with the entered email
                            PasswordEntry.Text = RepeatPasswordEntry.Text = string.Empty;
                        }
                        else
                        {
                            await DisplayAlert("Registration Completed", null, null, "Close"); //Inform the user that he's account has been succefully created
                            Application.Current.MainPage = new NavigationPage(new Login());
                        }
                    }
                }
                else
                {
                    ErrorMessage.Text = "The entered passwords do not match";
                }
            }
        }

        /*Removes the last page of the stack*/
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /*Show/hide the entered password*/
        public void ShowPassword(object sender, EventArgs args)
        {
            PasswordEntry.IsPassword = PasswordEntry.IsPassword ? false : true;
        }

        /*Show/hide the entered password*/
        public void ShowRepeatedPassword(object sender, EventArgs args)
        {
            RepeatPasswordEntry.IsPassword = RepeatPasswordEntry.IsPassword ? false : true;
        }
    }
}