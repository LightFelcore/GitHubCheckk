using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailMapPage : MasterDetailPage
    {
        public MasterDetailMapPage()
        {
            InitializeComponent();
            GetUserLocation();
        }

        public async void GetUserLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(0.5)));
        }

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            //MasterDetailMapPage.IsPresented = true;
        }

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

        private void LogoutButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}