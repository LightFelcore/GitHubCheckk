using System;
using System.Collections.Generic;
using System.Linq;
using SendMeADrink_Official.FinderViews.InfoViews;
using SendMeADrink_Official.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace SendMeADrink_Official.FinderViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoView : ContentView     
    {
        readonly App Current = (App)App.Current;
        public Label PrevInfoTab; //Used to store the previous tapped info view
        public Label PrevListTab; //Used to store the previous tapped list
        public IList<Drink> DrinkList { get; set; } //Used to store the list of drinks of a bar or club

        public InfoView()
        {
            InitializeComponent();
            GetDrinks(); //Calls a function to get all the drinks

            //Defining the text value of "Name" and "PlaceTypeAndDistance"
            Name.Text = Current.SelectedItem.Name;
            PlaceTypeAndDistance.Text = PlaceTypeAndDistanceString;
            
            //Default values on startup page
            PrevInfoTab = Address;
            InfoContent.Children.Add(new AddressDetails());

            PrevListTab = Beer; 
        }

        /*---------------------------*/
        /*Controls Finder Page*/
        private async void FinderGoUp(object sender, SwipedEventArgs e)
        {
            await Finder.TranslateTo(0, 64, 200);
        }

        private async void FinderGoDown(object sender, SwipedEventArgs e)
        {
            await Finder.TranslateTo(0, 587.5, 200);
        }

        /*---------------------------*/
        /*Button Handelings*/
        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await InfoViewContent.FadeTo(0, 125);
            await Finder.TranslateTo(0, 400, 200);
            Current.FV.Children[0] = new RouteView();
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

        /*String formatting the Place type and the distance into 1 string*/
        public string PlaceTypeAndDistanceString
        {
            get { return string.Format("{0}  •  {1}", Current.SelectedItem.Type, DistanceString); }
        }

        /*Info Buttons Handeling*/
        private void ChangeInfoView(object sender, EventArgs e)
        {
            Label TappedInfoView = (Label)sender; //Stores the data of the tapped type in the variable "TappedInfoView"
            string Id = TappedInfoView.Text;

            if (Id == "Opening Hours") 
            {
                Id = "OpeningHours";
            }

            //Styling for the tapped InfoView and the previous tapped InfoView
            PrevInfoTab.FontAttributes = FontAttributes.None;
            PrevInfoTab.TextColor = Color.LightGray;
            PrevInfoTab = TappedInfoView;
            TappedInfoView.TextColor = Color.White;
            TappedInfoView.FontAttributes = FontAttributes.Bold;

            //Switching the InfoContents view to the tapped InfoView
            switch (Id)
            {
                case "Address": InfoContent.Children[0] = new AddressDetails(); break;
                case "OpeningHours": InfoContent.Children[0] = new OpeningHoursDetails(); break;
                case "Contact": InfoContent.Children[0] = new ContactDetails(); break;
            }
        }

        /*List Of Drinks Buttons Handeling*/
        private void ChangeTypeOfDrink(object sender, EventArgs e)
        {
            Label TappedDrinkType = (Label)sender; //Stores the data of the tapped type in the variable "TappedDrinkType"

            //Styling for the tapped drink type and the previous tapped drink type
            PrevListTab.FontAttributes = FontAttributes.None;
            PrevListTab.TextColor = Color.LightGray;
            PrevListTab = TappedDrinkType;
            TappedDrinkType.TextColor = Color.White;
            TappedDrinkType.FontAttributes = FontAttributes.Bold;

            //Calling the "GetDrinksOfType()" function for a specific drink type
            switch (TappedDrinkType.Text)
            {
                case "Beer": GetDrinksOfType("Beer"); break;
                case "Cocktail": GetDrinksOfType("Cocktail"); break;
                case "Energy": GetDrinksOfType("Energy"); break;
                case "Soda": GetDrinksOfType("Soda"); break;
                default: break;
            }
        }

        /*---------------------------*/
        /*Getting all the drinks of a specific bar or club*/
        public async void GetDrinks()
        {
            HttpClient client = new HttpClient(new HttpClientHandler());

            /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Place_Id", Current.SelectedItem.Id)
            });

            HttpResponseMessage res = await client.PostAsync("http://send-meadrink.com/SMAD_App/Finder/GetDrinks.php", content); //send the variable content to the database as a POST method

            /*Checks if the data is retreived from the database*/
            if (res.IsSuccessStatusCode)
            {
                var DBOutput = await res.Content.ReadAsStringAsync();
                DrinkList = JsonConvert.DeserializeObject<IList<Drink>>(DBOutput);

                GetDrinksOfType("Beer"); //When InfoView is loaded we will need to load all the drinks of type "Beer" first and show them on the InfoView page
            }
        }

        /*Get all the drinks of a specific type*/
        public void GetDrinksOfType(string TypeOfDrink)
        {
            IList<Drink> FilteredList = new List<Drink>(); //List to store all the drinks of the chosen type
            IEnumerable<Drink> Query = DrinkList.Where(drink => (drink.TypeD == TypeOfDrink)); //Query to sort the DrinkList on the selected type of drink

            FilteredList = Query.ToList(); //Converting the Query (IEnumerable<Drink>) to an IList<Drink> that can be used to fill the Listview

            /*Checks if the list is empty*/
            if (FilteredList.Count == 0)
            {
                /*Creating a new drink and defining it's name as "Nothing available"*/
                Drink EmptyList = new Drink
                {
                    NameD = "Nothing available"
                };
                FilteredList.Add(EmptyList); //Adding EmptyList to the filteredList
            }

            ListOfDrinks.ItemsSource = FilteredList; //Defining the itemsource of the listofdrinks
        }

        /*Get all the information of the tapped drink*/
        private void Drink_Tapped(object sender, ItemTappedEventArgs e)
        {
            //Drink TappedDrink = (Drink)e.Item;
        }
    }
}