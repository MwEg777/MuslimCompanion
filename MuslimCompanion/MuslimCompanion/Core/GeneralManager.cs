using MuslimCompanion.Controls;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MuslimCompanion.Core
{
    public static class GeneralManager
    {

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

        }

        public interface ISelectableLabel
        {

            void SelectPartOfText(int startIndex, int endIndex);

        }

        public static IDownloader downloader;

        public static SQLiteConnection conn;

        public static async void InitConnection()
        {

            while (App.DatabaseLocation == null)
            {

                await Task.Delay(25);

            }

            conn = new SQLiteConnection(App.DatabaseLocation);

            downloader = DependencyService.Get<IDownloader>();
            downloader.OnFileDownloaded += OnFileDownloaded;


        }

        public static void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                //File was downloaded and saved
            }
            else
            {
                //File was downloaded but NOT saved
            }
        }

        public static void DownloadFile(string url, string folderName)
        {
            downloader.DownloadFile(url, "MuslimCompanion/" + folderName);
        }

    }
}
