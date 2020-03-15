using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SendMeADrink_Official.Database;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpUpdateData
    {
        string Id;

        public PopUpUpdateData(string _Id)
        {
            InitializeComponent();
            Id = _Id;
        }

        private readonly HttpClient _client = new HttpClient(new System.Net.Http.HttpClientHandler());

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UpdateDataUsername.Text) || string.IsNullOrWhiteSpace(UpdateDataEmail.Text) || string.IsNullOrWhiteSpace(UpdateDataAge.Text) || string.IsNullOrWhiteSpace(UpdateDataPasswd.Text) || string.IsNullOrWhiteSpace(UpdateDataRepeatPasswd.Text))
            {
                await DisplayAlert("Enter all information", "", "Close");
            }
            else
            {
                if(UpdateDataPasswd.Text == UpdateDataRepeatPasswd.Text)
                {
                    user u = new user() { Username = UpdateDataUsername.Text, Email = UpdateDataEmail.Text, Age = UpdateDataAge.Text, Passwd = UpdateDataPasswd.Text };

                    var _content = new FormUrlEncodedContent(new[]
                    {
                       new KeyValuePair<string, string>("Username", u.Username),
                       new KeyValuePair<string, string>("Email", u.Email),
                       new KeyValuePair<string, string>("Passwd", u.Passwd),
                       new KeyValuePair<string, string>("Age", u.Age)
                    });

                    var res = await _client.PostAsync("http://10.0.2.2/DATA/USER/update.php", _content);

                    var _tokenJson = await res.Content.ReadAsStringAsync();

                    await DisplayAlert("Profile Updated", null, null, "Ok");

                    Application.Current.MainPage = new ProfilePage(Id);
                }
            }
        }
    }
}