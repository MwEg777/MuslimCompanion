using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using FormsToolkit.Droid;
using Android.Content;
using Matcha.BackgroundService.Droid;
using System.Collections.Generic;
using MuslimCompanion.Core;
using MuslimCompanion.Droid.Services;
using Java.Lang;
using Android.Icu.Util;
using FormsToolkit;

namespace MuslimCompanion.Droid
{
    [Activity(Label = "MuslimCompanion", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            BackgroundAggregator.Init(this);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Toolkit.Init();

            string dbPath = FileAccessHelper.GetLocalFilePath("muslimcompanion.db");

            List<string> azanPaths = new List<string>(new string[] { "Azan_Abdulbasit.mp3" });

            List<string> actualAzanPaths = new List<string>();

            //foreach (string azanPath in azanPaths)
                //actualAzanPaths.Add(FileAccessHelper.GetLocalFilePath(azanPath));

            //GeneralManager.azanPaths = actualAzanPaths;

            LoadApplication(new App(dbPath));
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            MessagingService.Current.Subscribe<Tuple<DateTime, int>>("ScheduleAzan", (arg1, arg2) =>
            {

                ScheduleWakeup(arg2.Item1, arg2.Item2);

            });

            //ScheduleWakeup(DateTime.Now.AddSeconds(3), 1);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #region Alarm

        private void ScheduleWakeup(DateTime time, int mode)
        {

            AlarmManager manager = (AlarmManager)GetSystemService(Context.AlarmService);
            Intent myIntent;
            PendingIntent pendingIntent;

            myIntent = new Intent(this, typeof(AlarmNotificationReceiver));

            myIntent.PutExtra("MODE", mode);

            myIntent.SetData(Android.Net.Uri.Parse("myalarms://" + (int)SystemClock.UptimeMillis()));

            TimeSpan difference = time - DateTime.Now;

            if (difference.TotalMilliseconds < 0)
                return;

            pendingIntent = PendingIntent.GetBroadcast(this, (int)SystemClock.UptimeMillis(), myIntent, PendingIntentFlags.OneShot);

            long fireUpTime = SystemClock.ElapsedRealtime() + (long)difference.TotalMilliseconds;

            manager.SetExact(AlarmType.ElapsedRealtimeWakeup, fireUpTime, pendingIntent);


        }

        #endregion

    }

    public class DroidTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        public bool OnTouch(View v, MotionEvent e)
        {
            v.Parent?.RequestDisallowInterceptTouchEvent(true);
            if ((e.Action & MotionEventActions.Up) != 0 && (e.ActionMasked & MotionEventActions.Up) != 0)
            {
                v.Parent?.RequestDisallowInterceptTouchEvent(false);
            }
            return false;
        }
    }

    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            string dbPath = Path.Combine(path, filename);

            CopyDatabaseIfNotExists(dbPath, filename);

            return dbPath;
        }

        private static void CopyDatabaseIfNotExists(string dbPath, string filename)
        {
            if (!File.Exists(dbPath))
            {
                using (var br = new BinaryReader(Application.Context.Assets.Open(filename)))
                {
                    using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, length);
                        }
                    }
                }
            }
        }
    }
}