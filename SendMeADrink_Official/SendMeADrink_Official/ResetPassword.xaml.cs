using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Specialized;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Xamarin.Essentials;   
using Xamarin.Forms.Core;


namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPassword : ContentPage
    {
        public string _email;

        public ResetPassword(string Email)
        {
            InitializeComponent();
            _email = Email;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ResetPassword_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewPassword.Text) || string.IsNullOrWhiteSpace(NewPasswordConfirm.Text))
            {
                await DisplayAlert("Enter all information", "", "Close");
            }
            else
            {
                if (NewPassword.Text == NewPasswordConfirm.Text)
                {
                    HttpClient client = new HttpClient(new HttpClientHandler());

                    var content = new FormUrlEncodedContent(new[]
                    {
                       new KeyValuePair<string, string>("Email", _email),
                       new KeyValuePair<string, string>("Passwd", NewPassword.Text)
                    });

                    await client.PostAsync("http://send-meadrink.com/SMAD_App/ForgotPassword/ResetPassword/resetPassword.php", content);

                    await DisplayAlert("Your password has been reset successfully!", null, null, "OK");

                    Application.Current.MainPage = new NavigationPage(new Login());

                }
                else
                {
                    await DisplayAlert("The entered passwords do not match..",null, null, "Close");
                }
            }
        }
    }
}