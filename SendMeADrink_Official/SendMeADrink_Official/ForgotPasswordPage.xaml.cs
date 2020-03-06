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
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }
        private async void SEButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Email send succesfully", null, null, "Close");
            if(result == false)
            {
                await Navigation.PushAsync(new MainPage());
            }
        }
    }
}