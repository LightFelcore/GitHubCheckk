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
    public partial class ShellNavPage : Shell
    {
        public ShellNavPage()
        {
            InitializeComponent();
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {

            var res = await DisplayAlert("Would you like to logout?", null, "Yes", "Cancel");
            
            if(res == true)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }
    }

    
}
