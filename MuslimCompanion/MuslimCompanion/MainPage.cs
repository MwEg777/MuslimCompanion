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
using Plugin.LocalNotifications;
using FormsToolkit;
using SlideOverKit;

namespace MuslimCompanion
{
    public partial class MainPage : MenuContainerPage, INotifyPropertyChanged
    {

        bool playingAyah;

        public int counter = 1;

        ToolbarItem ZoomInItem, ZoomOutItem;

        List<Quran> quran;

        private string favState;

        public string FavState { get { return favState; } set { favState = value; OnPropertyChanged(nameof(FavState)); } }

        private string nightModeState;

        public string NightModeState { get { return nightModeState; } set { nightModeState = value; OnPropertyChanged(nameof(NightModeState)); } }

        private int selectedAyahNumber = -1;

        public List<int> AyahEndsIndices;

        Image basmalahImage, quranFrame;

        ToolbarItem TafseerToolbarItem, FavoriteAyahToolbarItem, PlayAyahToolbarItem, TranslateAyahToolbarItem;

        int loadedSura = 0;

        Grid grid;

        public MainPage(int selectedSura = 1, int mode = 0, AyahSearchResult asr = null) // 0 = Normal sura mode, 1 = Search Ayah mode
        {

            /*<ToolbarItem x:Name="DownloadSuraItem" Order="Secondary" Icon="Microsoft.png" Text="تحميل السورة" Priority="0" Clicked="DownloadSuraToolbarItem" />
        <ToolbarItem x:Name="PlaySuraItem" Order="Secondary" Icon="Xamarin.png" Text="تشغيل السورة" Priority="1" Clicked="PlaySuraToolbarItem"/>
        <ToolbarItem x:Name="AddToFavItem" Order="Secondary" Icon="Xamarin.png" Text="{Binding FavState}" Priority="1" Clicked="ToggleFav"/>
        <ToolbarItem x:Name="NightModeItem" Order="Secondary" Icon="Xamarin.png" Text="{Binding NightModeState}" Priority="1" Clicked="ToggleNightMode"/>
        <ToolbarItem x:Name="ZoomInItem" Order="Primary" Icon="baseline_zoom_in_white_24.png" Text="+" Priority="1" Clicked="ZoomInToolbarItem"/>
        <ToolbarItem x:Name="ZoomOutItem" Order="Primary" Icon="baseline_zoom_out_white_24.png" Text="-" Priority="1" Clicked="ZoomOutToolbarItem"/>*/

            this.SlideMenu = new SlideUpMenuView();

            ToolbarItem DownloadSuraItem = new ToolbarItem("DownloadSuraItem", "Microsoft.png", new Action(() => DownloadSuraToolbarItem(new object(), new EventArgs())));

            DownloadSuraItem.Order = ToolbarItemOrder.Secondary;

            DownloadSuraItem.Text = "تحميل السورة";

            ToolbarItems.Add(DownloadSuraItem);

            ToolbarItem PlaySuraItem = new ToolbarItem("PlaySuraItem", "Microsoft.png", new Action(() => PlaySuraToolbarItem(new object(), new EventArgs())));

            PlaySuraItem.Order = ToolbarItemOrder.Secondary;

            PlaySuraItem.Text = "تشغيل السورة";

            ToolbarItems.Add(PlaySuraItem);

            ToolbarItem AddToFavItem = new ToolbarItem("AddToFavItem", "Microsoft.png", new Action(() => ToggleFav(new object(), new EventArgs())));

            AddToFavItem.Order = ToolbarItemOrder.Secondary;

            AddToFavItem.SetBinding(ToolbarItem.TextProperty, "FavState");

            ToolbarItems.Add(AddToFavItem);

            ToolbarItem NightModeItem = new ToolbarItem("NightModeItem", "Microsoft.png", new Action(() => ToggleNightMode(new object(), new EventArgs())));

            NightModeItem.Order = ToolbarItemOrder.Secondary;

            NightModeItem.SetBinding(ToolbarItem.TextProperty, "NightModeState");

            ToolbarItems.Add(NightModeItem);

            ZoomInItem = new ToolbarItem("ZoomInItem", "baseline_zoom_in_white_24.png", new Action(() => ZoomInToolbarItem(new object(), new EventArgs())));

            ZoomInItem.Text = "تكبير";

            ToolbarItems.Add(ZoomInItem);

            ZoomOutItem = new ToolbarItem("ZoomOutItem", "baseline_zoom_out_white_24.png", new Action(() => ZoomOutToolbarItem(new object(), new EventArgs())));

            ZoomOutItem.Text = "تحميل السورة";

            ToolbarItems.Add(ZoomOutItem);

            BindingContext = this;

            grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(7, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(6, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(70, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(7, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5, GridUnitType.Star) });

            basmalahImage = new Image();

            basmalahImage.Source = "basmalah.png";

            basmalahImage.Aspect = Aspect.AspectFit;

            quranFrame = new Image();

            quranFrame.Source = "QuranFrame.png";

            quranFrame.Aspect = Aspect.Fill;

            grid.Children.Add(quranFrame);

            grid.Children.Add(basmalahImage);

            sl = new SelectableLabel();

            grid.Children.Add(sl);

            Grid.SetRow(quranFrame, 1);

            Grid.SetRow(basmalahImage, 2);

            Grid.SetColumn(basmalahImage, 1);

            Grid.SetRowSpan(quranFrame, 4);

            Grid.SetColumnSpan(quranFrame, 7);

            Grid.SetColumnSpan(basmalahImage, 5);

            this.Content = grid;

            TafseerToolbarItem = new ToolbarItem("تفسير", "baseline_library_books_white_24.png", () =>
            {

                this.ShowMenu();

            });

            FavoriteAyahToolbarItem = new ToolbarItem("أضف إلى الآيات المفضلة", "baseline_favorite_white_24.png", () =>
            {



            });

            PlayAyahToolbarItem = new ToolbarItem("تشغيل الآية", "baseline_play_arrow_white_24.png", () =>
            {

                if (!playingAyah)
                {
                    PlayAyah(selectedAyahNumber);
                    playingAyah = true;
                }
                else
                {
                    playingAyah = false;
                    DependencyService.Get<IAudioService>().PauseAudioFile();
                    PlayAyahToolbarItem.Icon = "baseline_play_arrow_white_24.png";
                }

            });

            TranslateAyahToolbarItem = new ToolbarItem("ترجمة الآية", "baseline_translate_white_24.png", () =>
            {

                this.ShowMenu();

            });

            AyahEndsIndices = new List<int>();

            quran = conn.Table<Quran>().ToList();

            if (mode == 1 && asr != null)
                LoadSura(asr.SuraID, mode, asr);
            else
                LoadSura(selectedSura, mode, asr);

            loadedSura = selectedSura;

            if (AppSettings.Contains("favsura" + loadedSura.ToString())) FavState = (string)AppSettings.GetValueOrDefault("favsura" + selectedSura.ToString(), "إضافة إلى المفضلة");
            else FavState = "إضافة إلى المفضلة";

            NightModeState = "وضع القراءة الليلي";

            ToggleNightMode(new object(), new EventArgs());
            ToggleNightMode(new object(), new EventArgs());

            //TestDB();

            MessagingService.Current.Subscribe("AyahDeselected", (arg1) => {

                if (ToolbarItems.Contains(TafseerToolbarItem)) ToolbarItems.Remove(TafseerToolbarItem);
                if (ToolbarItems.Contains(FavoriteAyahToolbarItem)) ToolbarItems.Remove(FavoriteAyahToolbarItem);
                if (ToolbarItems.Contains(PlayAyahToolbarItem)) ToolbarItems.Remove(PlayAyahToolbarItem);
                if (ToolbarItems.Contains(TranslateAyahToolbarItem)) ToolbarItems.Remove(TranslateAyahToolbarItem);
                ToolbarItems.Add(ZoomInItem);
                ToolbarItems.Add(ZoomOutItem);

            
            });

            MessagingService.Current.Subscribe("AyahPlaybackDone", (arg1) =>
            {

                playingAyah = false;
                PlayAyahToolbarItem.Icon = "baseline_play_arrow_white_24.png";

            });

            MessagingService.Current.Subscribe("AyahPlaybackStarted", (arg1) =>
            {

                playingAyah = true;
                PlayAyahToolbarItem.Icon = "baseline_pause_white_24.png";

            });

            MessagingService.Current.Subscribe<AyahSelected>("AyahSelected", (arg1, arg2) =>
            {
                int si = arg2.startIndex;
                int ei = arg2.endIndex;
                selectedAyahNumber = -1;

                foreach (int end in AyahEndsIndices)
                {

                    if (ei < end)
                    {

                        selectedAyahNumber = AyahEndsIndices.IndexOf(end) + 1;
                        break;

                    }

                }

                if (selectedAyahNumber == -1)
                {

                    return;

                }

                ToolbarItems.Remove(ZoomInItem);
                ToolbarItems.Remove(ZoomOutItem);
                if (!ToolbarItems.Contains(TafseerToolbarItem)) ToolbarItems.Add(TafseerToolbarItem);
                if (!ToolbarItems.Contains(FavoriteAyahToolbarItem)) ToolbarItems.Add(FavoriteAyahToolbarItem);
                if (!ToolbarItems.Contains(PlayAyahToolbarItem)) ToolbarItems.Add(PlayAyahToolbarItem);
                if (!ToolbarItems.Contains(TranslateAyahToolbarItem)) ToolbarItems.Add(TranslateAyahToolbarItem);

            });


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
                if (AppSettings.GetValueOrDefault("favsura" + loadedSura.ToString(), "إضافة إلى المفضلة") == "إضافة إلى المفضلة")
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

        public void ToggleNightMode(object sender, EventArgs e)
        {

            if (AppSettings.Contains("nightmode"))
            {
                if (AppSettings.GetValueOrDefault("nightmode", "وضع القراءة الليلي") == "وضع القراءة الليلي")
                {

                    NightModeState = "وضع القراءة العادي";
                    AppSettings.AddOrUpdateValue("nightmode", NightModeState);
                    sl.TextColor = Color.White;
                    ((NavigationPage)Application.Current.MainPage).BackgroundColor = Color.Black;
                    ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.DarkSlateGray;
                    basmalahImage.Source = "basmalahwhite.png";

                }
                else
                {

                    NightModeState = "وضع القراءة الليلي";
                    AppSettings.AddOrUpdateValue("nightmode", NightModeState);
                    sl.TextColor = Color.Black;
                    ((NavigationPage)Application.Current.MainPage).BackgroundColor = Color.White;
                    ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.DodgerBlue;
                    basmalahImage.Source = "basmalah.png";

                }
            }

            else
            {

                NightModeState = "وضع القراءة العادي";
                AppSettings.AddOrUpdateValue("nightmode", NightModeState);
                sl.TextColor = Color.White;
                ((NavigationPage)Application.Current.MainPage).BackgroundColor = Color.Black;
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.DarkSlateGray;
                basmalahImage.Source = "basmalahwhite.png";

            }

        }

        int tempSurahNumber = 1;
        int tempAyahNumber = 1;

        List<Quran> sura;

        public void OnTextSelected()
        {

            DisplayAlert("Got event", "Event triggered successfully!", "OK");

        }

        public void LoadSura(int suraNumber, int mode = 0, AyahSearchResult asr = null)
        {

            tempAyahNumber = 1;

            tempSurahNumber = suraNumber;

            AyahEndsIndices = new List<int>();

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

            foreach (Quran qr in sura)
            {

                string textToAdd = sura[i++].AyahText + " \uFD3F" + tempAyahNumber++.ToString() + "\uFD3E ";

                sl.Text += ConvertNumerals(textToAdd);

                AyahEndsIndices.Add(sl.Text.Length);

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
            ((NavigationPage)Application.Current.MainPage).BackgroundColor = Color.White;
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.DodgerBlue;
        }

        public async void PlayAyah(int ayahID)
        {

            if (!Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), loadedSura.ToString())))
            {

                await DisplayAlert("لم يتم تحميل السورة", "يرجى تحميل السورة أولا", "موافق");
                return;

            }

            string basePath = Path.Combine(GlobalVar.Get<string>("quranaudio"), loadedSura.ToString());

            int ayahCount = int.Parse(suraAyahCounts[loadedSura - 1]);

            string fullPathOfFile = basePath;

            if (!canPlaySura)
                return;

            if (ayahID >= 100)
            {

                if (loadedSura >= 100)
                    fullPathOfFile = Path.Combine(basePath, loadedSura.ToString() + (ayahID + 1).ToString() + ".mp3");

                else if (loadedSura >= 10)
                    if (loadedSura >= 17)
                        fullPathOfFile = Path.Combine(basePath, "0" + loadedSura.ToString() + (ayahID + 1).ToString() + ".mp3");

                    else
                        fullPathOfFile = Path.Combine(basePath, "0" + loadedSura.ToString() + ayahID.ToString() + ".mp3");

                else if (loadedSura >= 1)
                    fullPathOfFile = Path.Combine(basePath, "00" + loadedSura.ToString() + ayahID.ToString() + ".mp3");



            }

            else if (ayahID >= 10)
            {

                if (loadedSura >= 100)
                    fullPathOfFile = Path.Combine(basePath, loadedSura.ToString() + ayahID.ToString() + ".mp3");
                else if (loadedSura >= 10)
                    if (loadedSura >= 17)
                        fullPathOfFile = Path.Combine(basePath, "0" + loadedSura.ToString() + "0" + (ayahID + 1).ToString() + ".mp3");
                    else
                        fullPathOfFile = Path.Combine(basePath, "0" + loadedSura.ToString() + "0" + ayahID.ToString() + ".mp3");
                else if (loadedSura >= 1)
                    fullPathOfFile = Path.Combine(basePath, "00" + loadedSura.ToString() + "0" + ayahID.ToString() + ".mp3");

            }

            else if (ayahID >= 0)
            {

                if (loadedSura >= 100)
                    fullPathOfFile = Path.Combine(basePath, loadedSura.ToString() + "00" + (ayahID + 1).ToString() + ".mp3");
                else if (loadedSura >= 10)
                    if (loadedSura >= 17)
                        fullPathOfFile = Path.Combine(basePath, "0" + loadedSura.ToString() + "00" + (ayahID + 1).ToString() + ".mp3");
                    else
                        fullPathOfFile = Path.Combine(basePath, "0" + loadedSura.ToString() + "00" + ayahID.ToString() + ".mp3");
                else if (loadedSura >= 1)
                {
                    fullPathOfFile = Path.Combine(basePath, "00" + loadedSura.ToString() + "00" + ayahID.ToString() + ".mp3");
                    if (loadedSura == 1 && ayahID == 7)
                        fullPathOfFile = Path.Combine(basePath, "00" + loadedSura.ToString() + "007" + ".mp3");
                }

            }

            DependencyService.Get<IAudioService>().PlayAudioFile(fullPathOfFile);

            int AyahIndex = 0, AyahEndIndex = 0;

            if (loadedSura == 1)
            {

                if (ayahID != 0 && ayahID != 7)
                {

                    AyahIndex = sl.Text.IndexOf(sura[ayahID - 1].AyahText);

                    AyahEndIndex = sl.Text.IndexOf(sura[ayahID].AyahText);

                }

                else if (ayahID == 7)
                {

                    AyahIndex = sl.Text.IndexOf(sura[ayahID - 1].AyahText);


                    AyahEndIndex = sl.Text.Length;


                }

            }

            else
            {

                AyahIndex = sl.Text.IndexOf(sura[ayahID].AyahText);

                try
                {
                    AyahEndIndex = sl.Text.IndexOf(sura[ayahID + 1].AyahText);
                }
                catch
                {
                    AyahEndIndex = sl.Text.Length;
                }

            }

            DependencyService.Get<ISelectableLabel>().SelectPartOfText(AyahIndex, AyahEndIndex);

            await Task.Delay(50);

            ToolbarItems.Remove(ZoomInItem);
            ToolbarItems.Remove(ZoomOutItem);
            if (!ToolbarItems.Contains(TafseerToolbarItem)) ToolbarItems.Add(TafseerToolbarItem);
            if (!ToolbarItems.Contains(FavoriteAyahToolbarItem)) ToolbarItems.Add(FavoriteAyahToolbarItem);
            if (!ToolbarItems.Contains(PlayAyahToolbarItem)) ToolbarItems.Add(PlayAyahToolbarItem);
            if (!ToolbarItems.Contains(TranslateAyahToolbarItem)) ToolbarItems.Add(TranslateAyahToolbarItem);

            await Task.Delay(DependencyService.Get<IAudioService>().RetrieveLength(fullPathOfFile) - 50);

        }

        public async void PlaySurah(int suraID)
        {

            if (!Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), suraID.ToString())))
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

        public string ConvertNumerals(string input)
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

        private void Tafseer_Clicked(object sender, EventArgs e)
        {



        }
    }
}
