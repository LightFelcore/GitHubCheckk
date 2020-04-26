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
    public partial class AddressDetails : ContentView
    {
        public AddressDetails()
        {
            InitializeComponent();

            BindingContext = ((App)App.Current).SelectedItem;
            PostalcodeAndRegion.Text = PostalData;
        }

        /*--------------------------*/
        /*String format for postalcode and city*/
        public string PostalData
        {
            get { return string.Format("{0} {1}", ((App)App.Current).SelectedItem.Postalcode, ((App)App.Current).SelectedItem.Region); }
        }
    }
}