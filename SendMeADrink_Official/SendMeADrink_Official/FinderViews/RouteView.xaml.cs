using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Timers = System.Timers;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace SendMeADrink_Official.FinderViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RouteView : ContentView
    {
        readonly App Current = (App)App.Current;

        public RouteView()
        {
            InitializeComponent();

            BindingContext = Current.SelectedItem;
            PlaceTypeAndDistance.Text = PlaceTypeAndDistanceString;
            PostalcodeAndRegion.Text = PostalString;
        }

        /*--------------------------*/
        /*String formating*/
        public string PostalString
        {
            get { return string.Format("{0} {1}", Current.SelectedItem.Postalcode, Current.SelectedItem.Region); }
        }

        public string DistanceString
        {
            get 
            { 
                //The distance received from the database is in Km
                if(Current.SelectedItem.Distance >= 1)
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

        public string PlaceTypeAndDistanceString
        {
            get { return string.Format("{0}  •  {1}", Current.SelectedItem.Type, DistanceString); }
        }

        /*--------------------------*/
        /*Finder Controls/Navigation*/
        private async void FinderGoUp(object sender, SwipedEventArgs e)
        {
            if (Finder.TranslationY == 587.5)
            {
                await Finder.TranslateTo(0, 400, 200);
            }
        }

        private async void FinderGoDown(object sender, SwipedEventArgs e)
        {
            if (Finder.TranslationY == 400)
            {
                await Finder.TranslateTo(0, 587.5, 200);
            }
        }

        //Handle the close button clicked event
        private async void CloseButton_Clicked(object sender, EventArgs e)
        {          
            await RootContent.FadeTo(0, 125);
            await Finder.TranslateTo(0, 400, 200);
            Current.FV.Children[0] = new MainView();

            Position LocationUser = new Position(Current.CU.Latitude, Current.CU.Longitude);
            await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(LocationUser, 17.5), TimeSpan.FromSeconds(2.5));
        }

        //Handle the info button clicked event
        private async void InfoButton_Clicked(object sender, EventArgs e)
        {
            await SubContent.FadeTo(0, 125);
            await Finder.TranslateTo(0, 64, 200);
            Current.FV.Children[0] = new InfoView();
        }

        //Handle the route button clicked event
        private void RouteButton_Clicked(object sender, EventArgs e)
        {
            double Latitude = Current.SelectedItem.Latitude + ((Current.CU.Latitude - Current.SelectedItem.Latitude)/2);
            double Longitude = Current.SelectedItem.Longitude + ((Current.CU.Longitude - Current.SelectedItem.Longitude)/2);
            double Zoom = (Current.SelectedItem.Distance / 0.06479239943325303703703703703704) + 2; //zoom range: 2 - 21‬

            Task.Run(() => 
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Finder.TranslateTo(0, 587.5, 200);
                    Position FullRoutePosition = new Position(Latitude, Longitude);
                    await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(FullRoutePosition, Zoom), TimeSpan.FromSeconds(2.5));

                    Thread.Sleep(2500);

                    Position LocationUser = new Position(Current.CU.Latitude, Current.CU.Longitude);
                    await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(LocationUser, 17.5), TimeSpan.FromSeconds(2.5));
                    await SubContent.FadeTo(0, 125);
                    await Finder.TranslateTo(0, 400, 200);
                    Current.FV.Children[0] = new DirectionsView();
                });
            });
        }
    }
}