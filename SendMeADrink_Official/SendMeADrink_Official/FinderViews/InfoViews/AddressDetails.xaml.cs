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

            BindingContext = ((App)App.Current).SelectedItem; //Change the bindingcontext of this page to the SelectedItem
            PostalcodeAndRegion.Text = PostalData; //Changes the PostalCodeAndRegion variable it's text to PostalData
        }

        /*--------------------------*/
        /*String format for postalcode and city*/
        public string PostalData
        {
            get { return string.Format("{0} {1}", ((App)App.Current).SelectedItem.Postalcode, ((App)App.Current).SelectedItem.Region); }
        }
    }
}