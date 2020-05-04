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
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        public async void SUButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(AgeEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text) || string.IsNullOrWhiteSpace(RepeatPasswordEntry.Text))
            {
                await DisplayAlert("Enter all information", "", "Close");
            }
            else
            {
                if (PasswordEntry.Text == RepeatPasswordEntry.Text)
                {
                    HttpClient client = new HttpClient(new HttpClientHandler());

                    var content = new FormUrlEncodedContent(new[]
                    {
                       new KeyValuePair<string, string>("Username", UsernameEntry.Text),
                       new KeyValuePair<string, string>("Email", EmailEntry.Text),
                       new KeyValuePair<string, string>("Passwd", PasswordEntry.Text),
                       new KeyValuePair<string, string>("Age", AgeEntry.Text)
                    });

                    HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Register/Register.php", content);
                    var json = await res.Content.ReadAsStringAsync();

                    if (JsonConvert.DeserializeObject<Boolean>(json) == true)
                    {
                        await DisplayAlert("The email address you have entered is already registered", null, "Close");
                        PasswordEntry.Text = RepeatPasswordEntry.Text = string.Empty;
                    }
                    else
                    {
                        await DisplayAlert("Registration Completed", null, null, "Close");
                        Application.Current.MainPage = new NavigationPage(new Login());
                    }
                }
                else
                {
                    await DisplayAlert("The entered passwords do not match",null, null, "Close");
                }
            }
        }

        /*Removes the last page of the stack*/
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /*Show passwords*/
        public void ShowPassword(object sender, EventArgs args)
        {
            PasswordEntry.IsPassword = PasswordEntry.IsPassword ? false : true;
        }
        public void ShowRepeatPassword(object sender, EventArgs args)
        {
            RepeatPasswordEntry.IsPassword = RepeatPasswordEntry.IsPassword ? false : true;
        }
    }
}