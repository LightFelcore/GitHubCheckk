using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;


namespace SendMeADrink_Official.ForgotPasswordViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPassword : ContentPage
    {
        public string _email; //Variable to store the entered email

        public ResetPassword(string Email)
        {
            InitializeComponent();
            _email = Email; 
        }

        /*Naviagtes to the previous page*/
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ResetPassword_Clicked(object sender, EventArgs e)
        {
            /*Checks if the input fields aren't empty or do not contain only a white space*/
            if (string.IsNullOrWhiteSpace(NewPassword.Text) || string.IsNullOrWhiteSpace(NewPasswordConfirm.Text))
            {
                await DisplayAlert("Enter all information", "", "Close");
            }
            else
            {
                /*Checks if the entered passwords are the same*/
                if (NewPassword.Text == NewPasswordConfirm.Text)
                {
                    HttpClient client = new HttpClient(new HttpClientHandler());

                    /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
                    var content = new FormUrlEncodedContent(new[]
                    {
                       new KeyValuePair<string, string>("Email", _email),
                       new KeyValuePair<string, string>("Passwd", NewPassword.Text)
                    });

                    await client.PostAsync("http://send-meadrink.com/SMAD_App/ForgotPassword/ResetPassword/resetPassword.php", content);

                    /*Inform the user that the password has been reset*/
                    await DisplayAlert("Your password has been reset successfully!", null, null, "OK");

                    Application.Current.MainPage = new NavigationPage(new Login());

                }
                else
                {
                    /*Inform the user that the entered passwords do not match*/
                    await DisplayAlert("The entered passwords do not match..",null, null, "Close");
                }
            }
        }
    }
}