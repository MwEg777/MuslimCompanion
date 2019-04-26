
using MuslimCompanion.Core;
using MuslimCompanion.Model;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static MuslimCompanion.Core.GeneralManager;

namespace MuslimCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseSura : ContentPage
    {
        public ObservableCollection<SuraItem> Items;
        List<suranames> sura;
        List<string> suraNames;

        public ChooseSura()
        {
            InitializeComponent();

            InitSuras();

        }

        async void InitSuras()
        {

            downloader.OnFileDownloaded += OnFileDownloaded;

            if (conn == null)
                InitConnection();

            while (conn == null)
            {

                await Task.Delay(25);

            }

            sura = conn.Table<suranames>().ToList();

            Items = new ObservableCollection<SuraItem>();

            suraNames = new List<string>();

            for (int i = 0; i < 114; i++)
            {

                Items.Add(new SuraItem { sname = sura[i].SuraName, sindex = (i+1).ToString(), scount = suraAyahCounts[i] });
                suraNames.Add(sura[i].SuraName);

            }

            var suraDataTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();

                var ayahCountLabel = new Label { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

                var ayahIndexLabel = new Label { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

                var suraNameLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold , FontSize = 20};

                var suraDownloadButton = new Button { VerticalOptions = LayoutOptions.Center, FontSize = 20 };

                suraNameLabel.SetBinding(Label.TextProperty, "sname");

                ayahCountLabel.SetBinding(Label.TextProperty, "scount");

                ayahIndexLabel.SetBinding(Label.TextProperty, "sindex");

                suraDownloadButton.SetValue(Button.TextProperty, "تحميل");

                //suraDownloadButton.SetValue(Button.CommandProperty, "تحميل");

                grid.Children.Add(ayahIndexLabel, 1, 0);

                grid.Children.Add(ayahCountLabel, 2, 0);

                grid.Children.Add(suraNameLabel, 3, 0);

                grid.Children.Add(suraDownloadButton);

                return new ViewCell { View = grid};

            });

            ListView lv = new ListView { ItemsSource = Items, ItemTemplate = suraDataTemplate, Margin = new Thickness(0, 20, 0, 0), };

            lv.ItemTapped += Handle_ItemTapped;

            Grid gr = new Grid();

            var ayahCountLabelName = new Label { HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 40 };

            var ayahIndexLabelName = new Label { HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 40 };

            var suraNameLabelName = new Label { HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 40 };

            var suraDownloadButtonName = new Label { HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 40 };

            suraNameLabelName.Text = "إسم السورة";

            ayahCountLabelName.Text = "عدد آيات السورة";

            ayahIndexLabelName.Text = "رقم السورة";

            suraDownloadButtonName.Text = "تحميل";

            gr.Children.Add(ayahIndexLabelName, 1, 0);

            gr.Children.Add(ayahCountLabelName, 2, 0);

            gr.Children.Add(suraNameLabelName, 3, 0);

            gr.Children.Add(suraDownloadButtonName);

            Content = new StackLayout
            {
                Margin = new Thickness(10),
                Children = {
                gr,
                lv
                }
            };
       
    }

        public async void DownloadSura(int suraID)
        {

            int ayahCount = int.Parse(suraAyahCounts[suraID - 1]);

            List<string> fileURLs = new List<string>();

            for (int i=0; i<ayahCount; i++)
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
                            fileURLs.Add("http://www.everyayah.com/data/Abdul_Basit_Murattal_64kbps/0" + suraID.ToString() + "00" + (i+1).ToString() + ".mp3");
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
                            await DisplayAlert("Need storage permission", "Gunna need that permission", "OK");
                        }

                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                        //Best practice to always check that the key exists
                        if (results.ContainsKey(Permission.Storage))
                            status = results[Permission.Storage];
                    }
                    else if (status == PermissionStatus.Granted)
                    {
                        await DisplayAlert("No need for permission", "Permission is already granted. Will start downloading sura!", "OK");
                        foreach (string link in fileURLs)
                        {

                            DownloadFile(link, suraID.ToString());

                        }

                    }

                    if (status == PermissionStatus.Granted)
                    {
                        await DisplayAlert("Stogage Granted", "Got access successfully! Will download sura now.", "OK");
                        foreach (string link in fileURLs)
                        {

                            DownloadFile(link, suraID.ToString());


                        }

                    }
                    else if (status != PermissionStatus.Unknown)
                    {
                        await DisplayAlert("Storage Denied", "Can not continue, try again.", "OK");
                    }
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Exception happened.", ex.Message.ToString(), "OK");

                }

            } 

        }

        public async void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                //await DisplayAlert("File download", "File was downloaded and saved successfully.", "OK");
                //File was downloaded and saved
            }
            else
            {
                //await DisplayAlert("File download", "File was downloaded but NOT saved.", "OK");
                //File was downloaded but NOT saved
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            string selectedSuraName = ((SuraItem)e.Item).sname;

            int selItemIndex = suraNames.IndexOf(selectedSuraName) + 1;

            DownloadSura(selItemIndex);

            await Navigation.PushAsync( new MainPage(selItemIndex) );

        }

        

    }

    public class SuraItem
    {

        public string sname { get; set; }
        public string scount { get; set; }
        public string sindex { get; set; }

    }
}
