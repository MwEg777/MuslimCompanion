using Matcha.BackgroundService;
using MuslimCompanion.Controls;
using MuslimCompanion.Model;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.LocalNotifications;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using MuslimCompanion.Services;
using System.Net.Http;
using static MuslimCompanion.Model.PrayerTimeResponseModel;
using Newtonsoft.Json;
using FormsToolkit;

namespace MuslimCompanion.Core
{
    public static class GeneralManager
    {
        public static ObservableCollection<SuraItem> Items, CopyItems;

        public static ISettings AppSettings =>
    CrossSettings.Current;

        public static List<string> suraAyahCounts = new List<string>(new string[] { "8", "286", "200", "176", "120", "165", "206", "75", "129", "109",
            "123", "111", "43", "52", "99", "128" ,"111" ,"110", "98", "135",
            "112", "78", "118", "64", "77", "227" ,"93" ,"88" ,"69" ,"60",
            "34", "30", "73", "54", "45", "83" ,"182" ,"88", "75", "85",
            "54", "53", "89", "59", "37", "35" ,"38" ,"29" ,"18" ,"45",
            "60", "49", "62", "55", "78" ,"96" ,"29", "22", "24",
            "13", "14", "11", "11", "18", "12" ,"12" ,"30" ,"52" ,"52",
            "44", "28", "28", "20", "56", "40" ,"31" ,"50", "40", "46",
            "42", "29", "19", "36", "25", "22" ,"17" ,"19" ,"26" ,"30",
            "20", "15", "21", "11", "8", "8" ,"19" ,"5", "8", "8",
            "11", "11", "8", "3", "9", "5" ,"4" ,"7" ,"3" ,"6",
            "3", "5" , "4" , "5", "6"});

        public interface IAudioService
        {

            void PlayAudioFile(string filePath);
            int RetrieveLength(string filePath);
            void PauseAudioFile();
            void ResumeAudioFile();
            void StopAudioFile();

        }

        public interface ISelectableLabel
        {

            void SelectPartOfText(int startIndex, int endIndex);

        }

        public static IDownloader downloader;

        public static SQLiteConnection conn;

        public static List<string> azanPaths;

        public static List<Prayers> prayertimes;

        public static List<cities> cities;

        public static async void InitConnection()
        {

            if (!Application.Current.Properties.ContainsKey("azannotification"))
                Application.Current.Properties.Add("azannotification", AppSettings.GetValueOrDefault("azannotification", false));

            if (!Application.Current.Properties.ContainsKey("cityname"))
                Application.Current.Properties.Add("cityname", AppSettings.GetValueOrDefault("cityname", "موقعك"));

            while (App.DatabaseLocation == null)
            {

                await Task.Delay(25);

            }

            conn = new SQLiteConnection(App.DatabaseLocation);

            downloader = DependencyService.Get<IDownloader>();
            downloader.OnFileDownloaded += OnFileDownloaded;

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

                    if (results.ContainsKey(Permission.Storage))
                        status = results[Permission.Storage];
                }

            }
            catch
            {

                //await DisplayAlert("Exception happened.", ex.Message.ToString(), "OK");
            }

            ProcessPrayerTimes();

            cities = conn.Table<cities>().ToList();

        }

        public static async void ProcessPrayerTimes(int mode = 0) //Handles prayer times from A to Z. From fetching from server, Till updating database and scheduling.
        {

            //Reset prayertimes to null. Useful for showing a Loading text in Azan page.
            prayertimes = null;

            //Before proceeding, Wait for user location, and Fetch from server first!

            //Waiting for user location :

            while (muslimPosition == null)
                await Task.Delay(100);

            //Prepare a URL for API request
            string url = GeneralManager.GeneratePrayerAPIRequest();

            HttpClient _client = new HttpClient();

            var uri = new Uri(url);

            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(content);
                GlobalVar.Set("prayersresponse", rootObject);
                //List<Tuple<DateTime, string>> monthPrayerTimes = abs.GetNextPrayersList(50);
                Console.WriteLine(rootObject.data.Count.ToString());

            }

            else
                return;

            //First, Fetch next X prayers into a list

            int X = 50;

            List<Tuple<DateTime, string>> prayerTimes = AzanBackgroundService.GetNextPrayersList(X);

            //Second, refresh and validate database. Remove old entries. Remove fired entries. 

            ValidateDatabasePrayerTimes(mode);

            //Third, check how many missing entries there are in the database.

            prayertimes = conn.Table<Prayers>().ToList();

            int missingElementsCount = X - prayertimes.Count;

            //Forth, Make a for loop for all missing entries, and fill them with the next entries.

            for (int i = 0; i < missingElementsCount; i++)
            {

                if (X - missingElementsCount + i >= prayerTimes.Count)
                    break;

                conn.Execute("INSERT INTO prayers (prayertype, firetime, scheduledtofire, fired) VALUES (?, ?, 0, 0)", prayerTimes[X - missingElementsCount + i].Item2, prayerTimes[X - missingElementsCount + i].Item1);
                conn.Commit();

            }

            //Fifth, Validate database again.

            ValidateDatabasePrayerTimes();

            //Sixth, Schedule unscheduled alarms.

            prayertimes = conn.Table<Prayers>().ToList();

            for (int i=0; i < prayertimes.Count; i++)
            {

                if (prayertimes[i].ScheduledToFire == 0)
                {

                    //Schedule alarm for this prayer time.

                    Dictionary<string, int> prayerTypeToInt = new Dictionary<string, int>() { ["Fajr"] = 1, ["Dhuhr"] = 2, ["Asr"] = 3, ["Maghrib"] = 4, ["Isha"] = 5 };

                    MessagingService.Current.SendMessage("ScheduleAzan", new Tuple<DateTime, int>(prayertimes[i].FireTime, prayerTypeToInt[prayertimes[i].PrayerType]));

                    //Mark as scheduled in database
                    conn.Execute("UPDATE prayers SET scheduledtofire = 1 WHERE firetime = ?", prayertimes[i].FireTime);
                    conn.Commit();

                }

            }


        }

        public static void ValidateDatabasePrayerTimes(int mode = 0) //Remove fired or passed elements from database.
        {

            List<Prayers> prayertimes = conn.Table<Prayers>().ToList();

            foreach (Prayers pr in prayertimes)
            {

                if ((pr.Fired == 1 || PrayerTimePassed(pr.FireTime) ) || mode == 1)
                    conn.Execute("DELETE FROM prayers WHERE firetime = ?", pr.FireTime);

                conn.Commit();

            }

        }

        public static bool PrayerTimePassed(DateTime prayerTime)
        {

            if ((prayerTime - DateTime.Now).TotalMilliseconds >= 0)
                return false;
            else
                return true;

        } //A bool that checks if a certain prayer time passed or not

        public static void PrayerNotificationPressed()
        {

            /*List<Prayers> prayertimes = conn.Table<Prayers>().ToList();

            conn.Execute("DELETE FROM prayers WHERE id = (SELECT MIN(id) FROM prayers)");

            conn.Commit();*/

        } //Callback for prayer notification click

        public static void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                //File was downloaded and saved
                numberOfFilesDownloaded++;
                GlobalVar.Set("downloadprogress", ((float)numberOfFilesDownloaded / (float)numberOfFilesToDownload));
                CrossLocalNotifications.Current.Show("جاري تحميل السورة ", (GlobalVar.Get<float>("downloadprogress", 0) * 100).ToString() + "%");
                if (GlobalVar.Get<float>("downloadprogress", 0) >= 1) { 
                    CrossLocalNotifications.Current.Show("تم تحميل السورة بنجاح ", "يمكنك الآن تشغيل السورة");
                    GlobalVar.Set("downloadprogress", -1f);
                }
            }
            else
            {
                //File was downloaded but NOT saved
                //TODO : DOWNLOAD FAILED EVENT
            }
        }

        public static void DownloadFile(string url, string folderName)
        {
            downloader.DownloadFile(url, "MuslimCompanion/" + folderName);
        }

        static int numberOfFilesToDownload = 0, numberOfFilesDownloaded = 0;

        public static async void DownloadSura(int suraID)
        {

            GlobalVar.Set("downloadprogress", 0f);

            int ayahCount = int.Parse(suraAyahCounts[suraID - 1]);

            List<string> fileURLs = new List<string>();

            numberOfFilesToDownload = ayahCount - 1;

            numberOfFilesDownloaded = 0;

            for (int i = 0; i < ayahCount; i++)
            {

                if (i >= 100)
                {

                    if (suraID >= 100)
                        fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/" + suraID.ToString() + (i + 1).ToString() + ".mp3");
                    else if (suraID >= 10)
                        if (suraID >= 17)
                            fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/0" + suraID.ToString() + (i + 1).ToString() + ".mp3");
                        else
                            fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/0" + suraID.ToString() + i.ToString() + ".mp3");
                    else if (suraID >= 1)
                        fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/00" + suraID.ToString() + i.ToString() + ".mp3");

                }

                else if (i >= 10)
                {

                    if (suraID >= 100)
                        fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/" + suraID.ToString() + "0" + (i + 1).ToString() + ".mp3");
                    else if (suraID >= 10)
                        if (suraID >= 17)
                            fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/0" + suraID.ToString() + "0" + (i + 1).ToString() + ".mp3");
                        else
                            fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/0" + suraID.ToString() + "0" + i.ToString() + ".mp3");
                    else if (suraID >= 1)
                        fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/00" + suraID.ToString() + "0" + i.ToString() + ".mp3");

                }

                else if (i >= 0)
                {

                    if (suraID >= 100)
                        fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/" + suraID.ToString() + "00" + (i + 1).ToString() + ".mp3");
                    else if (suraID >= 10)
                        if (suraID >= 17)
                            fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/0" + suraID.ToString() + "00" + (i + 1).ToString() + ".mp3");
                        else
                            fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/0" + suraID.ToString() + "00" + i.ToString() + ".mp3");
                    else if (suraID >= 1)
                    {

                        fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/00" + suraID.ToString() + "00" + i.ToString() + ".mp3");
                        if (suraID == 1 && i == 6)
                            fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/00" + suraID.ToString() + "007" + ".mp3");
                    }

                }

            }
            if (!Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), suraID.ToString())))
            {
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                    if (status != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                        {
                            //await DisplayAlert("Need storage permission", "Gunna need that permission", "OK");
                        }

                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                        //Best practice to always check that the key exists
                        if (results.ContainsKey(Permission.Storage))
                            status = results[Permission.Storage];
                    }
                    else if (status == PermissionStatus.Granted)
                    {
                        //await DisplayAlert("No need for permission", "Permission is already granted. Will start downloading sura!", "OK");
                        foreach (string link in fileURLs)
                        {

                            DownloadFile(link, suraID.ToString());

                        }

                    }

                    if (status == PermissionStatus.Granted)
                    {
                        //await DisplayAlert("Stogage Granted", "Got access successfully! Will download sura now.", "OK");
                        foreach (string link in fileURLs)
                        {

                            DownloadFile(link, suraID.ToString());


                        }

                    }
                    else if (status != PermissionStatus.Unknown)
                    {
                        //await DisplayAlert("Storage Denied", "Can not continue, try again.", "OK");
                    }
                }
                catch (Exception ex)
                {

                    //await DisplayAlert("Exception happened.", ex.Message.ToString(), "OK");
                }

            }

            if (!Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), "bismillah")))
                DownloadFile("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/bismillah.mp3", "bismillah");

        }

        #region Geolocator

        public static IGeolocator locator;

        public static Position muslimPosition; //Variable that holds coordinates of user

        public static async Task<bool> UpdateLocation()
        {

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        //await DisplayAlert("Need storage permission", "Gunna need that permission", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }
                else if (status == PermissionStatus.Granted)
                {
                    //await DisplayAlert("No need for permission", "Permission is already granted. Will start downloading sura!", "OK");
                    

                    //TODO : Update location.

                }

                if (status != PermissionStatus.Granted)
                {
                    //await DisplayAlert("Stogage Granted", "Got access successfully! Will download sura now.", "OK");
                    return false;
                }

            }
            catch (Exception ex)
            {
                //await DisplayAlert("Exception happened.", ex.Message.ToString(), "OK");
                return false;
            }

            locator = CrossGeolocator.Current;

            locator.DesiredAccuracy = 50;

            muslimPosition = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(30));

            return true;

        }

        #endregion

        public static string GeneratePrayerAPIRequest()
        {

            //Initialize string base

            string baseString = "https://api.aladhan.com/v1/calendar?";

            string finalString = baseString;

            //Add latitude

            //finalString += "latitude=" + muslimPosition.Latitude + "&";
            finalString += "latitude=" + (GlobalVar.Get<float>("latitude", 0) == 0? muslimPosition.Latitude : GlobalVar.Get<float>("latitude", 0)) + "&";

            //Add longitude

            //finalString += "longitude=" + muslimPosition.Longitude + "&";
            finalString += "longitude=" + (GlobalVar.Get<float>("longitude", 0) == 0 ? muslimPosition.Latitude : GlobalVar.Get<float>("longitude", 0)) + "&";

            //Add month

            finalString += "month=" + DateTime.Now.Month + "&";

            //Add year

            finalString += "year=" + DateTime.Now.Year + "&";

            return finalString;

        }
    }
}
