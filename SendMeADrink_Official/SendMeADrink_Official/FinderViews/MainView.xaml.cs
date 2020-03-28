using Newtonsoft.Json;
using SendMeADrink_Official.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SendMeADrink_Official.FinderViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentView
    {
        public List<Bar> BarOrClub { get; set; }

        public MainView()
        {
            InitializeComponent();
            GetBarsAndClubs();

            BindingContext = BarOrClub;
            //Location coördinates = new Location(51.090457, 4.553066);
            //Location LocationBarOrClub = new Location(51.098731, 4.564226);
        }

        /*--------------------------*/
        /*Funtion to get all bars/Clubs in a radius of x KM*/
        public async void GetBarsAndClubs()
        {
            HttpClient client = new HttpClient(new HttpClientHandler());

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Longitude", ((App)App.Current).CU.Longitude.ToString()),
                new KeyValuePair<string, string>("Latitude", ((App)App.Current).CU.Latitude.ToString()),
            });

            HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/PHP/GetBars.php", content); //send the variable content to the database as a POST method
            var DBOutput = await res.Content.ReadAsStringAsync();
            BarOrClub = JsonConvert.DeserializeObject<List<Bar>>(DBOutput);

            Console.WriteLine(BarOrClub);
        }

        /*--------------------------*/
        /*Finder Controls/Naviagtion*/
        private async void FinderGoUp(object sender, SwipedEventArgs e)
        {
            if (Finder.TranslationY == 587.5)
            {
                await Finder.TranslateTo(0, 400, 200);
            }
            else if (Finder.TranslationY == 400)
            {
                await Finder.TranslateTo(0, 64, 200);
            }
        }

        private async void FinderGoDown(object sender, SwipedEventArgs e)
        {
            if (Finder.TranslationY == 64)
            {
                await Finder.TranslateTo(0, 400, 200);
            }
            else if (Finder.TranslationY == 400)
            {
                await Finder.TranslateTo(0, 587.5, 200);
            }
        }

        public async void SearchBar_Focused(object sender, FocusEventArgs e)
        {
            await Finder.TranslateTo(0, 64, 200);
        }

        private async void SearchBar_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                await Finder.TranslateTo(0, 400, 200);
            }
        }

        private void BarOrClubItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            //to be added
        }
    }
}