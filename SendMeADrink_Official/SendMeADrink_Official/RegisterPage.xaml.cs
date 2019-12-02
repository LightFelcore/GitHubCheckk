using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void SUButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("", "Succesfull sign up", "Close");
        }

        public void ShowPassword(object sender, EventArgs args)
        {
            Password.IsPassword = Password.IsPassword ? false : true;
            RepeatPassword.IsPassword = RepeatPassword.IsPassword ? false : true;
        }
    }
}