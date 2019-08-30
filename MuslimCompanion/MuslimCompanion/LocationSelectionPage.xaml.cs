using MuslimCompanion.Core;
using MuslimCompanion.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MuslimCompanion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationSelectionPage : ContentPage
	{

        public LocationSelectionPage ()
		{
			InitializeComponent ();
            ResultView.ItemTapped += Handle_ItemTapped;

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            string selectedCityName = e.Item.ToString();

            bool nameIsArabic = true;

            if (Regex.IsMatch(selectedCityName, @"[\u0000-\u024F]+"))
                nameIsArabic = false;

            cities selectedCity;

            if (nameIsArabic)
            {
                if (GeneralManager.cities.Exists(x => x.nameAR == selectedCityName))
                    selectedCity = GeneralManager.cities.Find(x => x.nameAR == selectedCityName);
                else if (GeneralManager.cities.Exists(x => x.nameEN == selectedCityName))
                    selectedCity = GeneralManager.cities.Find(x => x.nameEN == selectedCityName);
                else
                    return;
            }
            else
            {

                if (GeneralManager.cities.Exists(x => x.nameEN == selectedCityName))
                    selectedCity = GeneralManager.cities.Find(x => x.nameEN == selectedCityName);
                else if (GeneralManager.cities.Exists(x => x.nameAR == selectedCityName))
                    selectedCity = GeneralManager.cities.Find(x => x.nameAR == selectedCityName);
                else
                    return;
            }

            GlobalVar.Set("longitude", selectedCity.longitude);
            GlobalVar.Set("latitude", selectedCity.latitude);

            GeneralManager.ProcessPrayerTimes(1);

            AzanPage.instance.ResetToLoading();

            Application.Current.Properties["cityname"] = selectedCityName;
            GeneralManager.AppSettings.AddOrUpdateValue("cityname", selectedCityName);

            AzanPage.instance.UpdateLocationText();

            await Navigation.PopAsync();

        }

        void SearchForCities(string toSearch)
        {

            bool showArabicName = true;

            if (Regex.IsMatch(toSearch, @"[\u0000-\u024F]+"))
            { 
                showArabicName = false;
                toSearch = toSearch.ToLower();
            }

            ObservableCollection<string> oc = new ObservableCollection<string>();

            ResultView.ItemsSource = oc;

            if (String.IsNullOrEmpty(toSearch))
                return;

            if (toSearch.Length < 2)
                return;

            foreach (cities city in GeneralManager.cities)
            {

                if (showArabicName && city.nameAR.Contains(toSearch))
                    oc.Add(city.nameAR);
                else if (!showArabicName && city.nameEN.ToLower().Contains(toSearch))
                    oc.Add(city.nameEN);

            }

        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {

            SearchForCities(e.NewTextValue);

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            Navigation.PopAsync();

            AzanPage.instance.ResetToLoading();

            Application.Current.Properties["cityname"] = "موقعك";
            GeneralManager.AppSettings.AddOrUpdateValue("cityname", "موقعك");

            GlobalVar.Set("longitude", 0f);
            GlobalVar.Set("latitude", 0f);

            GeneralManager.ProcessPrayerTimes(1);

            AzanPage.instance.UpdateLocationText();

        }
    }
}