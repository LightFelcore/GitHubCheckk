using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace SendMeADrink_Official.ProfileViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateData
    {
        readonly App Current = (App)App.Current;

        public UpdateData()
        {
            InitializeComponent();
            BindingContext = Current.CU; //Change the bindingcontext of the page to Current.CU
        }

        /*Save button clicked handeler*/
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            /*Checks if the input fields aren't empty or do not contain only a white space*/
            if (string.IsNullOrWhiteSpace(UpdateDataUsername.Text) || string.IsNullOrWhiteSpace(UpdateDataEmail.Text) || string.IsNullOrWhiteSpace(UpdateDataAge.Text))
            {
                ErrorMessage.Text ="Enter all information"; //Show an error message
            }
            else
            {
                /*Checks if the entered data isn't the same as the data currently in the database*/
                if(UpdateDataUsername.Text != Current.CU.Username || UpdateDataEmail.Text != Current.CU.Email || UpdateDataAge.Text != Current.CU.Age)
                {
                    HttpClient client = new HttpClient(new HttpClientHandler());

                    /*Creating a new variable of the type "FormUrlEncodedContent" to store the data that will be send to our database*/
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Id", Current.CU.Id),
                        new KeyValuePair<string, string>("Username", UpdateDataUsername.Text),
                        new KeyValuePair<string, string>("Email", UpdateDataEmail.Text),
                        new KeyValuePair<string, string>("Age", UpdateDataAge.Text),
                    });

                    await client.PostAsync("http://send-meadrink.com/SMAD_App/Update/update.php", content); //Calling the database

                    /*Storing the updated data in the CU variable*/
                    Current.CU.Username = UpdateDataUsername.Text;
                    Current.CU.Email = UpdateDataEmail.Text;
                    Current.CU.Age = UpdateDataAge.Text;

                    await PopupNavigation.Instance.PopAsync(); //Make the popup disappaer
                    Application.Current.MainPage = new NavigationPage(new MapPage()); //Navigate back to the Map page
                }
                else
                {
                    ErrorMessage.Text = "The entered data is the same!"; //Show an error message
                }
            }
        }
    }
}