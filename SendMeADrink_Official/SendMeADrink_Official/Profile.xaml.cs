using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendMeADrink_Official.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Services;
using Newtonsoft.Json;
using Json.Net;
using SendMeADrink_Official.Payment.Views;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        readonly App Current = (App)App.Current;

        public Profile()
        {
            InitializeComponent();
            BindingContext = Current.CU;
        }

        /*-----------------*/
        /*Navigation*/
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new UpdateData());
        }

        private async void PaymentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreditCardPage());
        }
    }
}
