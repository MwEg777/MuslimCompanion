
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
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static MuslimCompanion.Core.GeneralManager;

namespace MuslimCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseSura : ContentPage, INotifyPropertyChanged
    {
        
        List<suranames> sura;
        List<string> suraNames, suraNamesOriginal;
        DataTemplate suraTemplate;
        bool showingFavs;
        private string showing;

        public string Showing { get { return showing; } set { showing = value; OnPropertyChanged(nameof(Showing)); } }

        public ChooseSura()
        {

            InitializeComponent();

            BindingContext = this;

            Showing = "عرض السور المفضلة";

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
            InitSuras(favsOnly: showingFavs);

            if (showingFavs)
                Showing = "عرض كل السور";
            else
                Showing = "عرض السور المفضلة";

        }

        void ToggleFavs(object sender, EventArgs e)
        {

            showingFavs = !showingFavs;
            InitSuras(favsOnly: showingFavs);
            
            if (showingFavs)
                Showing = "عرض كل السور";
            else
                Showing = "عرض السور المفضلة";

            App.Current.Properties["showingfavs"] = Showing;

        }

        async void InitSuras(int mode = 0, bool favsOnly = false) //0 means sort by sura number.  //1 means sort by sura name. //2 means sort by count of ayah
        {

            if (conn == null)
                InitConnection();

            while (conn == null)
            {

                await Task.Delay(25);

            }

            sura = conn.Table<suranames>().ToList();

            Items = new ObservableCollection<SuraItem>();

            CopyItems = new ObservableCollection<SuraItem>();

            suraNames = new List<string>();

            suraNamesOriginal = new List<string>();

            for (int i = 0; i < 114; i++)
            {

                suraNamesOriginal.Add(sura[i].SuraName);

                SuraItem suraItemToAdd = new SuraItem { sname = sura[i].SuraName,
                    sindex = (i + 1).ToString(),
                    scount = suraAyahCounts[i].ToString(),
                    sdownloaded = (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), (i + 1).ToString())) ? "تم تحميلها" : "لم يتم تحميلها"),
                    sdownloadcolor = (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), (i + 1).ToString())) ? Color.Green : Color.Red)
                };


                if (favsOnly)
                {

                    if (AppSettings.Contains("favsura" + (i+1).ToString()))
                    {
                        

                        if ((string)AppSettings.GetValueOrDefault("favsura" + (i+1).ToString(), "إضافة إلى المفضلة") == "إضافة إلى المفضلة")
                            continue;
                        Items.Add(suraItemToAdd);
                        CopyItems.Add(suraItemToAdd);
                        suraNames.Add(sura[i].SuraName);
                        Items[Items.IndexOf(suraItemToAdd)].sdownloaded = (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), (i + 1).ToString())) ? "تم تحميلها" : "لم يتم تحميلها");
                        Items[Items.IndexOf(suraItemToAdd)].sdownloadcolor = (Directory.Exists(Path.Combine(GlobalVar.Get<string>("quranaudio"), (i + 1).ToString())) ? Color.Green : Color.Red);


                    }
                    else
                        continue;


                }
                else
                {

                    Items.Add(suraItemToAdd);
                    CopyItems.Add(suraItemToAdd);
                    suraNames.Add(sura[i].SuraName);
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

            SearchBar sb = new SearchBar
            {
                Placeholder = "أدخل إسم سورة"
            };

            sb.Behaviors.Add(new TextChangedBehavior());

            Content = new StackLayout
            {
                Margin = new Thickness(10),
                Children = {
                sb,
                gr,
                lv
                }
            };
       
    }

        public class TextChangedBehavior : Behavior<SearchBar>
        {
            protected override void OnAttachedTo(SearchBar bindable)
            {
                base.OnAttachedTo(bindable);
                bindable.TextChanged += Bindable_TextChanged;
            }

            protected override void OnDetachingFrom(SearchBar bindable)
            {
                base.OnDetachingFrom(bindable);
                bindable.TextChanged -= Bindable_TextChanged;
            }

            private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
            {
                //((SearchBar)sender).SearchCommand?.Execute(e.NewTextValue);
                ObservableCollection<SuraItem> NewItems = new ObservableCollection<SuraItem>(CopyItems);
                foreach(SuraItem si in CopyItems)
                {

                    if (!si.sname.Contains(e.NewTextValue))
                        NewItems.Remove(si);

                }
                foreach (SuraItem si in new ObservableCollection<SuraItem>(Items))
                    Items.Remove(si);
                foreach (SuraItem si in NewItems)
                    Items.Add(si);
            }
        }


        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            string selectedSuraName = ((SuraItem)e.Item).sname;

            int selItemIndex = suraNamesOriginal.IndexOf(selectedSuraName) + 1;

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
