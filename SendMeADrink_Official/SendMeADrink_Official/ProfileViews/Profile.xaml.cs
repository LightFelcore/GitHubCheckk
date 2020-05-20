using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using SendMeADrink_Official.Payment.Views;

namespace SendMeADrink_Official.ProfileViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        readonly App Current = ((App)App.Current);

        public Profile()
        {
            InitializeComponent();
            BindingContext = Current.CU; //Change the bindingcontext of the page to Current.CU
        }

        /*-----------------*/
        /*Navigation*/
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /*Let a popup appear to change your data*/
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new UpdateData());
        }

        /*Navigate to the ListOfCreditCards page*/
        private async void PaymentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListOfCreditCards());
        }
    }
}
