using System;
using System.IO;
using Xamarin.Forms;
using SendMeADrink_Official.Database;


namespace SendMeADrink_Official
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }
        public user CU { get; set; } // current user
        public bar BI { get; set; } // bar info

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}