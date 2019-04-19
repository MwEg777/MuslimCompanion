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
        public ObservableCollection<string> Items { get; set; }

        List<suranames> sura;

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

            Items = new ObservableCollection<string>();

            for (int i = 0; i < 114; i++)
            {

                Items.Add(sura[i].SuraName);

            }

            SuraView.ItemsSource = Items;

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            int selItemIndex = Items.IndexOf(e.Item.ToString()) + 1;

            await Navigation.PushAsync( new MainPage(selItemIndex) );

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            //((ListView)sender).SelectedItem = null;

           
        }
    }
}
