using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using MuslimCompanion.Core;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MuslimCompanion
{
    public partial class App : Application
    {

        public static string DatabaseLocation;

        public App()
        {

            InitializeComponent();

            MainPage = new NavigationPage(new Search());

        }

        public App(string databaseLocation)
        {

            InitializeComponent();

            if (GeneralManager.conn == null)
                GeneralManager.InitConnection();

            MainPage = new NavigationPage(new Search());

            DatabaseLocation = databaseLocation;

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
