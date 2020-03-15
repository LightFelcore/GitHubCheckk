using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendMeADrink_Official.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Services;
using System.Net.Http;

namespace SendMeADrink_Official
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        string _Id;
        
        public ProfilePage(string Id)
        {
            InitializeComponent();
            GetUserData(Id);
            
            
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            
            PopupNavigation.Instance.PushAsync(new PopUpUpdateData(_Id));
        }

        readonly HttpClient client = new HttpClient(new HttpClientHandler());
        

        public async void GetUserData(string Id)
        {
            user u = new user();

            HttpResponseMessage res;
            u.Id = Id;
            

            
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Id", u.Id)
            });

            
            res = await client.PostAsync("http://10.0.2.2/DATA/USER/DataByID/UsernameById.php", content);
            
            u.Username = await res.Content.ReadAsStringAsync();


            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine(u.Username);

            res = await client.PostAsync("http://10.0.2.2/DATA/USER/DataByID/EmailById.php", content);
            u.Email = await res.Content.ReadAsStringAsync();
            

            //Console.WriteLine(u.Email);

            res = await client.PostAsync("http://10.0.2.2/DATA/USER/DataByID/PasswdById.php", content);
            u.Passwd = await res.Content.ReadAsStringAsync();

            //Console.WriteLine(u.Passwd);

            res = await client.PostAsync("http://10.0.2.2/DATA/USER/DataByID/AgeById.php", content);
            u.Age = await res.Content.ReadAsStringAsync();

            //Console.WriteLine(u.Age);

            BindingContext = u;
        }

        
    }

    
}
