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

[assembly: Dependency(typeof(AudioRender))]

namespace MuslimCompanion.Droid.AndroidCore
{
    class AudioRender : IAudioService
    {

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

        }

        public class OnCompletionHelper : Java.Lang.Object, IOnCompletionListener
        {

            public void OnCompletion(MediaPlayer mp)
            {

                MessagingService.Current.SendMessage("AyahPlaybackDone");

            }

        }

        public void PauseAudioFile()
        {

            var player = GlobalVar.Get<MediaPlayer>("activeplayer");
            player.Pause();

        }

        public void ResumeAudioFile()
        {

            var player = GlobalVar.Get<MediaPlayer>("activeplayer");
            player.Start();

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