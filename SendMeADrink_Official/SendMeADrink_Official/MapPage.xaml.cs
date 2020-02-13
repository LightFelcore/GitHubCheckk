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
            InitializeComponent();
            GetUserLocation();
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
                        var map = new Xamarin.Forms.Maps.Map(MapSpan.FromCenterAndRadius(new Position(Latitude, Longitude), Distance.FromKilometers(0.001)))
                        {
                            IsShowingUser = true,
                            HeightRequest = 100,
                            WidthRequest = 960,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };
                        var stack = new StackLayout { Spacing = 0 };
                        stack.Children.Add(map);
                        Content = stack;
                    }
                    
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
    }
}