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

            /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Longitude", Current.CU.Longitude.ToString()),
                new KeyValuePair<string, string>("Latitude", Current.CU.Latitude.ToString()),
                new KeyValuePair<string, string>("Entry", SearchBar.Text),
            });

            HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Finder/GetSearchedPlace.php", content); //send the variable content to the database as a POST method

            /*Checks if the data is retreived from the database*/
            if (res.IsSuccessStatusCode)
            {
                var DBOutput = await res.Content.ReadAsStringAsync();
                IList<Place> SearchedPlace = JsonConvert.DeserializeObject<IList<Place>>(DBOutput); //A bar or club searched in the database

                /*Checks if the SearchedPlace variable (response from the database) isn't equal to null*/
                if (SearchedPlace != null)
                {
                    ListOfPlaces.ItemsSource = SearchedPlace; //Sets the itemsource of ListOfPlaces to the IList SearchedPlace
                }
            }
        }

        /*--------------------------*/
        /*String formating*/
        public static string DistanceFormatting(double Distance)
        {
            //The distance received from the database is in Km
            if (Distance >= 1)
            {
                return string.Format("{0:F2} km", Distance);
            }
            else
            {
                //If the distance is 0.999 km (or lower) we need to multiply it by 1000 and display it in meters
                double DistanceInMeters = Distance * 1000;
                return string.Format("{0:F0} m", DistanceInMeters);
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
            /*Checks if the input field isn't empty or doesn't contain a white space*/
            if (string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                await Finder.TranslateTo(0, 400, 200);
            }
        }

        /*Change FinderView and go to selected place*/
        public async void Place_Tapped(object sender, ItemTappedEventArgs e)
        {
            Current.SelectedItem = (Place)e.Item; //Stores the selected place it's data in the variable Current.SelectedItem

            await Finder.TranslateTo(0, 400, 200); //Makes the content of the current page fade out in 125 ms
            await MainViewContent.FadeTo(0, 150);
            Current.FV.Children[0] = new RouteView(); //Changes the value of the grid FV to the new RouteView page

            Current.UpdateCamera = false; //Prevents the camera from updating when the postion changes
            Position LocationTappedPlace = new Position(Current.SelectedItem.Latitude, Current.SelectedItem.Longitude);
            await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(LocationTappedPlace, 17.5), TimeSpan.FromSeconds(2.5)); //Animate the camera of the map to the selected position with a zoom of 17.5 and in a timespan of 2.5 seconds
        }
    }
}