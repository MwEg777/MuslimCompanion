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
    public partial class HomePage : TabbedPage
    {
        public HomePage ()
        {
            InitializeComponent();

            Children.Add(new ChooseSura());
            Children.Add(new Search());

            Children[0].Title = "Choose Sura";
            Children[1].Title = "Search Ayah";
        }
    }
}