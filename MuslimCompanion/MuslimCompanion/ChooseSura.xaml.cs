using MuslimCompanion.Core;
using MuslimCompanion.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            if (GeneralManager.conn == null)
                GeneralManager.InitConnection();

            while (GeneralManager.conn == null)
            {

                await Task.Delay(25);

            }

            sura = GeneralManager.conn.Table<suranames>().ToList();

            Items = new ObservableCollection<SuraItem>();

            suraNames = new List<string>();

            for (int i = 0; i < 114; i++)
            {

                Items.Add(new SuraItem { sname = sura[i].SuraName });
                suraNames.Add(sura[i].SuraName);

            }

            var suraDataTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();

                var suraNameLabel = new Label { FontAttributes = FontAttributes.Bold , FontSize = 20, VerticalOptions = LayoutOptions.Center};

                suraNameLabel.SetBinding(Label.TextProperty, "sname");

                grid.Children.Add(suraNameLabel);

                return new ViewCell { View = grid, Height = 50};

            });

            ListView lv = new ListView { ItemsSource = Items, ItemTemplate = suraDataTemplate, Margin = new Thickness(0, 0, 0, 0), };

            lv.ItemTapped += Handle_ItemTapped;

            Content = new StackLayout
            {
                Margin = new Thickness(20),
                Children = {
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

            await Navigation.PushAsync( new MainPage(selItemIndex) );

           
        }
    }

    public class SuraItem
    {

        public string sname { get; set; }

    }
}
