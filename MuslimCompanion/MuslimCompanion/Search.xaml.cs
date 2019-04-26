using MuslimCompanion.Core;
using MuslimCompanion.Model;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MuslimCompanion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Search : ContentPage
	{

        List<QuranNoTashkeel> quran;

        ObservableCollection<string> AyahSearchResults;

        public Search ()
		{

			InitializeComponent ();

            Init();

        }

        async void Init()
        {

            if (GeneralManager.conn == null)
                GeneralManager.InitConnection();

            while (GeneralManager.conn == null)
            {

                await Task.Delay(25);

            }

            quran = GeneralManager.conn.Table<QuranNoTashkeel>().ToList();

            ResultView.ItemTapped += Handle_ItemTapped;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(AyahField.Text))
            {

                SearchForAyah(AyahField.Text);

            }


        }

        void SearchForAyah(string toSearch)
        {

            AyahSearchResults = new ObservableCollection<string>();

            ResultView.ItemsSource = AyahSearchResults;

            foreach (QuranNoTashkeel Ayah in quran)
            {

                if (String.IsNullOrEmpty(Ayah.aya))
                    continue;

                if (Ayah.aya.Contains(toSearch))
                {
                    AyahSearchResults.Add(Ayah.aya);
                }

            }

            

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            AyahSearchResult asr = new AyahSearchResult();

            string selectedAyah = e.Item.ToString();

            foreach (QuranNoTashkeel Ayah in quran)
            {

                if (selectedAyah == Ayah.aya)
                {

                    asr.SuraID = Ayah.sid;
                    asr.AyahID = Ayah.aid;

                }

            }

            await Navigation.PushAsync(new MainPage(asr.SuraID, 1, asr));

        }
    }
}