using System;
using System.IO;
using Xamarin.Forms;

namespace SendMeADrink_Official
{
    public partial class App : Application
    {
        static MyDatabase db;
        public static MyDatabase Database
        {
            get
            {
                if (db == null)
                {
                    db = new MyDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "people.db3"));
                }
                return db;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage (new MainPage());
        }

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