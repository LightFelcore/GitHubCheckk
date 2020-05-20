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
            BindingContext = new CreditCardPageViewModel();
        }

        /*AddCard button clicked handeler*/
        private async void AddCard_Clicked(object sender, System.EventArgs e)
        {
            /*Checks if the input fields aren't empty or do not contain only a white space*/
            if (string.IsNullOrWhiteSpace(CardNum.Text) || string.IsNullOrWhiteSpace(Expires.Text) || string.IsNullOrWhiteSpace(CVC.Text))
            {
                ErrorMessage.Text = "Please enter all your card infromation"; //Show an error message
            }
            else
            {
                HttpClient client = new HttpClient(new HttpClientHandler());

                /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("User_Id", Current.CU.Id),
                    new KeyValuePair<string, string>("CardNumber", CardNum.Text.Replace("-", string.Empty)),
                    new KeyValuePair<string, string>("ExpireDate", Expires.Text.Replace("/", string.Empty)),
                    new KeyValuePair<string, string>("CVC", CVC.Text)
                });

                await client.PostAsync("http://send-meadrink.com/SMAD_App/Payment/AddCard.php", content); //Calling the database

                Current.CreditCards = await Login.GetCards(Current.CU.Id); //Calls the function to get all the cards from the logged in user
                Current.CreditCardsListView.ItemsSource = ListOfCreditCards.CheckAllCards(Current.CreditCards); //Changes the ItemSoure of the list

                await DisplayAlert("Your card has been added successfully!", null, null, "OK"); //Inform the user that the card has been addres

                await Navigation.PopAsync(); //Navigates to the previous page
            }
        }

        /*Navigates to the previous page*/
        private async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}