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
using System.IO;
using static MuslimCompanion.Core.GeneralManager;

namespace MuslimCompanion
{
    public partial class MainPage : ContentPage
    {

        public int counter = 1;

        List<Quran> quran;

        public MainPage(int selectedSura = 1, int mode = 0, AyahSearchResult asr = null) // 0 = Normal sura mode, 1 = Search Ayah mode
        {

            InitializeComponent();

            quran = conn.Table<Quran>().ToList();

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
            //NavigationPage.SetHasNavigationBar(this, false);
            
        }

        void SetupLayout()
        {



        }

        int tempSurahNumber = 1;
        int tempAyahNumber = 1;

        List<Quran> sura;

        public async void LoadSura(int suraNumber, int mode = 0, AyahSearchResult asr = null)
        {

            tempAyahNumber = 1;

            tempSurahNumber = suraNumber;

            SetupLayout();

            GlobalVar.Set("selectabletoset", "suralabel");
            sl = new SelectableLabel();
            grid.Children.Add(sl);
            Grid.SetRow(sl, 3);
            Grid.SetColumn(sl, 1);
            Grid.SetColumnSpan(sl, 5);

            sl.Text = "";

            sura = new List<Quran>();

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

            if (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), suraNumber.ToString())))
            {
                PlaySurah(suraNumber);
            }


            if (mode == 1 && asr != null)
            {

                int AyahIndex = 0, AyahEndIndex = 0;

                AyahIndex = sl.Text.IndexOf(sura[asr.AyahID - 1].AyahText);

                AyahEndIndex = sl.Text.IndexOf(sura[asr.AyahID].AyahText);

                DependencyService.Get<ISelectableLabel>().SelectPartOfText(AyahIndex, AyahEndIndex);

            }



               



            

        }

        public async void PlaySurah(int suraID)
        {

            string basePath = Path.Combine(GlobalVar.Get<string>("quranaudio"), suraID.ToString());

            int ayahCount = int.Parse(suraAyahCounts[suraID - 1]);

            string fullPathOfFile = basePath;

            for (int i = 0; i < ayahCount; i++)
            {

                if (i >= 100)
                {

                    if (suraID >= 100)
                        fullPathOfFile = Path.Combine(basePath, suraID.ToString() + (i + 1).ToString() + ".mp3");

                    else if (suraID >= 10)
                        if (suraID >= 17)
                            fullPathOfFile = Path.Combine(basePath, "0" + suraID.ToString() + (i + 1).ToString() + ".mp3");

                        else
                            fullPathOfFile = Path.Combine(basePath, "0" + suraID.ToString() + i.ToString() + ".mp3");

                    else if (suraID >= 1)
                        fullPathOfFile = Path.Combine(basePath, "00" + suraID.ToString() + i.ToString() + ".mp3");



                }

                else if (i >= 10)
                {

                    if (suraID >= 100)
                        fullPathOfFile = Path.Combine(basePath, "0" + suraID.ToString() + i.ToString() + ".mp3");
                    else if (suraID >= 10)
                        if (suraID >= 17)
                            fullPathOfFile = Path.Combine(basePath, "0" + suraID.ToString() + "0" + (i + 1).ToString() + ".mp3");
                        else
                            fullPathOfFile = Path.Combine(basePath, "0" + suraID.ToString() + "0" + i.ToString() + ".mp3");
                    else if (suraID >= 1)
                        fullPathOfFile = Path.Combine(basePath, "00" + suraID.ToString() + "0" + i.ToString() + ".mp3");

                }

                else if (i >= 0)
                {

                    if (suraID >= 100)
                        fullPathOfFile = Path.Combine(basePath, "00" + suraID.ToString() + (i + 1).ToString() + ".mp3");
                    else if (suraID >= 10)
                        if (suraID >= 17)
                            fullPathOfFile = Path.Combine(basePath, "0" + suraID.ToString() + "00" + (i + 1).ToString() + ".mp3");
                        else
                            fullPathOfFile = Path.Combine(basePath, "0" + suraID.ToString() + "00" + i.ToString() + ".mp3");
                    else if (suraID >= 1)
                    {
                        fullPathOfFile = Path.Combine(basePath, "00" + suraID.ToString() + "00" + i.ToString() + ".mp3");
                        if (suraID == 1 && i == 7)
                            fullPathOfFile = Path.Combine(basePath, "00" + suraID.ToString() + "007" + ".mp3");
                    }

                }

                DependencyService.Get<IAudioService>().PlayAudioFile(fullPathOfFile);

                int AyahIndex = 0, AyahEndIndex = 0;

                if (suraID == 1)
                {

                    if (i != 0 && i != 7)
                    {
                        
                        AyahIndex = sl.Text.IndexOf(sura[i - 1].AyahText);

                        AyahEndIndex = sl.Text.IndexOf(sura[i].AyahText);

                    }

                }

                else
                {

                    AyahIndex = sl.Text.IndexOf(sura[i].AyahText);

                    AyahEndIndex = sl.Text.IndexOf(sura[i + 1].AyahText);

                }

                DependencyService.Get<ISelectableLabel>().SelectPartOfText(AyahIndex, AyahEndIndex);

                await Task.Delay(DependencyService.Get<IAudioService>().RetrieveLength(fullPathOfFile));


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
