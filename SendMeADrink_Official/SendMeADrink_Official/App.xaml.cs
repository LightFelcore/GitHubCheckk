using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;
using SendMeADrink_Official.Database;
using Map = Xamarin.Forms.GoogleMaps.Map;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

namespace SendMeADrink_Official
{
    public partial class App : Application
    {
        public bool IsUserLoggedIn = Preferences.Get("IsUserLoggedIn", false); //Stores a bool that tells if a user checked Remember me or not
        public User CU { get; set; } //CU = Current User (logged In)
        public Map CustomMap { get; set; } //Map used by the user
        public Grid FV { get; set; } //Stores the 3 different finder views
        public IList<Place> Places { get; set; } //List that stores all the places
        public ListView PlacesListView { get; set; } //ListView that will list all places
        public Place SelectedItem { get; set; } //Stores the selected bar of clubs data

        public App()
        {
            InitializeComponent();

            //Check if user checked 'RememberMe' checkbox on login page
            if (!IsUserLoggedIn)
            {
                MainPage = new NavigationPage(new Login());
            }
            else
            {
                AutoLoginUser();
            }
        }

        public async void AutoLoginUser()
        {
            string PasswordToken = null;

            try
            {
                PasswordToken = await SecureStorage.GetAsync("PasswordToken");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to retrieve data from secure storage: " + ex);
            }

            HttpClient client = new HttpClient(new HttpClientHandler());

            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("Email", Preferences.Get("EmailToken", null)),
                    new KeyValuePair<string, string>("Passwd", PasswordToken),
            });

            HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Login/login.php", content);
            var json = await res.Content.ReadAsStringAsync();
            CU = JsonConvert.DeserializeObject<User>(json);

            MainPage = new NavigationPage(new MapPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}