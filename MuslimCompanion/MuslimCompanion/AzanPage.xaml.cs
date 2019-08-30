using MuslimCompanion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Settings.Abstractions;
using Plugin.Settings;

namespace MuslimCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class AzanPage : ContentPage
	{

        public static AzanPage instance;

        public static ISettings AppSettings =>
    CrossSettings.Current;

        public AzanPage ()
		{

            instance = this;

            InitializeComponent();
            PrayerTimeChecker();
            if (Application.Current.Properties.ContainsKey("azannotification"))
                NotificationToggle.IsToggled = (bool)Application.Current.Properties["azannotification"];
            else
                Application.Current.Properties.Add("azannotification", AppSettings.GetValueOrDefault("azannotification", false));

            NotificationToggle.IsToggled = AppSettings.GetValueOrDefault("azannotification", false);

            UpdateLocationText();

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                OnModifyWordClicked();
            };
            ModifyWord.GestureRecognizers.Add(tapGestureRecognizer);

        }

        void OnModifyWordClicked()
        {

            Navigation.PushAsync(new LocationSelectionPage());

        }

        void PrayerTimeChecker()
        {

            //Update prayer times immediately on view load if ready

            if (GeneralManager.prayertimes != null)
            {

                if (GeneralManager.prayertimes.Count >= 5)
                {

                    UpdatePrayerTimesUI();

                }

            }

            //Update prayer times every 1 second

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (GeneralManager.prayertimes != null)
                {

                    if (GeneralManager.prayertimes.Count >= 5)
                    {

                        UpdatePrayerTimesUI();

                    }
                    else
                    {

                        ResetToLoading();

                    }

                }

                else
                {

                    ResetToLoading();

                }

                return true; // True = Repeat again, False = Stop the timer

            });


        }

        public void UpdatePrayerTimesUI()
        {

            Value1.Text = GeneralManager.prayertimes[0].FireTime.ToShortTimeString();
            Value2.Text = GeneralManager.prayertimes[1].FireTime.ToShortTimeString();
            Value3.Text = GeneralManager.prayertimes[2].FireTime.ToShortTimeString();
            Value4.Text = GeneralManager.prayertimes[3].FireTime.ToShortTimeString();
            Value5.Text = GeneralManager.prayertimes[4].FireTime.ToShortTimeString();
            Name1.Text = ConvertPrayerNameToArabic(GeneralManager.prayertimes[0].PrayerType);
            Name2.Text = ConvertPrayerNameToArabic(GeneralManager.prayertimes[1].PrayerType);
            Name3.Text = ConvertPrayerNameToArabic(GeneralManager.prayertimes[2].PrayerType);
            Name4.Text = ConvertPrayerNameToArabic(GeneralManager.prayertimes[3].PrayerType);
            Name5.Text = ConvertPrayerNameToArabic(GeneralManager.prayertimes[4].PrayerType);

        }

        public void ResetToLoading()
        {

            Value1.Text = "...";
            Value2.Text = "...";
            Value3.Text = "...";
            Value4.Text = "...";
            Value5.Text = "...";
            Name1.Text = "جاري التحميل";
            Name2.Text = "جاري التحميل";
            Name3.Text = "جاري التحميل";
            Name4.Text = "جاري التحميل";
            Name5.Text = "جاري التحميل";

        }

        public void UpdateLocationText()
        {

            MainLabel.Text = "بتوقيت: " + Application.Current.Properties["cityname"];

        }

        public static string ConvertPrayerNameToArabic(string englishPrayerName)
        {

            string toReturn = "";

            switch(englishPrayerName.ToLower())
            {

                case "fajr":

                    toReturn = "الفجر";

                    break;

                case "dhuhr":

                    toReturn = "الظهر";

                    break;

                case "asr":

                    toReturn = "العصر";

                    break;

                case "maghrib":

                    toReturn = "المغرب";

                    break;

                case "isha":

                    toReturn = "العشاء";

                    break;

            }

            return toReturn;

        }

        private void NotificationToggle_Toggled(object sender, ToggledEventArgs e)
        {

            AppSettings.AddOrUpdateValue("azannotification", NotificationToggle.IsToggled);
            Application.Current.Properties["azannotification"] = NotificationToggle.IsToggled;

        }
    }

}