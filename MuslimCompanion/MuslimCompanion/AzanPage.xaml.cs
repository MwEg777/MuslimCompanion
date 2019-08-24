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

        public static ISettings AppSettings =>
    CrossSettings.Current;

        public AzanPage ()
		{

			InitializeComponent();
            PrayerTimeChecker();
            if (Application.Current.Properties.ContainsKey("azannotification"))
                NotificationToggle.IsToggled = (bool)Application.Current.Properties["azannotification"];
            else
                Application.Current.Properties.Add("azannotification", AppSettings.GetValueOrDefault("azannotification", false));

            NotificationToggle.IsToggled = AppSettings.GetValueOrDefault("azannotification", false);

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

                }

                return true; // True = Repeat again, False = Stop the timer

            });


        }

        void UpdatePrayerTimesUI()
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