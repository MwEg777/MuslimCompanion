using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MuslimCompanion.ViewModel
{
    class ChooseSuraVM : INotifyPropertyChanged
    {

        public ICommand SuraNumberCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ChooseSuraVM ()
        {

            SuraNumberCommand = new Command(GetSuraNumber);

        }

        void GetSuraNumber()
        {



        }
    }
}
