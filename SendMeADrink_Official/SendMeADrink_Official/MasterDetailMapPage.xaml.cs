using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using SendMeADrink_Official.Database;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailMapPage : MasterDetailPage
    {
        public IList<Bar> BarOrClub { get; set; }

        public MasterDetailMapPage()
        {
            InitializeComponent();
            //Location coördinates = GetUserLocationAsync().Result;
            MoveUser();// pass coördinates
            
            //BindingContext = ((App)App.Current).CU;

            Location coördinates = new Location(51.090457, 4.553066);
            Location LocationBarOrClub = new Location(51.098731, 4.564226);

            BarOrClub = new List<Bar>
            {
                new Bar
                {
                    BarName = "Den BlauwenHoek",
                    Distance = Math.Round(Location.CalculateDistance(coördinates, LocationBarOrClub, DistanceUnits.Kilometers), 2)
                },

                new Bar
                {
                    BarName = "Het GildenHuis",
                    Distance = Math.Round(Location.CalculateDistance(coördinates, LocationBarOrClub, DistanceUnits.Kilometers), 2)
                },

                new Bar
                {
                    BarName = "De Lateirn",
                    Distance = Math.Round(Location.CalculateDistance(coördinates, LocationBarOrClub, DistanceUnits.Kilometers), 2)
                }
            };

            BindingContext = this;
        }

        /*--------------------*/
        /*Getting the users location*//*
        public async Task<Location> GetUserLocationAsync()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best);
                Location coördinates = await Geolocation.GetLocationAsync(request);

                if (coördinates != null)
                {
                    return coördinates;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        */
        public async void MoveUser() //Location coördinates
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best);
            Location coördinates = await Geolocation.GetLocationAsync(request);
            Position LocationUser = new Position(coördinates.Latitude, coördinates.Longitude);
            //Position LocationUser = new Position(51.090457, 4.553066);
 
            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(LocationUser, Distance.FromKilometers(0.5)));
        }
        
        /*-------------------*/
        /*Bar Finder*/
        private async void FinderGoUp(object sender, SwipedEventArgs e)
        {
            if(Finder.TranslationY == 587.5)
            {
                await Finder.TranslateTo(0, 400, 200);
            }
            else if(Finder.TranslationY == 400)
            {
                await Finder.TranslateTo(0, 64, 200);
            }
        }

        private async void FinderGoDown(object sender, SwipedEventArgs e)
        {
            if(Finder.TranslationY == 64)
            {
                await Finder.TranslateTo(0, 400, 200);
            }
            else if(Finder.TranslationY == 400)
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
            if(string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                await Finder.TranslateTo(0, 400, 200);
            }
        }

        private void BarOrClubItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            //to be added
        }

        /*-------------------*/
        /*Navigation menu*/
        private async void MapButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MasterDetailMapPage());
        }

        private async void ProfileButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("Would you like to logout", null, "Yes", "No");

            if(res == true)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }
    }
}