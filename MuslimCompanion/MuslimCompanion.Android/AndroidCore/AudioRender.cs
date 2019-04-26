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

[assembly: Dependency(typeof(AudioRender))]

namespace MuslimCompanion.Droid.AndroidCore
{
    class AudioRender : IAudioService
    {

        public void PlayAudioFile(string filePath)
        {
            var player = new MediaPlayer();
            player.SetDataSource(filePath);
            player.Prepared += (s, e) => { player.Start(); };
            player.Prepare();
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