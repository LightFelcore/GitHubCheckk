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
    public partial class UpdateData
    {
        readonly App Current = (App)App.Current;

        public UpdateData()
        {
            InitializeComponent();
            BindingContext = Current.CU;
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
                    new KeyValuePair<string, string>("Id", Current.CU.Id),
                    new KeyValuePair<string, string>("Username", Current.CU.Username),
                    new KeyValuePair<string, string>("Email", Current.CU.Email),
                    new KeyValuePair<string, string>("Age", Current.CU.Age),
                });

                await client.PostAsync("http://send-meadrink.com/SMAD_App/Update/update.php", content);

                Current.CU.Username = UpdateDataUsername.Text;
                Current.CU.Email = UpdateDataEmail.Text;
                Current.CU.Age = UpdateDataAge.Text;
            }
        }
    }
}