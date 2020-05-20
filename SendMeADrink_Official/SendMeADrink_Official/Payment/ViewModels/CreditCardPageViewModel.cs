using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SendMeADrink_Official.Payment.ViewModels
{
    public class CreditCardPageViewModel : BindableObject
    {
        private string _cardNumber;
        private string _expiration;
        private string _cvc;

        public string CardNumber
        {
            get => _cardNumber; //Gets the value of CardNumber and stores it in _cardNumber
            set
            {
                _cardNumber = value; //Stores the value in _cardNumber
                OnPropertyChanged(); //Calls the OnPropertyChanged event handeler
            }
        }

        public string CardExpirationDate
        {
            get => _expiration; //Gets the value of CardExpirationDate and stores it in _expiration
            set
            {
                _expiration = value; //Stores the value in _expiration
                OnPropertyChanged(); //Calls the OnPropertyChanged event handeler
            }
        }

        public string CardCvv
        {
            get => _cvc; //Gets the value of CardCvv and stores it in _cvc
            set
            {
                _cvc = value; //Stores the value in _cvc
                OnPropertyChanged(); //Calls the OnPropertyChanged event handeler
            }
        }
    }
}