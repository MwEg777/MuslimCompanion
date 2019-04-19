using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using MuslimCompanion.Model;
using System.Threading;
using MuslimCompanion.Controls;
using MuslimCompanion.Core;

namespace MuslimCompanion
{
    public partial class MainPage : ContentPage
    {

        public int counter = 1;

        List<Quran> quran;

        public MainPage(int selectedSura = 1)
        {

            InitializeComponent();

            quran = GeneralManager.conn.Table<Quran>().ToList();

            LoadSura(selectedSura);

            //TestDB();

        }

        SelectableLabel sl;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
            
        }

        void SetupLayout()
        {



        }

        int tempSurahNumber = 1;
        int tempAyahNumber = 1;

        public void TestDB()
        {

            SetupLayout();

            tempAyahNumber = 1;

            sl.Text = "";

            while (quran[counter].SuraID == tempSurahNumber)
            {

                tempSurahNumber = quran[counter].SuraID;

                tempAyahNumber = quran[counter].VerseID;

                string textToAdd = quran[counter++].AyahText + " \uFD3F" + tempAyahNumber.ToString() + "\uFD3E ";

                sl.Text += ConvertNumerals(textToAdd);

            }

            tempSurahNumber++;

            sl.FontFamily = Device.RuntimePlatform == Device.Android ? "me_quran.ttf#me_quran" : "me_quran";

        }

        public void LoadSura(int suraNumber)
        {

            tempAyahNumber = 1;

            tempSurahNumber = suraNumber;

            SetupLayout();

            sl = new SelectableLabel();
            grid.Children.Add(sl);
            Grid.SetRow(sl, 3);
            Grid.SetColumn(sl, 1);
            Grid.SetColumnSpan(sl, 5);

            sl.Text = "";

            List<Quran> sura = new List<Quran>();

            //Still needs optimization. Maybe increment i, and stop checking after finding first Ayah that is in another sura? We'll see.

            bool firstAyah = true;

            foreach (Quran qr in quran)
            {

                if (qr.SuraID == tempSurahNumber)
                {

                    if (!firstAyah)
                    {

                        sura.Add(qr);

                    }

                    else
                    {

                        if (qr.AyahText.Contains("بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ"))
                        {

                            Quran qrr = qr;

                            qrr.AyahText = qr.AyahText.Replace("بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ", "");

                            sura.Add(qrr);

                        }

                        

                    }

                    firstAyah = false;

                }

            }

            int i = 0;

            foreach(Quran qr in sura)
            {

                string textToAdd = sura[i++].AyahText + " \uFD3F" + tempAyahNumber++.ToString() + "\uFD3E ";

                sl.Text += ConvertNumerals(textToAdd);

            }

            sl.FontFamily = Device.RuntimePlatform == Device.Android ? "me_quran.ttf#me_quran" : "me_quran";

        }

        private void Button1_Clicked(object sender, EventArgs e)
        {

            TestDB();

        }

        public string ConvertNumerals( string input)
        {

                return input.Replace('0', '\u0660')
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
