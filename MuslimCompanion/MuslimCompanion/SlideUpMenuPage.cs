using SlideOverKit;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MuslimCompanion
{
    public class SlideUpMenuPage : MenuContainerPage, INotifyPropertyChanged
    {
        public SlideUpMenuPage()
        {

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Spacing = 10,
                Children = {
                    new Button{
                        Text ="Show Menu",
                        Command = new Command(()=>{
                            this.ShowMenu();
                        })
                    },
                    new Button{
                        Text ="Hide Menu",
                        Command = new Command(()=>{
                            this.HideMenu();
                        })
                    },
                }
            };

            this.SlideMenu = new SlideUpMenuView();
        }
    }
}
