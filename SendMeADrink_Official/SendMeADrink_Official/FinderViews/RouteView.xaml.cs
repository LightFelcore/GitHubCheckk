using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;

namespace SendMeADrink_Official.FinderViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RouteView : ContentView
    {
        readonly App Current = (App)App.Current;

        public RouteView()
        {
            InitializeComponent();

            BindingContext = Current.SelectedItem; //Change the bindingcontext of this page to the SelectedItem

            //Defining the text value of "PlaceTypeAndDistance" and "PostalcodeAndRegion"
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
            await RootContent.FadeTo(0, 125); //Makes the RootContent of the current page fade out in 125 ms
            await Finder.TranslateTo(0, 400, 200);
            Current.FV.Children[0] = new MainView(); //Changes the value of the grid FV to the new MainView page

            Current.UpdateCamera = true;
            Position LocationUser = new Position(Current.CU.Latitude, Current.CU.Longitude);
            await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(LocationUser, 17.5), TimeSpan.FromSeconds(2.5));
        }

        //Handle the info button clicked event
        private async void InfoButton_Clicked(object sender, EventArgs e)
        {
            await SubContent.FadeTo(0, 125); //Makes the SubContent of the current page fade out in 125 ms
            await Finder.TranslateTo(0, 64, 200);
            Current.FV.Children[0] = new InfoView(); //Changes the value of the grid FV to the new InfoView page
        }

        //Handle the route button clicked event
        private void RouteButton_Clicked(object sender, EventArgs e)
        {
            double Latitude = Current.SelectedItem.Latitude + ((Current.CU.Latitude - Current.SelectedItem.Latitude)/2); //Variable that stores the latitude point between the selectedItem it's latitude and the user it's latitude position
            double Longitude = Current.SelectedItem.Longitude + ((Current.CU.Longitude - Current.SelectedItem.Longitude)/2); //Variable that stores the longitude point between the selectedItem it's longitude and the user it's longitude position
            double Zoom = 14; //zoom range: 2 - 21‬

            Task.Run(() => 
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Finder.TranslateTo(0, 587.5, 200);
                    Position FullRoutePosition = new Position(Latitude, Longitude);
                    await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(FullRoutePosition, Zoom), TimeSpan.FromSeconds(2.5)); //Animate the camera of the map to the FullRoute position of the user with a calculated zoom level and in a timespan of 2.5 seconds

                    Thread.Sleep(2500); //Thread will stop for 2.5 seconds

                    Current.UpdateCamera = true; //Allow camera updates
                    Position LocationUser = new Position(Current.CU.Latitude, Current.CU.Longitude);
                    await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(LocationUser, 17.5), TimeSpan.FromSeconds(2.5)); //Animate the camera of the map to the position of the user with a zoom of 17.5 and in a timespan of 2.5 seconds
                    await SubContent.FadeTo(0, 125); //Makes the SubContent of the current page fade out in 125 ms
                    await Finder.TranslateTo(0, 400, 200);
                    Current.FV.Children[0] = new DirectionsView(); //Changes the value of the grid FV to the new DirectionsView page
                });
            });
        }
    }
}