using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SendMeADrink_Official.ForgotPasswordViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValidateCode : ContentPage
    {
        public string code; //Variable to store the randomly generated code
        public string Email; //Variable to store the entered email

        public ValidateCode(string randomCode, string email)
        {
            InitializeComponent();
            code = randomCode;
            Email = email;
        }

        /*Navigate to the previous page*/
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /*Validatecode button handeler*/
        private async void ValidateCode_Clicked(object sender, EventArgs e)
        {
            /*Checks if the entered code and the randomly generated code are the same*/
            if(code == ValidationCode.Text)
            {
                await Navigation.PushAsync(new ResetPassword(Email)); //Navigate to the reset password page
            }
            else
            {
                ErrorMessage.Text = "You entered an invalid validation code"; //Show an error message
            }
        }
    }
}