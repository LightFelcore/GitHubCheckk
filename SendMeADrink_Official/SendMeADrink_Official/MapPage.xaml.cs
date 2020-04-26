using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using SendMeADrink_Official.Database;
using SendMeADrink_Official.FinderViews;
using GoogleMaps = Xamarin.Forms.GoogleMaps;
using System.Reflection;
using Plugin.Geolocator;
using Geolocator = Plugin.Geolocator.Abstractions;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : MasterDetailPage
    {
        readonly App Current = (App)App.Current;

        public MapPage()
        {
            InitializeComponent();
            ChangeMapStyle();
            InitMapAndTracking();

            Current.FV = FinderView; //Store the grid in FV
            Current.FV.Children.Add(new MainView()); //Adding MainView to the FV grid (because it's the main view for the finder)
            UsernameLabel.BindingContext = Current.CU; //Binding the UsernameLabel with the CU variable

            //Standard Map settings
            CustomMap.UiSettings.ZoomControlsEnabled = false;
            CustomMap.UiSettings.MyLocationButtonEnabled = true;
            Current.CustomMap = CustomMap;
        }

        /*---------------------------------*/
        /*Map functions and event handeling*/
        void ChangeMapStyle()
        {
            var assembly = typeof(MapPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"SendMeADrink_Official.MapRenders.Night.json");
        
            string styleFile;

            using (var reader = new System.IO.StreamReader(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            CustomMap.MapStyle = GoogleMaps.MapStyle.FromJson(styleFile);
        }

        public async void InitMapAndTracking()
        {
            Geolocator.Position position = await GetUserLocation();

            //move the users position on the map
            GoogleMaps.Position LocationUser = new GoogleMaps.Position(position.Latitude, position.Longitude);
            CustomMap.InitialCameraUpdate = GoogleMaps.CameraUpdateFactory.NewPositionZoom(LocationUser, 17.5);

            //Store the long and lat data in the CU variable
            Current.CU.Latitude = position.Latitude;
            Current.CU.Longitude = position.Longitude;

            await GetPlaces(position); 
            await StartListening();
        }

        public static async Task<Geolocator.Position> GetUserLocation()
        {
            Geolocator.Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 1;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(1), null, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
            {
                return null;
            }

            return position;
        }

        /*Move the user on the map*/
        public async Task UpdateUserLocation(Geolocator.Position position)
        {
            //move the users position on the map
            GoogleMaps.Position LocationUser = new GoogleMaps.Position(position.Latitude, position.Longitude);
            await CustomMap.AnimateCamera(GoogleMaps.CameraUpdateFactory.NewPositionZoom(LocationUser, 17.5));

            //Store the long and lat data in the CU variable
            Current.CU.Latitude = position.Latitude;
            Current.CU.Longitude = position.Longitude;
        }

        /*Function to get all places nearby*/
        public async Task GetPlaces(Geolocator.Position position)
        {
            HttpClient client = new HttpClient(new HttpClientHandler());

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Longitude", position.Longitude.ToString()),
                new KeyValuePair<string, string>("Latitude", position.Latitude.ToString()),
            });

            HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Finder/GetPlaces.php", content); //send the variable content to the database as a POST method
            var DBOutput = await res.Content.ReadAsStringAsync();
            IList<Place> PlacesList = JsonConvert.DeserializeObject<IList<Place>>(DBOutput); //A list of places received from the database

            Current.Places = PlacesList; //Used to store all the places nearby (Variable is used when the page loads)
            Current.PlacesListView.ItemsSource = PlacesList; //Used to change the list of places nearby when the location changes
        }

        /*Listens for location changes every second*/
        async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
            {
                return;
            }
            else
            {
                await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, true);

                CrossGeolocator.Current.PositionChanged += PositionChanged;
                CrossGeolocator.Current.PositionError += PositionError;
            }
        }

        /*Handle the position change event*/
        private async void PositionChanged(object sender, Geolocator.PositionEventArgs e)
        {
            var position = e.Position; //Used to store all the users current positions data

            await UpdateUserLocation(position);
            await GetPlaces(position);
        }

        /*Handle the error message*/
        private void PositionError(object sender, Geolocator.PositionErrorEventArgs e)
        {
            //Handle event here for errors
            Console.WriteLine(e.Error);
        }

        /*---------------------------------*/
        /*Navigation menu*/
        private void Menu_Clicked(object sender, EventArgs e)
        {
            IsPresented = true; //This will present the MasterDetailPage.Master (Menu)
        }

        private void MapButton_Clicked(object sender, EventArgs e)
        {
            IsPresented = false; //This will hide the MasterDetailPage.Master (Menu)
        }

        private async void ProfileButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("Would you like to logout", null, "Yes", "No");

            if (res == true)
            {
                Preferences.Remove("IsUserLoggedIn");
                Application.Current.MainPage = new NavigationPage(new Login());
            }
        }
    }
}