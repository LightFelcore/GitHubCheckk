using SendMeADrink_Official.Database;
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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        public async void SUButton_Clicked(object sender, EventArgs e)
        {    
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(AgeEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text) || string.IsNullOrWhiteSpace(RepeatPasswordEntry.Text))
            {
                await DisplayAlert("Please enter all your information", "", "Close");
            }
            else
            {
                if (PasswordEntry.Text == RepeatPasswordEntry.Text)
                {
                    Person person = new Person()
                    {
                        Username = UsernameEntry.Text,
                        Age = int.Parse(AgeEntry.Text),
                        Email = EmailEntry.Text,
                        Password = PasswordEntry.Text
                    };
                    //Add New Person
                    await App.Database.SaveItemAsync(person);
                    var result = await DisplayAlert("", "Succesfull sign up", "", "Close");
                    if (result == false)
                    {
                        await Navigation.PushAsync(new MainPage());
                    }
                }
                else
                {
                    await DisplayAlert("The entered passwords aren't the same", "", "Close");
                }
            }
            UsernameEntry.Text = AgeEntry.Text = EmailEntry.Text = PasswordEntry.Text = RepeatPasswordEntry.Text = string.Empty;
            //UserList.ItemsSource = await App.Database.GetPeopleAsync();
        }
        public void ShowPassword(object sender, EventArgs args)
        {
            PasswordEntry.IsPassword = PasswordEntry.IsPassword ? false : true;
            RepeatPasswordEntry.IsPassword = RepeatPasswordEntry.IsPassword ? false : true;
        }
    }

}