using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SendMeADrink_Official
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void LIButton_Clicked(object sender, EventArgs e)
        {
            if(Email != null || PasswordSignIn != null)
            {
                DisplayAlert("", "Succesfull login", "Close");
            }
            else
            {
                DisplayAlert("", "You haven't entered all the details", "Close");
            }
        }
        private void SUButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("", "Succesfull login", "Close");
        }

        public void ShowPassword(object sender, EventArgs args)
        {
            PasswordSignIn.IsPassword = PasswordSignIn.IsPassword ? false : true;
        }
    }
}
