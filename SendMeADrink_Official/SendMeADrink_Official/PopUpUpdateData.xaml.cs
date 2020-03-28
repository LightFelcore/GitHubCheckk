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

        public PopUpUpdateData()
        {
            InitializeComponent();
            BindingContext = ((App)App.Current).CU;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UpdateDataUsername.Text) || string.IsNullOrWhiteSpace(UpdateDataEmail.Text) || string.IsNullOrWhiteSpace(UpdateDataAge.Text))
            {
                await DisplayAlert("Enter all information", "", "Close");
            }
            else
            {
                HttpClient client = new HttpClient(new HttpClientHandler());

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Id", ((App)App.Current).CU.Id),
                    new KeyValuePair<string, string>("Username", ((App)App.Current).CU.Username),
                    new KeyValuePair<string, string>("Email", ((App)App.Current).CU.Email),
                    new KeyValuePair<string, string>("Age", ((App)App.Current).CU.Age),
                });

                await client.PostAsync("http://send-meadrink.com/PHP/update.php", content);

                ((App)App.Current).CU.Username = UpdateDataUsername.Text;
                ((App)App.Current).CU.Email = UpdateDataEmail.Text;
                ((App)App.Current).CU.Age = UpdateDataAge.Text;

                
            }
        }
    }
}