
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
        DataTemplate suraTemplate;

        public ChooseSura()
        {

            InitializeComponent();
            suraTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();

                var ayahCountLabel = new Label { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

                var ayahIndexLabel = new Label { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

                var suraNameLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, FontSize = 20 };

                var suraDownloadedLabel = new Label { VerticalOptions = LayoutOptions.Center, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center };

                suraNameLabel.SetBinding(Label.TextProperty, "sname");

                ayahCountLabel.SetBinding(Label.TextProperty, "scount");

                ayahIndexLabel.SetBinding(Label.TextProperty, "sindex");

                suraDownloadedLabel.SetBinding(Label.TextProperty, "sdownloaded");

                suraDownloadedLabel.SetBinding(Label.TextColorProperty, "sdownloadcolor");

                grid.Children.Add(ayahIndexLabel, 1, 0);

                grid.Children.Add(ayahCountLabel, 2, 0);

                grid.Children.Add(suraNameLabel, 3, 0);

                grid.Children.Add(suraDownloadedLabel);

                return new ViewCell { View = grid };

            });

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitSuras();
        }

        async void InitSuras(int mode = 0) //0 means sort by sura number.  //1 means sort by sura name. //2 means sort by count of ayah
        {

            if (conn == null)
                InitConnection();

            while (conn == null)
            {

                await Task.Delay(25);

            }

            sura = conn.Table<suranames>().ToList();

            if (!GlobalVar.Get<bool>("initializedsuras", false)) { 

                Items = new ObservableCollection<SuraItem>();

                suraNames = new List<string>();

            }

            for (int i = 0; i < 114; i++)
            {

                if (!GlobalVar.Get<bool>("initializedsuras", false)) { 

                    SuraItem suraItemToAdd = new SuraItem { sname = sura[i].SuraName,
                        sindex = (i + 1).ToString(),
                        scount = suraAyahCounts[i].ToString(),
                        sdownloaded = (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), (i + 1).ToString())) ? "تم تحميلها" : "لم يتم تحميلها"),
                        sdownloadcolor = (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), (i + 1).ToString())) ? Color.Green : Color.Red)
                    };

                    Items.Add(suraItemToAdd);
                    suraNames.Add(sura[i].SuraName);

                }

                else
                { 

                    Items[i].sdownloaded = (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), (i + 1).ToString())) ? "تم تحميلها" : "لم يتم تحميلها");
                    Items[i].sdownloadcolor = (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), (i + 1).ToString())) ? Color.Green : Color.Red);

                }

                if (mode == 2)
                {

                    List<SuraItem> tempList = Items.ToList();
                    tempList = tempList.OrderBy(o => int.Parse(o.scount)).ToList();
                    Items = new ObservableCollection<SuraItem>(tempList);

                }

                else if (mode == 1)
                {

                    List<SuraItem> tempList = Items.ToList();
                    tempList = tempList.OrderBy(o => o.sname).ToList();
                    Items = new ObservableCollection<SuraItem>(tempList);

                }

            }

            GlobalVar.Set("initializedsuras", true);

            ListView lv = new ListView { ItemsSource = Items, ItemTemplate = suraTemplate, Margin = new Thickness(0, 20, 0, 0), };

            lv.ItemTapped += Handle_ItemTapped;

            Grid gr = new Grid();

            var ayahCountLabelName = new Label { HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 40 };

            var ayahIndexLabelName = new Label { HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 40 };

            var suraNameLabelName = new Label { HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 40 };

            var suraDownloadButtonName = new Label { HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 40 };

            suraNameLabelName.Text = "إسم السورة";

            ayahCountLabelName.Text = "عدد آيات السورة";

            ayahIndexLabelName.Text = "رقم السورة";

            suraDownloadButtonName.Text = "حالة التحميل";

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


        

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            string selectedSuraName = ((SuraItem)e.Item).sname;

            int selItemIndex = suraNames.IndexOf(selectedSuraName) + 1;

            //DownloadSura(selItemIndex);

            await Navigation.PushAsync( new MainPage(selItemIndex) );

        }

        private void SortAlphabetically(object sender, EventArgs e)
        {

            InitSuras(1);

        }

        private void SortNumerically(object sender, EventArgs e)
        {

            InitSuras(0);

        }

        private void SortByCount(object sender, EventArgs e)
        {

            InitSuras(2);

        }

    }

    public class SuraItem
    {

        public string sname { get; set; }
        public string scount { get; set; }
        public string sindex { get; set; }
        public string sdownloaded { get; set; }
        public Color sdownloadcolor { get; set; }

    }
}
