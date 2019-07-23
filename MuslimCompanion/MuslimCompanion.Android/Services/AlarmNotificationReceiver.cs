using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MuslimCompanion.Core;

namespace MuslimCompanion.Droid.Services
{

    [BroadcastReceiver(Enabled = true)]
    class AlarmNotificationReceiver : BroadcastReceiver
    {



        public const string URGENT_CHANNEL = "com.xamarin.myapp.urgent";
        public const int NOTIFY_ID = 1100;

        public override void OnReceive(Context context, Intent intent)
        {

            int mode = intent.GetIntExtra("MODE", 0);

            List<int> azanModes = new List<int> { 1, 2, 3, 4, 5 };

            if (mode == 0)
            { 
                Console.WriteLine("Unhandled Alarm mode! Mode is zero. Parameter was NOT passed.");
                return;
            }

            else if (azanModes.Contains(mode))
            {

                string AzanNotificationTitle = "", AzanNotificationText = "اضغط هنا لإيقاف الآذان", AzanNotificationInfo = "";

                switch (mode)
                {

                    case 1: //Azan Fajr

                        AzanNotificationTitle = "حان الآن موعد آذان الفجر";

                        break;

                    case 2: //Azan Dohr

                        AzanNotificationTitle = "حان الآن موعد آذان الظهر";

                        break;

                    case 3: //Azan Asr

                        AzanNotificationTitle = "حان الآن موعد آذان العصر";

                        break;

                    case 4: //Azan Maghrib

                        AzanNotificationTitle = "حان الآن موعد آذان المغرب";

                        break;

                    case 5: //Azan Isha

                        AzanNotificationTitle = "حان الآن موعد آذان العشاء";

                        break;

                    default: break;

                }

                string azanVoice = "abdulbasit";

                //switch(AppSettings)

                // Create the uri for the alarm file                 
                Android.Net.Uri alarmuri = Android.Net.Uri.Parse("android.resource://" + Application.Context.PackageName + "/raw/" + azanVoice);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

                var resultIntent = new Intent(context, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                resultIntent.PutExtra("PRAYERID", intent.GetIntExtra("PRAYERID", 0));
                var pendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.UpdateCurrent);

                var importance = NotificationImportance.High;
                //string azanPath = GeneralManager.azanPaths[0];

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {

                    // Creating an Audio Attribute
                    var alarmAttributes = new AudioAttributes.Builder()
                        .SetContentType(AudioContentType.Sonification)
                        .SetUsage(AudioUsageKind.Notification).Build();


                    NotificationChannel chan = new NotificationChannel(URGENT_CHANNEL, "Urgent", importance);
                    chan.EnableVibration(true);
                    chan.LockscreenVisibility = NotificationVisibility.Public;
                    chan.SetSound(alarmuri, alarmAttributes);

                    builder.SetAutoCancel(true)
                        .SetDefaults((int)NotificationDefaults.All)
                        .SetSmallIcon(Resource.Drawable.notification_template_icon_bg)
                        .SetContentTitle(AzanNotificationTitle)
                        .SetContentText(AzanNotificationText)
                        .SetContentInfo(AzanNotificationInfo)
                        .SetContentIntent(pendingIntent)
                        .SetSound(alarmuri)
                        //.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                        .SetChannelId(URGENT_CHANNEL)
                        .SetAutoCancel(true);



                    NotificationManager manager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                    manager.CreateNotificationChannel(chan);
                    manager.Notify(NOTIFY_ID, builder.Build());

                }

                else
                {

                    

                    builder.SetAutoCancel(true)
                        //.SetDefaults((int)NotificationDefaults.All)
                        .SetSmallIcon(Resource.Drawable.notification_template_icon_bg)
                        .SetContentTitle(AzanNotificationTitle)
                        .SetContentText(AzanNotificationText)
                        .SetContentInfo(AzanNotificationInfo)
                        .SetContentIntent(pendingIntent)
                        .SetSound(alarmuri)
                        //.SetSound(Android.Net.Uri.Parse(azanPath))
                        .SetAutoCancel(true);

                    NotificationManager manager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                    manager.Notify(NOTIFY_ID, builder.Build());

                }
                

            }

        }

    }
}