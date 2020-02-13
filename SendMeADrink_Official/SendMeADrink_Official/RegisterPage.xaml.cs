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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void SUButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("", "Succesfull sign up", "", "Close");

            if(result == false)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }

        public void ShowPassword(object sender, EventArgs args)
        {
            Password.IsPassword = Password.IsPassword ? false : true;
            RepeatPassword.IsPassword = RepeatPassword.IsPassword ? false : true;
        }
    }

}