using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FormsToolkit;
using Xamarin.Forms;
using static MuslimCompanion.Core.GeneralManager;

namespace MuslimCompanion.Droid.Activities
{
    [BroadcastReceiver(Label = "PauseReceiver")]
    public class PauseActivity : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {


            MessagingService.Current.SendMessage("PausedFromNotification");

        }
    }

    [BroadcastReceiver(Label = "StopReceiver")]
    public class StopActivity : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {

            MessagingService.Current.SendMessage("StoppedFromNotification");

        }
    }

    [BroadcastReceiver(Label = "ResumeReceiver")]
    public class ResumeActivity : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {

            MessagingService.Current.SendMessage("ResumedFromNotification");

        }
    }
}