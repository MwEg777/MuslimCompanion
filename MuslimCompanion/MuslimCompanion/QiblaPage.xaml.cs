using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using MuslimCompanion.Core;

namespace MuslimCompanion
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QiblaPage : ContentPage
    {

        Vec2 kaabaVector;

        SensorSpeed speed = SensorSpeed.UI;

        double baseNeedleRotation = 0;

        public QiblaPage()
        {
            
            InitializeComponent();
            
            try
            {
                kaabaVector = new Vec2(39.8262d, 21.4225d);
                Vec2 userPositionVector = new Vec2(GeneralManager.muslimPosition.Longitude, GeneralManager.muslimPosition.Latitude);
                baseNeedleRotation = 360 - (Math.Atan2(kaabaVector.Y - userPositionVector.Y, kaabaVector.X - userPositionVector.X) * -180);
                Compass.ReadingChanged += Compass_ReadingChanged;
                Compass.Start(speed, true);
                var distance = new Coordinates(userPositionVector.Y, userPositionVector.X)
                .DistanceTo(
                    new Coordinates(kaabaVector.Y, kaabaVector.X),
                    UnitOfLength.Kilometers
                );

                distanceLabel.Text = "المسافة إلى مكة: " + ( Math.Truncate(distance * 1000) / 1000 ) + " كيلومتر";
            }
            catch
            {
                ShowAlert();
                directionWord.Text = "هاتفك لا يدعم البوصلة، لا يمكن تحديد اتجاه القبلة";
            }
        }

        async void ShowAlert()
        {
            await DisplayAlert("خطأ", "هاتفك لا يدعم البوصلة", "حسنا");
        }

        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var data = e.Reading;
            compassImage.Rotation = 360 - data.HeadingMagneticNorth;
            needleImage.Rotation = 360 - (baseNeedleRotation + data.HeadingMagneticNorth);

            
        }

    }
}