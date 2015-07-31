using System;

using UIKit;
using AVFoundation;
using Foundation;
using MediaPlayer;

namespace MPRemoteCommandCenterSample
{
    public partial class ViewController : UIViewController
    {
        readonly NSUrl audioUrl = NSUrl.FromString("http://vprclassical.streamguys.net/vprclassical128.mp3");

        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var audioSession = AVAudioSession.SharedInstance();
            audioSession.SetCategory(AVAudioSessionCategory.Playback);
            audioSession.OutputChannelsChanged += (sender, e) => Console.WriteLine("Output channels changed");
            audioSession.SetActive(true);

            // This is not working...
            MPRemoteCommandCenter rcc = MPRemoteCommandCenter.Shared;
            rcc.SeekBackwardCommand.Enabled = false;
            rcc.SeekForwardCommand.Enabled = false;
            rcc.NextTrackCommand.Enabled = false;
            rcc.PreviousTrackCommand.Enabled = false;
            rcc.SkipBackwardCommand.Enabled = false;
            rcc.SkipForwardCommand.Enabled = false;

            // You must enable a command so that others can be disabled?
            // See http://stackoverflow.com/a/28925369.
            rcc.PlayCommand.Enabled = true;  

            MPNowPlayingInfo nowPlayingInfo = new MPNowPlayingInfo();
            nowPlayingInfo.AlbumTitle = "Vermont";
            nowPlayingInfo.Artist = "Colchester";
            nowPlayingInfo.Title = "VPR";
            MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = nowPlayingInfo;

            AVPlayer player = new AVPlayer(audioUrl);
            player.Play();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();
            this.BecomeFirstResponder();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            UIApplication.SharedApplication.EndReceivingRemoteControlEvents();
            this.ResignFirstResponder();
        }
            
    }
}

