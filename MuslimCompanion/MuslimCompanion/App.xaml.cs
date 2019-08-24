using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using MuslimCompanion.Core;
using Matcha.BackgroundService;
using static MuslimCompanion.Core.GeneralManager;
using MuslimCompanion.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MuslimCompanion
{
    public partial class App : Application
    {

        public static string DatabaseLocation;

        //public App()
        //{

        //    InitializeComponent();

        //    MainPage = new NavigationPage(new Search());

        //}

        public App(string databaseLocation)
        {

            InitializeComponent();

            DatabaseLocation = databaseLocation;

            if (GeneralManager.conn == null)
                GeneralManager.InitConnection();

            MainPage = new NavigationPage(new BasePage());

        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //Register Periodic Tasks
            //BackgroundAggregatorService.Add(() => new AzanBackgroundService());
            BackgroundAggregatorService.Add(() => new LocatorBackgroundService());

            //Start the background service
            BackgroundAggregatorService.StartBackgroundService();
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
