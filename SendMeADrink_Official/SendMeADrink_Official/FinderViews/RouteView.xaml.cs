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
    public partial class RouteView : ContentView
    {
        public RouteView()
        {
            InitializeComponent();
        }

        private async void FinderGoUp(object sender, SwipedEventArgs e)
        {
            if (Finder.TranslationY == 587.5)
            {
                await Finder.TranslateTo(0, 400, 200);
            }
        }

        private async void FinderGoDown(object sender, SwipedEventArgs e)
        {
            if (Finder.TranslationY == 400)
            {
                await Finder.TranslateTo(0, 587.5, 200);
            }
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            //to be added
        }

        private void InfoButton_Clicked(object sender, EventArgs e)
        {
            //to be added
        }

        private void RouteButton_Clicked(object sender, EventArgs e)
        {
            //to be added
        }

        
    }
}