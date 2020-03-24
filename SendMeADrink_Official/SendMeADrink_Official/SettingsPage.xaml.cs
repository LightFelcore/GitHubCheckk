using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            MapMode.SelectedIndex = 0;
        }

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            //to be added
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {   
            await DisplayAlert("Settings Saved", null, null, "Close");
        }
    }
}