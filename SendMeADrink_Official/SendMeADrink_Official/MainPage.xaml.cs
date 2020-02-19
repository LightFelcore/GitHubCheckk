using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        }
        private async void LIButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Email.Text) || string.IsNullOrWhiteSpace(PasswordLogIn.Text))
            {
                await DisplayAlert("Enter email/password", null, "Close");
            }
            else
            {
                await Navigation.PushAsync(new MapPage());
            }
            Email.Text = PasswordLogIn.Text = string.Empty;
        }
        public void ShowPassword(object sender, EventArgs args)
        {
            PasswordLogIn.IsPassword = PasswordLogIn.IsPassword ? false : true;
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
