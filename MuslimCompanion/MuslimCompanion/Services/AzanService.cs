using Matcha.BackgroundService;
using MuslimCompanion.Core;
using MuslimCompanion.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static MuslimCompanion.Core.GeneralManager;
using static MuslimCompanion.Model.PrayerTimeResponseModel;

namespace MuslimCompanion.Services
{

    public static class AzanBackgroundService
    {

        //public List<DateTime> GetPrayerTimesList()
        //{

        //    RootObject rootObject = GlobalVar.Get<RootObject>("prayersresponse");

        //    List<DateTime> todayPrayers = new List<DateTime>();

        //    List<string> prayerStringsToLoopOn = new List<string>();

        //    prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1].timings.Fajr);

        //    prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1].timings.Dhuhr);

        //    prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1].timings.Asr);

        //    prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1].timings.Maghrib);

        //    prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1].timings.Isha);

        //    foreach(string prayer in prayerStringsToLoopOn)
        //    {

        //        string[] prayerTimeStringParts = prayer.Split('(');

        //        string[] prayerTimeHoursAndMinutes = prayerTimeStringParts[0].Split(':');

        //        todayPrayers.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        //            int.Parse(prayerTimeHoursAndMinutes[0]), int.Parse(prayerTimeHoursAndMinutes[1]), 0));

        //    }

        //    return todayPrayers;

        //}

        public static List<Tuple<DateTime, string>> GetNextPrayersList(int prayerCount = 50)
        {

            RootObject rootObject = GlobalVar.Get<RootObject>("prayersresponse");

            List<DateTime> prayersTimes = new List<DateTime>();

            List<string> prayerTypes = new List<string>();

            List<Tuple<DateTime, string>> toReturn = new List<Tuple<DateTime, string>>();

            List<string> prayerStringsToLoopOn = new List<string>();

            for (int i=0; i < (int)( prayerCount / 5 ); i++)
            {

                #region Daily prayers adding

                //Adding the time string of the 5 prayers of one day to the prayerStringsToLoopOn list of strings

                if (rootObject.data.Count < (DateTime.Now.Day + i)) //If all remaining days are fetched
                    continue;

                prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1+ i].timings.Fajr);

                prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1 + i].timings.Dhuhr);

                prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1 + i].timings.Asr);

                prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1 + i].timings.Maghrib);

                prayerStringsToLoopOn.Add(rootObject.data[DateTime.Now.Day - 1 + i].timings.Isha);

                #endregion

            }

            int j = 0, k = 0;

            foreach (string prayer in prayerStringsToLoopOn)
            {

                string[] prayerTimeStringParts = prayer.Split('(');

                string[] prayerTimeHoursAndMinutes = prayerTimeStringParts[0].Split(':');

                //prayersTimes.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + (int)(j/5),
                    //int.Parse(prayerTimeHoursAndMinutes[0]), int.Parse(prayerTimeHoursAndMinutes[1]), 0));

                string prayerTypeToAdd = "Fajr";

                switch (k)
                {

                    case 0:

                        prayerTypeToAdd = "Fajr";

                        break;

                    case 1:

                        prayerTypeToAdd = "Dhuhr";

                        break;

                    case 2:

                        prayerTypeToAdd = "Asr";

                        break;

                    case 3:

                        prayerTypeToAdd = "Maghrib";

                        break;

                    case 4:

                        prayerTypeToAdd = "Isha";

                        break;

                }

                toReturn.Add(new Tuple<DateTime, string>(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + (int)(j / 5),
                    int.Parse(prayerTimeHoursAndMinutes[0]), int.Parse(prayerTimeHoursAndMinutes[1]), 0),
                    prayerTypeToAdd));

                j++;
                k++;
                if (k >= 5)
                    k = 0;

            }

            return toReturn;

        }

        public static bool jsonIsParsed { get; set; }

        public static string url { get; set; }

        //public RootObject rootObject;

        public static List<DateTime> todayPrayerTimes;

        //public async Task<bool> StartJob()
        //{
        //    //Check for location first

        //    while(muslimPosition == null)
        //    {

        //        await Task.Delay(100);

        //    }  

        //    DateTime dt = DateTime.Now;

        //    //Uncomment this line to reset prayer time data and re-query them from API
        //    //AppSettings.Remove("prayertimes_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString());

        //    if (!AppSettings.Contains("prayertimes_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString())) //This should contain a json string from the API response
        //    {

        //        //If prayer times not found locally : Make a GET Request to API to retrieve prayer times and store them in app settings as json

        //        //Prepare a URL for API request
        //        url = GeneralManager.GeneratePrayerAPIRequest();

        //        HttpClient _client = new HttpClient();

        //        var uri = new Uri(url);

        //        var response = await _client.GetAsync(uri);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            AppSettings.AddOrUpdateValue("prayertimes_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString(), content);
        //        }

        //        jsonIsParsed = false;

        //    }

        //    if (!jsonIsParsed)
        //    { 

        //        string jsonToParse = AppSettings.GetValueOrDefault("prayertimes_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString(), "");

        //        rootObject = JsonConvert.DeserializeObject<RootObject>(jsonToParse);

        //        jsonIsParsed = true;

        //        todayPrayerTimes = GetPrayerTimesList();

        //    }

        //    List<double> prayerTimeDifferences = new List<double>();

        //    foreach(DateTime prayerTime in todayPrayerTimes)
        //    {

        //        bool shouldBeAdded = (prayerTime.Subtract(DateTime.Now).TotalMinutes >= 0);

        //        if (shouldBeAdded)
        //            prayerTimeDifferences.Add(prayerTime.Subtract(DateTime.Now).TotalMinutes);

        //    }

        //    double minutesTillNextPrayer = prayerTimeDifferences[0];

        //    if (Math.Floor(minutesTillNextPrayer) == 0)
        //    {

        //        //It is time for prayer! play azan!
        //        string azanPath = GeneralManager.azanPaths[0];
        //        DependencyService.Get<IAudioService>().PlayAudioFile(azanPath);

        //        await Task.Delay(1800000);

        //    }


        //    //List<string> fajrTimes = new List<string>();

        //    //for (int i = 0; i < timesObject.data.Count; i++)
        //    //    fajrTimes.Add(timesObject.data[i].timings.Fajr);

        //    //Console.WriteLine("Prayer time fajr is: " + fajrTimes);

        //    return true;
        //}

    }


    //--------------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------------------------------------------


    public class LocatorBackgroundService : IPeriodicTask
    {
        public LocatorBackgroundService()
        {
            Interval = TimeSpan.FromMinutes(1);
        }

        public TimeSpan Interval { get; set; }

        public async Task<bool> StartJob()
        {
            //Check for location first
            //If it's saved locally, you're ready to move on.

            //Check if it's time for an Azan yet.

            await UpdateLocation();

            return true;
        }

    }

}
