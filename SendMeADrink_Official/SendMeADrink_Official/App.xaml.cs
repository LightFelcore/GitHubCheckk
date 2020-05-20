using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;
using SendMeADrink_Official.Database;
using Map = Xamarin.Forms.GoogleMaps.Map;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SendMeADrink_Official
{
    public partial class App : Application
    {
        public bool IsUserLoggedIn = Preferences.Get("IsUserLoggedInToken", false); //Stores a bool that tells if a user checked Remember me or not
        public User CU { get; set; } //CU = Current User (logged In)
        public IList<Creditcard> CreditCards{ get; set; } //Stores all the added credit cards of the user
        public ListView CreditCardsListView { get; set; } //Listview that will list all credit cards

        public Map CustomMap { get; set; } //Map used by the user
        public bool UpdateCamera { get; set; } //Stores if camera tracking is used to follow the user

        public Grid FV { get; set; } //Stores the 3 different finder views
        public IList<Place> Places { get; set; } //List that stores all the places
        public ListView PlacesListView { get; set; } //ListView that will list all places
        public Place SelectedItem { get; set; } //Stores the selected bar of clubs data

        public App()
        {
            InitializeComponent();
            UpdateCamera = true; //Used to start updating the camera to the users position

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

        /*Function used to login the user on startup of the app*/
        public async void AutoLoginUser()
        {
            string PasswordToken = null; //Variable that stores the password of the user

            try
            {
                PasswordToken = await SecureStorage.GetAsync("PasswordToken"); //Getting the password of the user from the secure storage
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to retrieve data from secure storage: " + ex); //Error message if something goes wrong while retrieving the data
            }

            /*Using a new task to call our database*/
            await Task.Run(async () => 
            {
                HttpClient client = new HttpClient(new HttpClientHandler());

                /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", Preferences.Get("EmailToken", null)),
                    new KeyValuePair<string, string>("Passwd", PasswordToken)
                });

                HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Login/login.php", content); //Calling the database

                /*Checks if the data is retreived from the database*/
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    CU = JsonConvert.DeserializeObject<User>(json); //Stores the data from the database in the variable CU

                    CreditCards = await Login.GetCards(CU.Id); //Calls the function to get all the cards added by the logged in user

                    MainPage = new NavigationPage(new MapPage());
                }
            });
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