using Newtonsoft.Json;
using SendMeADrink_Official.Database;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;

namespace SendMeADrink_Official.FinderViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentView
    {
        readonly App Current = (App)App.Current;

        public MainView()
        {
            InitializeComponent();

            ListOfPlaces.ItemsSource = Current.Places; //Used to load the list of places once the page loads
            Current.PlacesListView = ListOfPlaces; //Used to update the list of places
        }

        /*--------------------------*/
        /*Function to get a place*/
        public async void SearchButton_Pressed(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient(new HttpClientHandler());

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Longitude", Current.CU.Longitude.ToString()),
                new KeyValuePair<string, string>("Latitude", Current.CU.Latitude.ToString()),
                new KeyValuePair<string, string>("Entry", SearchBar.Text),
            });

            HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Finder/GetSearchedPlace.php", content); //send the variable content to the database as a POST method
            var DBOutput = await res.Content.ReadAsStringAsync();
            IList<Place> SearchedPlace = JsonConvert.DeserializeObject<IList<Place>>(DBOutput); //A bar or club searched in the database

            if (SearchedPlace != null)
            {
                ListOfPlaces.ItemsSource = SearchedPlace;
            }
        }

        /*--------------------------*/
        /*String formating*/
        public string DistanceString
        {
            get
            {
                //The distance received from the database is in Km
                if (Current.SelectedItem.Distance >= 1)
                {
                    return string.Format("{0:F2} km", Current.SelectedItem.Distance);
                }
                else
                {
                    //If the distance is 0.999 km (or lower) we need to multiply it by 1000 and display it in meters
                    double DistanceInMeters = Current.SelectedItem.Distance * 1000;
                    return string.Format("{0:F0} m", DistanceInMeters);
                }
            }
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

        /*Search bar controls*/
        public async void SearchBar_Focused(object sender, FocusEventArgs e)
        {
            await Finder.TranslateTo(0, 64, 200);
        }

        public async void SearchBar_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                await Finder.TranslateTo(0, 400, 200);
            }
        }

        /*Change FinderView and go to selected place*/
        public async void Place_Tapped(object sender, ItemTappedEventArgs e)
        {
            Current.SelectedItem = (Place)e.Item;

            await Finder.TranslateTo(0, 400, 200);
            await MainViewContent.FadeTo(0, 150);
            Current.FV.Children[0] = new RouteView();

            Position LocationTappedPlace = new Position(Current.SelectedItem.Latitude, Current.SelectedItem.Longitude);
            await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(LocationTappedPlace, 17.5), TimeSpan.FromSeconds(2.5));
        }
    }
}