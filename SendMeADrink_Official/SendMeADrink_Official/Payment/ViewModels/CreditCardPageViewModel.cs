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
            get => _cardNumber;
            set
            {
                _cardNumber = value;
                OnPropertyChanged();
            }
        }

        public string CardExpirationDate
        {
            get => _expiration;
            set { _expiration = value; OnPropertyChanged(); }
        }

        public string CardCvv
        {
            get => _cvc;
            set { _cvc = value; OnPropertyChanged(); }
        }
    }
}