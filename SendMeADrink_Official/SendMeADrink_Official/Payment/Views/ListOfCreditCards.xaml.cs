using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SendMeADrink_Official.Database;
using System.Net.Http;

namespace SendMeADrink_Official.Payment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListOfCreditCards : ContentPage
    {
        readonly App Current = ((App)App.Current);

        public ListOfCreditCards()
        {
            InitializeComponent();

            Current.CreditCardsListView = ListOfAvailableCreditCards;
            ListOfAvailableCreditCards.ItemsSource = CheckAllCards(Current.CreditCards);
        }

        //Used to go through all the cards added to the Current.Creditcards
        public static IList<Creditcard> CheckAllCards(IList<Creditcard> CreditCards)
        {
            foreach (Creditcard Card in CreditCards)
            {
                /*Checks if the cardnumber contains a "-" between each 4 numbers*/
                if(!Card.CardNumber.Contains("-"))
                {
                    //Used to format the cardnumber to ####-####-####-####
                    Card.CardNumber = string.Format("{0}-{1}-{2}-{3}", Card.CardNumber.Substring(0, 4), Card.CardNumber.Substring(4, 4), Card.CardNumber.Substring(8, 4), Card.CardNumber.Substring(12, 4));
                }
            }

            return CreditCards;
        }

        /*Handels the delete button event*/
        public async void DeleteCard(object sender, EventArgs e)
        {
            var SelectedCard = ((MenuItem)sender).CommandParameter.ToString();

            HttpClient client = new HttpClient(new HttpClientHandler());

            /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("User_Id", Current.CU.Id),
                new KeyValuePair<string, string>("CardNumber", SelectedCard)
            });

            await client.PostAsync("http://send-meadrink.com/SMAD_App/Payment/DeleteCard.php", content);

            Current.CreditCards = await Login.GetCards(Current.CU.Id); //Calls the function to get all the cards from the logged in user
            Current.CreditCardsListView.ItemsSource = CheckAllCards(Current.CreditCards); //Changes the ItemSoure of the list
        }

        /*Navigates to the previous page*/
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /*Navigates to the credit card page*/
        private async void AddCardButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreditCardPage());
        }
    }
}