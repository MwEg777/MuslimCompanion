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
using Xamarin.Forms.PlatformConfiguration;

namespace MuslimCompanion
{
    public partial class MainPage : ContentPage
    {

        public int counter = 1;

        List<Quran> quran;

        public MainPage(int selectedSura = 1, int mode = 0, AyahSearchResult asr = null) // 0 = Normal sura mode, 1 = Search Ayah mode
        {

            InitializeComponent();

            quran = GeneralManager.conn.Table<Quran>().ToList();

            if (mode == 1 && asr != null)
                LoadSura(asr.SuraID, mode, asr);
            else
                LoadSura(selectedSura, mode, asr);

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

        public void MarkPartOfAyah(int AyahNumber, List<Quran> Sura)
        {

            GlobalVar.Set("SEARCHING_AYAH", true);

            int AyahIndex = 0;

            AyahIndex = sl.Text.IndexOf(Sura[AyahNumber].AyahText);

            GlobalVar.Set("START_INDEX", AyahIndex);

        }

        public void LoadSura(int suraNumber, int mode = 0, AyahSearchResult asr = null)
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

            if (mode == 1 && asr != null)
            {

                MarkPartOfAyah(asr.AyahID, sura);

            }

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
