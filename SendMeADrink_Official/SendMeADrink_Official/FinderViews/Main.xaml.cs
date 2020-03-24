using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SendMeADrink_Official.FinderViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        public Main()
        {
            InitializeComponent();
        }

        private async void FinderGoUp(object sender, SwipedEventArgs e)
        {
            if (Finder.TranslationY == 587.5)
            {
                await Finder.TranslateTo(0, 400, 200);
            }
            else if (Finder.TranslationY == 400)
            {
                await Finder.TranslateTo(0, 64, 200);
            }
        }

        private async void FinderGoDown(object sender, SwipedEventArgs e)
        {
            if (Finder.TranslationY == 64)
            {
                await Finder.TranslateTo(0, 400, 200);
            }
            else if (Finder.TranslationY == 400)
            {
                await Finder.TranslateTo(0, 587.5, 200);
            }
        }

        public async void SearchBar_Focused(object sender, FocusEventArgs e)
        {
            await Finder.TranslateTo(0, 64, 200);
        }

        private async void SearchBar_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBar.Text))
            {
                await Finder.TranslateTo(0, 400, 200);
            }
        }

        private void BarOrClubItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            //to be added
        }
    }
}