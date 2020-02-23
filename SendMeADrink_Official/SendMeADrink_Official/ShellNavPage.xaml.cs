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

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            Current.FlyoutIsPresented = true;
        }
    }
}