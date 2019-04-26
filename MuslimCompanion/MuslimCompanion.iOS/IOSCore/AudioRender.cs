using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MuslimCompanion.iOS;
using AVFoundation;
using System.IO;
using Foundation;
using UIKit;
using MuslimCompanion.iOS.IOSCore;
using static MuslimCompanion.Core.GeneralManager;

[assembly: Dependency(typeof(AudioRender))]
namespace MuslimCompanion.iOS.IOSCore
{

    public class AudioRender : IAudioService
    {
        public void PlayAudioFile(string fileName)
        {
            NSError err;
            var player = new AVAudioPlayer(new NSUrl(fileName), "MP3", out err);
            player.FinishedPlaying += delegate
            {
                player = null;
            };
            player.Play();
        }
    }

}