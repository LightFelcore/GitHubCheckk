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
using SendMeADrink_Official.Database;
using System.Net.Http.Headers;
using Xamarin.Essentials;
using Xamarin.Forms.Core;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private readonly HttpClient _client = new HttpClient(new System.Net.Http.HttpClientHandler());

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
                    user u = new user() { Username = UsernameEntry.Text, Email = EmailEntry.Text, Passwd = PasswordEntry.Text, Age = AgeEntry.Text };

                    var _content = new FormUrlEncodedContent(new[]
                    {
                       new KeyValuePair<string, string>("Id", u.Id),
                       new KeyValuePair<string, string>("Username", u.Username),
                       new KeyValuePair<string, string>("Email", u.Email),
                       new KeyValuePair<string, string>("Passwd", u.Passwd),
                       new KeyValuePair<string, string>("Age", u.Age)
                    });

                    var _result = await _client.PostAsync("http://10.0.2.2/DATA/USER/server.php", _content);


                    var _tokenJson = await _result.Content.ReadAsStringAsync();


                    await DisplayAlert("Registration Completed", null, null, "Close");

                    Application.Current.MainPage = new MainPage();

                }
                else
                {
                    await DisplayAlert("The entered passwords aren't the same", "", "Close");
                }
            }
        }
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