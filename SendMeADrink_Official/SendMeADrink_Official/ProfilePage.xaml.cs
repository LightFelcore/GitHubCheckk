using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendMeADrink_Official.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = ((App)App.Current).CU;
        }

        /*-----------------*/
        /*Navigation*/
        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            //MasterDetailMapPage.IsPresented = true;
        }
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("OI",null,"Close");
            //await PopupNavigation.Instance.PushAsync(new PopupUpdateData(u));
        }
    }
}
