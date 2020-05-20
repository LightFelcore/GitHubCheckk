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

            BindingContext = ((App)App.Current).SelectedItem; //Change the bindingcontext of this page to the SelectedItem
        }
    }
}