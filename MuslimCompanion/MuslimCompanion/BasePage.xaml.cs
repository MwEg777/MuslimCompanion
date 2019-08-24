using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MuslimCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasePage : ContentPage
    {
        public BasePage()
        {
            InitializeComponent();
        }

        private void QuranButton_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new HomePage());

        }

        private void AzanButton_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new AzanPage());

        }
    }
}