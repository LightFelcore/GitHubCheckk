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
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            GetUserLocation();
            InitializeComponent();
        }
        async void GetUserLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var Longitude = location.Longitude;
                    var Latitude = location.Latitude;

                    if (location.IsFromMockProvider)
                    {
                        // location is from a mock provider
                    }
                    else
                    {
                        var map = new Xamarin.Forms.Maps.Map(MapSpan.FromCenterAndRadius(new Position(Latitude, Longitude), Distance.FromKilometers(0.75)))
                        {
                            IsShowingUser = true,
                            HeightRequest = 100,
                            WidthRequest = 980,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };
                        var stack = new StackLayout { Spacing = 0 };
                        stack.Children.Add(map);
                        Content = stack;
                    }
                    
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException)
            {
                // Handle permission exception
            }
            catch (Exception)
            {
                // Unable to get location
            }
        }
        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Menu button clicked", "", "Close");
        }
    }
}