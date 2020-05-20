using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using System;

namespace SendMeADrink_Official.FinderViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectionsView : ContentView
    {
        readonly App Current = (App)App.Current;

        public DirectionsView()
        {
            InitializeComponent();

            BindingContext = Current.SelectedItem; //Change the bindingcontext of this page to the SelectedItem
            PlaceTypeAndDistance.Text = PlaceTypeAndDistanceString; //Changes the PlaceTypeAndDistance variable it's text to PlaceTypeAndDistanceString
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

        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await RootContent.FadeTo(0, 125); //Makes the RootContent of the current page fade out in 125 ms
            await Finder.TranslateTo(0, 400, 200);
            Current.FV.Children[0] = new RouteView(); //Changes the value of the grid FV to the new RouteView page

            Current.UpdateCamera = false; //Prevents the camera from updating when the postion changes
            Position LocationUser = new Position(Current.SelectedItem.Latitude, Current.SelectedItem.Longitude);
            await Current.CustomMap.AnimateCamera(CameraUpdateFactory.NewPositionZoom(LocationUser, 17.5), TimeSpan.FromSeconds(2.5)); //Animate the camera of the map to the selected position with a zoom of 17.5 and in a timespan of 2.5 seconds
        }
    }
}