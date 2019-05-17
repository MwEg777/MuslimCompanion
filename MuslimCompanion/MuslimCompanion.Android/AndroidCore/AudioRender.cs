using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MuslimCompanion.Droid.AndroidCore;
using Xamarin.Forms;
using static MuslimCompanion.Core.GeneralManager;
using static Android.Media.MediaPlayer;
using FormsToolkit;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using MuslimCompanion.Droid.Activities;

[assembly: Dependency(typeof(AudioRender))]

namespace MuslimCompanion.Droid.AndroidCore
{
    class AudioRender : Activity, IAudioService
    {

        Context context;

        PauseActivity pauseActivity;

        StopActivity stopActivity;

        ResumeActivity resumeActivity;

        IntentFilter intentFilter;

        public AudioRender()
        {

            pauseActivity = new PauseActivity();
            stopActivity = new StopActivity();
            resumeActivity = new ResumeActivity();

            context = Forms.Context;

            
            //intentFilter.AddAction("RESUME");



            //context.RegisterReceiver(resumeActivity, intentFilter);

        }

        public void PlayAudioFile(string filePath)
        {

            

            var player = new MediaPlayer();
            if (GlobalVar.Get<MediaPlayer>("activeplayer") != null) GlobalVar.Get<MediaPlayer>("activeplayer").Stop();
            GlobalVar.Set("activeplayer", player);
            player.SetDataSource(filePath);
            player.Prepared += (s, e) => { player.Start(); };
            player.Prepare();
            player.SetOnCompletionListener(new OnCompletionHelper());
            MessagingService.Current.SendMessage("AyahPlaybackStarted");

            //Create notification
            var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

            //Use Notification Builder
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

            //Create the notification
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.

            Intent intent = new Intent();

            String message = "Message";

            intent.PutExtra("toastMessage" , message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, 0);

            var actionIntent1 = new Intent();

            actionIntent1.SetAction("PAUSE");

            var pIntent1 = PendingIntent.GetBroadcast(context, 0, actionIntent1, PendingIntentFlags.CancelCurrent);

            var actionIntent2 = new Intent();

            actionIntent2.SetAction("STOP");

            var pIntent2 = PendingIntent.GetBroadcast(context, 0, actionIntent2, PendingIntentFlags.CancelCurrent);

            intentFilter = new IntentFilter();

            intentFilter.AddAction("PAUSE");

            var intentFilter2 = new IntentFilter();

            intentFilter2.AddAction("STOP");

            context.RegisterReceiver(pauseActivity, intentFilter);
            context.RegisterReceiver(stopActivity, intentFilter2);

            var notification = builder.SetContentIntent(PendingIntent.GetActivity(context, 0, intent, 0))
                    .SetSmallIcon(Resource.Drawable.abc_ic_star_black_48dp)
                    .SetTicker(new Java.Lang.String("Test Ticker 1"))
                    .SetContentTitle(new Java.Lang.String("Test Title 1"))
                    .SetContentText(new Java.Lang.String("Test Content 1"))
                    .AddAction(Resource.Drawable.ic_media_pause_dark, "إيقاف مؤقت" , pIntent1)
                    .AddAction(Resource.Drawable.ic_media_stop_dark, "إيقاف" , pIntent2).Build();

            //Show the notification
            //notificationManager.Cancel(2);
            notificationManager.Notify(1, notification);


        }

        public class OnCompletionHelper : Java.Lang.Object, IOnCompletionListener
        {

            public void OnCompletion(MediaPlayer mp)
            {

                MessagingService.Current.SendMessage("AyahPlaybackDone");

            }

        }

        public void StopAudioFile()
        {

            var player = GlobalVar.Get<MediaPlayer>("activeplayer");
            player.Stop();

            //Create notification
            var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

            notificationManager.Cancel(1);
            notificationManager.Cancel(2);


        }

        public void PauseAudioFile()
        {

            var player = GlobalVar.Get<MediaPlayer>("activeplayer");
            player.Pause();

            //Create notification
            var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

            //Use Notification Builder
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

            //Create the notification
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.

            Intent intent = new Intent();

            String message = "Message";

            intent.PutExtra("toastMessage", message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, 0);

            var actionIntent1 = new Intent();

            actionIntent1.SetAction("RESUME");

            var pIntent1 = PendingIntent.GetBroadcast(context, 0, actionIntent1, PendingIntentFlags.CancelCurrent);

            var actionIntent2 = new Intent();

            actionIntent2.SetAction("STOP");

            var pIntent2 = PendingIntent.GetBroadcast(context, 0, actionIntent2, PendingIntentFlags.CancelCurrent);

            intentFilter = new IntentFilter();

            intentFilter.AddAction("RESUME");

            var intentFilter2 = new IntentFilter();

            intentFilter2.AddAction("STOP");

            context.RegisterReceiver(resumeActivity, intentFilter);
            context.RegisterReceiver(stopActivity, intentFilter2);

            var notification = builder.SetContentIntent(PendingIntent.GetActivity(context, 0, intent, 0))
                    .SetSmallIcon(Resource.Drawable.abc_ic_star_black_48dp)
                    .SetTicker(new Java.Lang.String("Test Ticker 2"))
                    .SetContentTitle(new Java.Lang.String("Test Title 2"))
                    .SetContentText(new Java.Lang.String("Test Content 2"))
                    .AddAction(Resource.Drawable.ic_media_play_dark, "تشغيل", pIntent1)
                    .AddAction(Resource.Drawable.ic_media_stop_dark, "إيقاف", pIntent2).Build();

            //Show the notification
            notificationManager.Notify(1, notification);

            //notificationManager.Cancel(1);
        }

        bool firstResume = true, firstPlay = true, firstPause = true;

        public void ResumeAudioFile()
        {

            //Create notification
            var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

            var player = GlobalVar.Get<MediaPlayer>("activeplayer");
            player.Start();

            //Use Notification Builder
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context);

            //Create the notification
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.

            Intent intent = new Intent();

            String message = "Message";

            intent.PutExtra("toastMessage", message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, 0);

            var actionIntent1 = new Intent();

            actionIntent1.SetAction("PAUSE");

            var pIntent1 = PendingIntent.GetBroadcast(context, 0, actionIntent1, PendingIntentFlags.CancelCurrent);

            var actionIntent2 = new Intent();

            actionIntent2.SetAction("STOP");

            var pIntent2 = PendingIntent.GetBroadcast(context, 0, actionIntent2, PendingIntentFlags.CancelCurrent);

            intentFilter = new IntentFilter();

            intentFilter.AddAction("PAUSE");

            var intentFilter2 = new IntentFilter();

            intentFilter2.AddAction("STOP");

            context.RegisterReceiver(pauseActivity, intentFilter);
            context.RegisterReceiver(stopActivity, intentFilter2);

            var notification = builder.SetContentIntent(PendingIntent.GetActivity(context, 0, intent, 0))
                    .SetSmallIcon(Resource.Drawable.abc_ic_star_black_48dp)
                    .SetTicker(new Java.Lang.String("Test Ticker 3"))
                    .SetContentTitle(new Java.Lang.String("Test Title 3"))
                    .SetContentText(new Java.Lang.String("Test Content 3"))
                    .AddAction(Resource.Drawable.ic_media_pause_dark, "إيقاف مؤقت", pIntent1)
                    .AddAction(Resource.Drawable.ic_media_stop_dark, "إيقاف", pIntent2).Build();
                    //.AddAction(new NotificationCompat.Action())


                    ////Set the notification sound
                    //.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))

                    //Auto cancel will remove the notification once the user touches it

            //Show the notification
            //notificationManager.Cancel(2);
            notificationManager.Notify(1, notification);

        }

        public int RetrieveLength(string filePath)
        {
            MediaMetadataRetriever metaRetriever = new MediaMetadataRetriever();
            metaRetriever.SetDataSource(filePath);


            string duration =
            metaRetriever.ExtractMetadata(MetadataKey.Duration);
            long dur = long.Parse(duration);
            int seconds = (int)dur;

            metaRetriever.Release();

            return seconds;
        }

    }

}