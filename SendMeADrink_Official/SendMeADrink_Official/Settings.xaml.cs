using System;
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

            AppTheme.SelectedIndex = Preferences.Get("AppThemeToken", 2); //Get the value that is stored in tha variable AppThemeToken
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            /*Checks if the selected apptheme isn't the same as the current selected theme*/
            if (Preferences.Get("AppThemeToken", 2) != AppTheme.SelectedIndex)
            {
                Preferences.Set("AppThemeToken", AppTheme.SelectedIndex); //Storing the selected app theme index in the variable "AppThemeToken" 

                //Changing the style of the map
                string styleFile = await MapPage.ChangeMapStyle(); //Calls the function that will read the whole JSON file and returns a string
                Current.CustomMap.MapStyle = MapStyle.FromJson(styleFile); //Changing the map it's theme (style)
            }

            await DisplayAlert("Settings Saved", null, null, "Close");
        }
    }
}