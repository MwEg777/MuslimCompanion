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
using System.ComponentModel;

namespace MuslimCompanion
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {

        public int counter = 1;

        List<Quran> quran;

        private string favState;

        public string FavState { get { return favState; } set { favState = value; OnPropertyChanged(nameof(FavState)); } } 

        int loadedSura = 0;

        public MainPage(int selectedSura = 1, int mode = 0, AyahSearchResult asr = null) // 0 = Normal sura mode, 1 = Search Ayah mode
        {

            InitializeComponent();

            BindingContext = this;

            quran = conn.Table<Quran>().ToList();

            if (mode == 1 && asr != null)
                LoadSura(asr.SuraID, mode, asr);
            else
                LoadSura(selectedSura, mode, asr);

            loadedSura = selectedSura;

            if (AppSettings.Contains("favsura" + loadedSura.ToString())) FavState = (string)AppSettings.GetValueOrDefault("favsura" + selectedSura.ToString(), "إضافة إلى المفضلة");
            else FavState = "إضافة إلى المفضلة";

            //TestDB();

        }

        

        SelectableLabel sl;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //NavigationPage.SetHasNavigationBar(this, false);
            canPlaySura = true;
            
        }

        public void ToggleFav(object sender, EventArgs e)
        {

            if (AppSettings.Contains("favsura" + loadedSura.ToString()))
            {
                if ((string)App.Current.Properties["favsura" + loadedSura.ToString()] == "إضافة إلى المفضلة")
                {

                    FavState = "حذف من المفضلة";
                    AppSettings.AddOrUpdateValue("favsura" + loadedSura.ToString(), FavState);

                }
                else
                {

                    FavState = "إضافة إلى المفضلة";
                    AppSettings.AddOrUpdateValue("favsura" + loadedSura.ToString(), FavState);

                }
            }

            else
            {

                FavState = "حذف من المفضلة";
                AppSettings.AddOrUpdateValue("favsura" + loadedSura.ToString(), FavState);

            }

        }

        int tempSurahNumber = 1;
        int tempAyahNumber = 1;

        List<Quran> sura;

        public async void LoadSura(int suraNumber, int mode = 0, AyahSearchResult asr = null)
        {

            tempAyahNumber = 1;

            tempSurahNumber = suraNumber;

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


            if (mode == 1 && asr != null)
            {

                int AyahIndex = 0, AyahEndIndex = 0;

                AyahIndex = sl.Text.IndexOf(sura[asr.AyahID - 1].AyahText);

                AyahEndIndex = sl.Text.IndexOf(sura[asr.AyahID].AyahText);

                DependencyService.Get<ISelectableLabel>().SelectPartOfText(AyahIndex, AyahEndIndex);

            }

        }

        bool canPlaySura = true;

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            canPlaySura = false;
        }



        public async void PlaySurah(int suraID)
        {

            if(!Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), suraID.ToString())))
            {

                await DisplayAlert("لم يتم تحميل السورة", "يرجى تحميل السورة أولا", "موافق");
                return;

            }


            string basePath = Path.Combine(GlobalVar.Get<string>("quranaudio"), suraID.ToString());

            int ayahCount = int.Parse(suraAyahCounts[suraID - 1]);

            string fullPathOfFile = basePath;

            if (!canPlaySura)
                return;

            if (suraID != 1)
            {

                if (!Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), "bismillah")))
                {

                    DependencyService.Get<IAudioService>().PlayAudioFile(Path.Combine(GlobalVar.Get<string>("quranaudio"), "bismillah", "bismillah.mp3"));

                    await Task.Delay(DependencyService.Get<IAudioService>().RetrieveLength(Path.Combine(GlobalVar.Get<string>("quranaudio"), "bismillah", "bismillah.mp3")));

                }
            }
            for (int i = 0; i < ayahCount; i++)
            {

                if (!canPlaySura)
                    return;

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
                        fullPathOfFile = Path.Combine(basePath, suraID.ToString() + i.ToString() + ".mp3");
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
                        fullPathOfFile = Path.Combine(basePath, suraID.ToString() + "00" + (i + 1).ToString() + ".mp3");
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

                    else if (i == 7)
                    {

                        AyahIndex = sl.Text.IndexOf(sura[i - 1].AyahText);


                        AyahEndIndex = sl.Text.Length;


                    }

                }

                else
                {

                    AyahIndex = sl.Text.IndexOf(sura[i].AyahText);

                    try
                    { 
                        AyahEndIndex = sl.Text.IndexOf(sura[i + 1].AyahText);
                    }
                    catch
                    {
                        AyahEndIndex = sl.Text.Length;
                    }

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

        private void DownloadSuraToolbarItem(object sender, EventArgs e)
        {

            if (GlobalVar.Get<float>("downloadprogress", -1) == -1)
                DownloadSura(loadedSura);

        }

        private void PlaySuraToolbarItem(object sender, EventArgs e)
        {

            PlaySurah(loadedSura);

        }

        private void ZoomInToolbarItem(object sender, EventArgs e)
        {

            sl.FontSize++;

        }

        private void ZoomOutToolbarItem(object sender, EventArgs e)
        {

            sl.FontSize--;

        }
    }
}
