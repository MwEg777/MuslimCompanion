using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using MuslimCompanion.Model;
using System.Threading;

namespace MuslimCompanion
{
    public partial class MainPage : ContentPage
    {

        public int counter = 1;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        int tempSurahNumber = 0;
        int tempAyahNumber = 1;

        public void TestDB()
        {

            label1.Text = "";

            SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation);

            var quran = conn.Table<Quran>().ToList();

            for (int i=0; i<15; i++)
            {

                int surahNumber = quran[counter].SuraID;
                tempAyahNumber = quran[counter].VerseID;

                if (surahNumber != tempSurahNumber)
                {

                    tempAyahNumber = 1;
                    tempSurahNumber = surahNumber;

                }

                string textToAdd = quran[counter++].AyahText + " \uFD3F" + tempAyahNumber.ToString() + "\uFD3E ";

                label1.Text += ConvertNumerals(textToAdd);

            }

            label1.FontFamily = Device.RuntimePlatform == Device.Android ? "me_quran.ttf#me_quran" : "me_quran";

        }

        private void Button1_Clicked(object sender, EventArgs e)
        {

            TestDB();

        }

        public string ConvertNumerals( string input)
        {

                return input.Replace('0', '\u0661')
                        .Replace('1', '\u0661')
                        .Replace('2', '\u0662')
                        .Replace('3', '\u0663')
                        .Replace('4', '\u0664')
                        .Replace('5', '\u0665')
                        .Replace('6', '\u0666')
                        .Replace('7', '\u0667')
                        .Replace('8', '\u0668')
                        .Replace('9', '\u0669');

        }


    }
}
