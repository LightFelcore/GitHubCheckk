using System.ComponentModel;
using SendMeADrink_Official.Payment.ViewModels;
using Xamarin.Forms;
using System.Net.Http;
using System.Collections.Generic;

namespace SendMeADrink_Official.Payment.Views
{
    [DesignTimeVisible(false)]
    public partial class CreditCardPage : ContentPage
    {
        readonly App Current = (App)App.Current;
        public CreditCardPage()
        {
            InitializeComponent();
            this.BindingContext = new CreditCardPageViewModel();
        }

        private async void AddCard_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CardNum.Text) || string.IsNullOrWhiteSpace(Expires.Text) || string.IsNullOrWhiteSpace(CVC.Text))
            {
                ErrorMessage.Text = "Please enter all your card infromation";
            }
            else
            {
                HttpClient client = new HttpClient(new HttpClientHandler());

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("User_Id", Current.CU.Id),
                    new KeyValuePair<string, string>("CardNumber", CardNum.Text.Replace("-", string.Empty)),
                    new KeyValuePair<string, string>("ExpireDate", Expires.Text.Replace("/", string.Empty)),
                    new KeyValuePair<string, string>("CVC", CVC.Text)
                });

                await client.PostAsync("http://send-meadrink.com/SMAD_App/Payment/AddCard.php", content);

                Current.CreditCards = await Login.GetCards(Current.CU.Id);
                Current.CreditCardsListView.ItemsSource = ListOfCreditCards.CheckAllCards(Current.CreditCards);

                await DisplayAlert("Your card has been added successfully!", null, null, "OK");

                await Navigation.PopAsync();
            }
        }

        private async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}