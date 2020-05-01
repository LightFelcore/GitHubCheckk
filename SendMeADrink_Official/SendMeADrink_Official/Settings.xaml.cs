using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        readonly App Current = (App)App.Current;

        public Settings()
        {
            InitializeComponent();

            AppTheme.SelectedIndex = Preferences.Get("AppTheme", 2);
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if(Preferences.Get("AppTheme", 2) != AppTheme.SelectedIndex)
            {
                Preferences.Set("AppTheme", AppTheme.SelectedIndex);

                //Changing the style of the map
                string styleFile = await MapPage.ChangeMapStyle();
                Current.CustomMap.MapStyle = MapStyle.FromJson(styleFile);
            }

            await DisplayAlert("Settings Saved", null, null, "Close");
        }
    }
}