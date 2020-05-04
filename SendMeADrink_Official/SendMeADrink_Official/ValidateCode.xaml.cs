using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValidateCode : ContentPage
    {

        public string code;
        public string Email;

        public ValidateCode(string randomCode, string email)
        {
            InitializeComponent();
            code = randomCode;
            Email = email;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ValidateCode_Clicked(object sender, EventArgs e)
        {
            if(code == ValidationCode.Text)
            {
                Application.Current.MainPage = new NavigationPage(new ResetPassword(Email));
            }
            else
            {
                await DisplayAlert("You entred an invalid validation code. Please, try again with obbtained code or make a new password request.", null, null, "OK"); 
            }
        }
    }
}