using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SendMeADrink_Official.Database
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfoPage : ContentPage
    {
        public UserInfoPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //Get All Persons
            var personList = await App.Database.GetItemsAsync();
            if (personList != null)
            {
                UserList.ItemsSource = personList;
            }
        }
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Success", "User Deleted", "OK");
            /*
            //Get Person
            var person = await App.Database.GetItemAsync(Id);
            if (person != null)
            {
                //Delete Person
                await App.Database.DeleteItemAsync(person);
                await DisplayAlert("Success", "User Deleted", "OK");
                //Get All Persons
                var personList = await App.Database.GetItemsAsync();
                if (personList != null)
                {
                    UserList.ItemsSource = personList;
                }
            }*/
        }
        public async void EditButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Coming soon", null, "Close");
        }
    }
}