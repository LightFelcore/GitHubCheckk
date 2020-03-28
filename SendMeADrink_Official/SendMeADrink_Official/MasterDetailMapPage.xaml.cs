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
using SendMeADrink_Official.FinderViews;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailMapPage : MasterDetailPage
    {
        public MasterDetailMapPage()
        {
            InitializeComponent();
            MoveUser();
            
            BindingContext = ((App)App.Current).CU;
        }

        /*--------------------*/
        /*Move the user on the map*/
        public void MoveUser() //Location coördinates
        {
            //Position LocationUser = new Position(((App)App.Current).CU.Latitude, ((App)App.Current).CU.Longitude);
            Position LocationUser = new Position(51.090457, 4.553066);
            ((App)App.Current).CU.Longitude = 4.553066;
            ((App)App.Current).CU.Latitude = 51.090457;

            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(LocationUser, Distance.FromKilometers(0.5)));
        }

        /*-------------------*/
        /*Bar Finder*/
        

        /*-------------------*/
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
            await Navigation.PushAsync(new ProfilePage());
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("Would you like to logout", null, "Yes", "No");

            if (res == true)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }
    }
}