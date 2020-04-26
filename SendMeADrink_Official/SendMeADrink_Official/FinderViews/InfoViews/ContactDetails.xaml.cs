using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SendMeADrink_Official.FinderViews.InfoViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetails : ContentView
    {
        public ContactDetails()
        {
            InitializeComponent();

            BindingContext = ((App)App.Current).SelectedItem;
        }
    }
}